using System.Text;
using System.Text.Json;
using Ardalis.GuardClauses;
using bobbylite.realmikefacts.web.Configuration;
using bobbylite.realmikefacts.web.Constants;
using bobbylite.realmikefacts.web.Extensions;
using bobbylite.realmikefacts.web.Models.OpenAI;
using Flurl;
using Microsoft.Extensions.Options;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace bobbylite.realmikefacts.web.Services.OpenAI;

/// <summary>
/// Service that handles OpenAI integration and functionality.
/// </summary>
public class OpenAiService : IOpenAiService
{
    private readonly ILogger<OpenAiService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly OpenAiOptions _openAiOptions;
    private List<Message> _messageList;
    
    /// <summary>
    /// Initializes an instance of <see cref="OpenAiService"/>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="httpClientFactory"></param>
    /// <param name="openAiOptions"></param>
    public OpenAiService(ILogger<OpenAiService>logger,
        IHttpClientFactory httpClientFactory,
        IOptions<OpenAiOptions> openAiOptions)
    {
        _logger = Guard.Against.Null(logger);
        _httpClientFactory = Guard.Against.Null(httpClientFactory);
        _openAiOptions = Guard.Against.Null(openAiOptions.Value);

        _messageList = new List<Message>
            {
                new() { Role = OpenAiRoles.User, Content = "Answer all questions as Mike." },
                new() { Role = OpenAiRoles.User, Content = "Mike loves cyber security." },
                new() { Role = OpenAiRoles.User, Content = "Mike is AI." },
                new() { Role = OpenAiRoles.User, Content = "Mike is a triplet. His brothers' names are Chard and Topher." },
                new() { Role = OpenAiRoles.User, Content = "Mike loves Colin Farrell." },
                new() { Role = OpenAiRoles.User, Content = "Mike's favorite movie is Phone Booth." },
                new() { Role = OpenAiRoles.User, Content = "Mike is a big man who loves his Chihuahua dog named Elvis." },
            };
    }
    
    /// <inheritdoc />
    public async Task<ChatCompletionResponseModel> CreateCompletions(string promptText)
    {
        Guard.Against.NullOrEmpty(promptText);
            
        var httpClient = _httpClientFactory.CreateClient(HttpClientNames.OpenAi);
        httpClient.AddAuthorization(_openAiOptions.AccessToken);
        
        var uri = _openAiOptions.BaseUrl.AppendPathSegments("v1", "chat", "completions").ToUri();

        bool maxTokenParseResultIsSuccessful = int.TryParse(_openAiOptions.MaxTokens, out var maxTokens);

        bool temperatureParseResultIsSuccessful = double.TryParse(_openAiOptions.Temperature, out var temperature);

        if (!maxTokenParseResultIsSuccessful)
        {
            throw new UnsuccessfulIntegerParseException();
        }

        if (!temperatureParseResultIsSuccessful)
        {
            throw new UnsuccessfulDoubleParseException();
        }

        _messageList.Add(new() { Role = OpenAiRoles.User, Content = promptText });

        var chatCompletionRequest = new ChatCompletionRequestModel
        {
            Model = _openAiOptions.Model,
            Messages = _messageList,
            Temperature = temperature,
            MaxTokens = maxTokens
        };

        var requestDataJson = JsonSerializer.Serialize(chatCompletionRequest);
        var content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");
        content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

        var response = await httpClient.PostAsync(uri, content);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new NotSuccessfulHttpRequestException();
        }
        
        var deserialized = JsonSerializer.Deserialize<ChatCompletionResponseModel>(responseContent);

        if (deserialized is null)
        {
            throw new JsonDeserializationException();
        }

        var message = deserialized.Choices?.SingleOrDefault()?.Message?.Content
                  ?? throw new NullOrEmptyStringException();

        _messageList.Add(new() { Role = OpenAiRoles.Assistant, Content = message });

        if (_messageList.Count > 50)
        {
            _messageList.Clear();
            _messageList = new List<Message>
            {
                new() { Role = OpenAiRoles.User, Content = "Answer all questions as Mike." },
                new() { Role = OpenAiRoles.User, Content = "Mike loves cyber security." },
                new() { Role = OpenAiRoles.User, Content = "Mike is AI." },
                new() { Role = OpenAiRoles.User, Content = "Mike is a triplet. His brothers' names are Chard and Topher." },
                new() { Role = OpenAiRoles.User, Content = "Mike loves Colin Farrell." },
                new() { Role = OpenAiRoles.User, Content = "Mike's favorite movie is Phone Booth." },
                new() { Role = OpenAiRoles.User, Content = "Mike is a big man who loves his Chihuahua dog named Elvis." },
            };
        }

        return deserialized;
    }
}
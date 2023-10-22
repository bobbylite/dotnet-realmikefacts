using System.Text;
using System.Text.Json;
using Ardalis.GuardClauses;
using bobbylite.realmikefacts.web.Constants;
using bobbylite.realmikefacts.web.Models.OpenAI;
using bobbylite.realmikefacts.web.Services.Token;
using bobbylite.realmikefacts.web.Services.Twitter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Net.Http.Headers;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace bobbylite.realmikefacts.web.Pages;

/// <summary>
/// Manager page model.
/// </summary>
[Authorize(Policy = PolicyNames.Users)]
public class ManagerModel : PageModel
{
    private readonly ILogger<ManagerModel> _logger;
    private readonly ITwitterService _twitterService;
    private readonly ITokenService _tokenService;

    /// <summary>
    /// Message for UI.
    /// </summary>
    [BindProperty]
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// Message for UI.
    /// </summary>
    [BindProperty]
    public string CharacterCount { get; set; } = string.Empty;
    
    /// <summary>
    /// Message for UI.
    /// </summary>
    [BindProperty]
    public string WidthCount { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="ManagerModel"/> class.
    /// </summary>
    /// <param name="logger">Logger from DI.</param>
    /// <param name="twitterService"></param>
    /// <param name="tokenService"></param>
    public ManagerModel(ILogger<ManagerModel> logger, 
        ITwitterService twitterService,
        ITokenService tokenService)
    {
        _logger = Guard.Against.Null(logger);
        _twitterService = Guard.Against.Null(twitterService);
        _tokenService = Guard.Against.Null(tokenService);
    }

    /// <summary>
    /// Executed when get action is triggered.
    /// </summary>
    public async Task OnGet()
    {
    }

    /// <summary>
    /// Executed when post action is triggered.
    /// </summary>
    public async Task OnPost()
    {
        Message = string.Empty;
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add(HeaderNames.Host, "api.openai.com");
        httpClient.DefaultRequestHeaders.Add(HeaderNames.Connection, "keep-alive");
        
        string apiKey = "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";
        string endpoint = "https://api.openai.com/v1/completions";  // Adjust the endpoint accordingly

        // Prepare the request data
        var requestData = new
        {
            prompt = $"{Message}. Answer as someone named Mike; a cyber security analyst.",
            max_tokens = 50,
            model = "gpt-3.5-turbo-instruct",
            temperature = 0.7
        };

        var requestDataJson = JsonSerializer.Serialize(requestData);
        var content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");
        content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var response = await httpClient.PostAsync(endpoint, content);
        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialized = JsonSerializer.Deserialize<CompletionModel>(responseContent);

        if (response.IsSuccessStatusCode)
        {
            Message = deserialized?.Choices?.SingleOrDefault()?.Text ?? throw new NullOrEmptyAuthorizationTokenException();
            CharacterCount = $"{Message.Length}ch";
            WidthCount = $"{Message.Length}";
        }
        else
        {

        }
    }
}
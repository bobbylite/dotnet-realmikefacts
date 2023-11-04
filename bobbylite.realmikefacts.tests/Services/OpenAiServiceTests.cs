using System.Net;
using System.Text.Json;
using Ardalis.GuardClauses;
using bobbylite.realmikefacts.web.Configuration;
using bobbylite.realmikefacts.web.Constants;
using bobbylite.realmikefacts.web.Models.OpenAI;
using bobbylite.realmikefacts.web.Services.OpenAI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Message = bobbylite.realmikefacts.web.Models.OpenAI.Message;


namespace bobbylite.realmikefacts.tests.Services;

public class OpenAiServiceTests
{
    private readonly OpenAiService _openAiService;
    
    public OpenAiServiceTests()
    {
        Mock<ILogger<OpenAiService>> logger = new();
        var response = new ChatCompletionResponseModel
        {
            Choices = new List<Choice>
            {
                new Choice
                {
                    Message = new Message
                    {
                        Content = "foobar"
                    }
                }
            }
        };
        var mockHttpHandler = CreateMockHttpMessageHandler(HttpStatusCode.OK, JsonSerializer.Serialize(response));
        Mock<IHttpClientFactory> httpClientFactory = CreateMockHttpClientFactory(mockHttpHandler.Object, HttpClientNames.OpenAi);

        IOptions<OpenAiOptions> openAiOptions = Options.Create(new OpenAiOptions
        {
            Model = "test",
            Temperature = ".8",
            AccessToken = string.Empty,
            BaseUrl = "https://example.server",
            MaxTokens = "10"
        });

        _openAiService = new OpenAiService(logger.Object, httpClientFactory.Object, openAiOptions);
    }

    [Fact]
    public async Task CreateCompletions_WithValidData_ReturnsChatCompletionResponseModel()
    {
        // Arrange
        const string expected = "foobar";
        
        // Act
        var result = await _openAiService.CreateCompletions("example-prompt");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected, result.Choices?[0].Message?.Content);
    }
    
    private static Mock<HttpMessageHandler> CreateMockHttpMessageHandler(HttpStatusCode statusCode, string content)
    {
        Guard.Against.Null(statusCode);
        Guard.Against.Null(content);
        
        Mock<HttpMessageHandler> mock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        mock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(new HttpResponseMessage()
        {
            StatusCode = statusCode,
            Content = new StringContent(content)
        }).Verifiable();
        return mock;
    }
    
    private static Mock<IHttpClientFactory> CreateMockHttpClientFactory(HttpMessageHandler httpMessageHandler, string httpClientName)
    {
        ArgumentNullException.ThrowIfNull(httpMessageHandler);
        ArgumentNullException.ThrowIfNull(httpClientName);
        
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        var httpClient = new HttpClient(httpMessageHandler);
        mockHttpClientFactory.Setup(m => m.CreateClient(httpClientName))
            .Returns(httpClient);

        return mockHttpClientFactory;
    }
}
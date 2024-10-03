using System.Net.Http.Headers;
using System.Text.Json;
using Ardalis.GuardClauses;
using bobbylite.realmikefacts.web.Constants;
using bobbylite.realmikefacts.web.Extensions;
using bobbylite.realmikefacts.web.Models.Token;
using bobbylite.realmikefacts.web.Services.Token;
using Flurl;
using Microsoft.Extensions.Options;
using TwitterOptions = bobbylite.realmikefacts.web.Configuration.TwitterOptions;

namespace bobbylite.realmikefacts.web.Services.Twitter;

/// <summary>
/// Service for sending HTTP requests to twitter application.
/// </summary>
public class TwitterService : ITwitterService
{
    private readonly ILogger<TwitterService> _logger;
    private readonly ITokenService _tokenService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly TwitterOptions _twitterOptions;
    private int _tweetCount = 326;

    /// <summary>
    /// Initializes an instance of <see cref="ITwitterService"/>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="httpClientFactory"></param>
    /// <param name="tokenService"></param>
    /// <param name="twitterOptions"></param>
    public TwitterService(ILogger<TwitterService> logger, 
        IHttpClientFactory httpClientFactory, 
        ITokenService tokenService,
        IOptions<TwitterOptions> twitterOptions)
    {
        _httpClientFactory = httpClientFactory;
        _logger = Guard.Against.Null(logger);
        _tokenService = Guard.Against.Null(tokenService);
        _twitterOptions = Guard.Against.Null(twitterOptions.Value);
    }

    /// <inheritdoc />
    public async Task Tweet(string tweetMessage)
    {
        await _tokenService.RefreshAccessToken();

        var tweet = new TweetModel
        {
            Text = $"Mike Fact #{_tweetCount}: {tweetMessage}"
        };
        var serializedTweet = JsonSerializer.Serialize(tweet);
        var content = new StringContent(serializedTweet);

        content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        
        HttpClient httpClient = _httpClientFactory.CreateClient(HttpClientNames.TwitterApi);
        if (string.IsNullOrEmpty(_tokenService?.Token?.AccessToken!))
        {
            _logger.LogInformation("Token is null. Setting access token.");
        }
        httpClient.AddTwitterAuthorization(_tokenService?.Token?.AccessToken!);

        string requestUri = _twitterOptions.BaseUrl
            .AppendPathSegments("2", "tweets");
        
        var httpResponseMessage = await httpClient.PostAsync(requestUri, content);
        var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
        
        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            _logger.LogError("Authentication operation was unsuccessful");
        }

        _tweetCount++;
        _logger.LogInformation("Successful tweet operation: {TweetResponse}", responseContent);
    }
}
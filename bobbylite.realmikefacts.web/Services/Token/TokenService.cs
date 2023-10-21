using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Ardalis.GuardClauses;
using bobbylite.realmikefacts.web.Configuration;
using bobbylite.realmikefacts.web.Constants;
using bobbylite.realmikefacts.web.Extensions;
using bobbylite.realmikefacts.web.Models.Token;
using Flurl;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Environment = System.Environment;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace bobbylite.realmikefacts.web.Services.Token;

/// <summary>
/// Service for token support.
/// </summary>
public class TokenService : ITokenService
{
    private readonly ILogger<TokenService> _logger;
    private readonly TwitterOptions _twitterOptions;
    private readonly IHttpClientFactory _httpClientFactory;

    /// <summary>
    /// Token model for creating PingOne API requests. 
    /// </summary>
    public TokenEndpointResponse? Token { get; set; } 

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="twitterOptions"></param>
    /// <param name="httpClientFactory"></param>
    public TokenService(ILogger<TokenService> logger,
        IOptions<TwitterOptions> twitterOptions,
        IHttpClientFactory httpClientFactory)
    {
        _logger = Guard.Against.Null(logger);
        _twitterOptions = Guard.Against.Null(twitterOptions.Value);
        _httpClientFactory = Guard.Against.Null(httpClientFactory);
    }
    
    /// <inheritdoc />
    public async Task SetAccessToken()
    {
        var urlEncodedFormData = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("grant_type", "client_credentials") };
        var content = new FormUrlEncodedContent(urlEncodedFormData);
        content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
        
        HttpClient httpClient = _httpClientFactory.CreateClient(HttpClientNames.TwitterApi);
        httpClient.AddBasicAuthorization(_twitterOptions.ClientId, _twitterOptions.ClientSecret);

        string requestUri = _twitterOptions.BaseUrl
            .AppendPathSegments("oauth2", "token");
        
        var httpResponseMessage = await httpClient.PostAsync(requestUri, content);
        var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
        
        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            _logger.LogError("Authentication operation was unsuccessful");
        }

        _logger.LogInformation("Authentication operation was successful");

        var tokenResponse = JsonSerializer.Deserialize<TokenEndpointResponse>(responseContent);

        Token = tokenResponse ?? throw new Exception("ErrorAuthenticating");
    }
}
using Ardalis.GuardClauses;
using bobbylite.realmikefacts.web.Services.Token;
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

    /// <summary>
    /// Initializes an instance of <see cref="ITwitterService"/>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="httpClientFactory"></param>
    /// <param name="tokenService"></param>
    public TwitterService(ILogger<TwitterService> logger, 
        IHttpClientFactory httpClientFactory, 
        ITokenService tokenService)
    {
        _httpClientFactory = httpClientFactory;
        _logger = Guard.Against.Null(logger);
        _tokenService = Guard.Against.Null(tokenService);
    }

    /// <inheritdoc />
    public void Tweet()
    {
    }
}
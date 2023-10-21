using Ardalis.GuardClauses;
using bobbylite.realmikefacts.web.Configuration;
using bobbylite.realmikefacts.web.Constants;
using bobbylite.realmikefacts.web.Extensions;
using bobbylite.realmikefacts.web.Services.Token;
using Microsoft.Extensions.Options;

namespace bobbylite.realmikefacts.web.Services.Twitter;

/// <summary>
/// Service for sending HTTP requests to twitter application.
/// </summary>
public class TwitterService : ITwitterService
{
    private readonly ILogger<TwitterService> _logger;
    private readonly ITokenService _tokenService;

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
        _logger = Guard.Against.Null(logger);
        _tokenService = Guard.Against.Null(tokenService);
    }

    /// <inheritdoc />
    public async Task Tweet()
    {
    }
}
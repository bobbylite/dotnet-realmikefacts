using Ardalis.GuardClauses;
using bobbylite.realmikefacts.web.Constants;
using bobbylite.realmikefacts.web.Services.Token;
using bobbylite.realmikefacts.web.Services.Twitter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace bobbylite.realmikefacts.web.Pages;

/// <summary>
/// Twitter page model.
/// </summary>
[Authorize(Policy = PolicyNames.BetaTestersGroup)]
[Authorize(Policy = PolicyNames.Users)]
public class TwitterModel : PageModel
{
    private readonly ILogger<UserModel> _logger;
    private readonly ITwitterService _twitterService;
    private readonly ITokenService _tokenService;

    /// <summary>
    /// Gets or sets the tweet message.
    /// </summary>
    [BindProperty]
    public string TweetMessage { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="TwitterModel"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="twitterService"></param>
    /// <param name="tokenService"></param>
    public TwitterModel(ILogger<UserModel> logger, 
        ITwitterService twitterService,
        ITokenService tokenService)
    {
        _logger = Guard.Against.Null(logger);
        _twitterService = Guard.Against.Null(twitterService);
        _tokenService = Guard.Against.Null(tokenService);
    }

    /// <summary>
    /// Handles the GET request.
    /// </summary>
    public void OnGet()
    {
    }

    /// <summary>
    /// Handles the POST request.
    /// </summary>
    public async Task OnPost()
    {
        _logger.LogInformation("Sending Tweet: {TweetMessage}", TweetMessage);
        await _twitterService.Tweet(TweetMessage);
    }
}
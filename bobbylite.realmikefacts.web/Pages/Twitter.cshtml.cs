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
    public TwitterModel(ILogger<UserModel> logger, 
    ITwitterService twitterService)
{
    _logger = Guard.Against.Null(logger);
    _twitterService = Guard.Against.Null(twitterService);
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
    public void OnPost()
    {
        _logger.LogInformation("Tweeting: {TweetMessage}", TweetMessage);
    }
}
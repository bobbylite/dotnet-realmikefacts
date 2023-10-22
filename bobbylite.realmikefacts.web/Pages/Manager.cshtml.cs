using Ardalis.GuardClauses;
using bobbylite.realmikefacts.web.Constants;
using bobbylite.realmikefacts.web.Services.Token;
using bobbylite.realmikefacts.web.Services.Twitter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
    public string Message { get; set; } = string.Empty;

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
        await _tokenService.SetAccessToken();
        Message = _tokenService.Token.AccessToken!;
    }
}
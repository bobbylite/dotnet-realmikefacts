using Ardalis.GuardClauses;
using bobbylite.realmikefacts.web.Constants;
using bobbylite.realmikefacts.web.Services.OpenAI;
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
    private readonly IOpenAiService _openAiService;
    private readonly ILogger<ManagerModel> _logger;
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
    /// <param name="openAiService"></param>
    /// <param name="logger">Logger from DI.</param>
    /// <param name="tokenService"></param>
    public ManagerModel(IOpenAiService openAiService,
        ILogger<ManagerModel> logger, 
        ITokenService tokenService)
    {
        _openAiService = Guard.Against.Null(openAiService);
        _logger = Guard.Against.Null(logger);
        _tokenService = Guard.Against.Null(tokenService);
    }

    /// <summary>
    /// Executed when get action is triggered.
    /// </summary>
    public async Task OnGet()
    {
        _logger.LogInformation("GET - {PageModel}", nameof(ManagerModel));
    }

    /// <summary>
    /// Executed when post action is triggered.
    /// </summary>
    public async Task OnPost()
    {
        _logger.LogInformation("POST - {PageModel}", nameof(ManagerModel));
        
        var completionResult = await _openAiService.CreateCompletions(Message);
        Message = completionResult?.Choices?.SingleOrDefault()?.Text 
                  ?? throw new NullOrEmptyStringException();
        CharacterCount = $"{Message.Length}ch";
        WidthCount = $"{Message.Length}";
        
        _logger.LogInformation("Completed OnPost successfully.");
    }
}
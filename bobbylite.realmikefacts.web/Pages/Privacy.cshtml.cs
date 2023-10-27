using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace bobbylite.realmikefacts.web.Pages;

/// <summary>
/// Privacy.
/// </summary>
public class PrivacyModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PrivacyModel"/> class.
    /// </summary>
    /// <param name="logger">Logger from DI.</param>
    public PrivacyModel(ILogger<PrivacyModel> logger)
    {
        _logger = Guard.Against.Null(logger);
    }

    /// <summary>
    /// Runs on get.
    /// </summary>
    public void OnGet()
    {
    }
}
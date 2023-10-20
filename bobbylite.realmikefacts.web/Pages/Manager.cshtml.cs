using bobbylite.realmikefacts.web.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace bobbylite.realmikefacts.web.Pages;

/// <summary>
/// Manager page model.
/// </summary>
[Authorize(Policy = PolicyNames.Managers)]
public class ManagerModel : PageModel
{
    private readonly ILogger<ManagerModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ManagerModel"/> class.
    /// </summary>
    /// <param name="logger">Logger from DI.</param>
    public ManagerModel(ILogger<ManagerModel> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Runs on get.
    /// </summary>
    public void OnGet()
    {
    }
}
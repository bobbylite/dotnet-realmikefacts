using bobbylite.realmikefacts.web.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace bobbylite.realmikefacts.web.Pages;

/// <summary>
/// Administrators page model.
/// </summary>
[Authorize(Policy = PolicyNames.AdministratorsGroup)]
public class AdministratorsModel : PageModel
{
    private readonly ILogger<AdministratorsModel> _logger;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AdministratorsModel"/> class.
    /// </summary>
    /// <param name="logger">Logger from DI.</param>
    public AdministratorsModel(ILogger<AdministratorsModel> logger)
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
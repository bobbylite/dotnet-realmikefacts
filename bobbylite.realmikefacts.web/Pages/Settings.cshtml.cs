using bobbylite.realmikefacts.web.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace bobbylite.realmikefacts.web.Pages;

/// <summary>
/// Settings razor page model.
/// </summary>
[Authorize(Policy = PolicyNames.Users)]
public class Settings : PageModel
{
    /// <summary>
    /// GET method executed.
    /// </summary>
    public void OnGet()
    {
        
    }
}
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace bobbylite.realmikefacts.web.Pages;

/// <summary>
/// Index page model.
/// </summary>
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="IndexModel"/> class.
    /// </summary>
    /// <param name="logger">Logger from DI.</param>
    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Runs on get.
    /// </summary>
    public void OnGet()
    {
        var cookie = HttpContext.Request.Cookies[".AspNetCore.Custom.Auth.Cookies"];
        bool isAuthenticated = User.Identity?.IsAuthenticated ?? false;
        if ((!isAuthenticated) && (cookie is not null))
        {
            HttpContext.Response.Cookies.Delete(".AspNetCore.Custom.Auth.Cookies");
        }
    }
}

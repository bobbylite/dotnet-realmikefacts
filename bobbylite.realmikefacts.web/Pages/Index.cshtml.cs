using System.Text;
using System.Text.Json;
using bobbylite.realmikefacts.web.Models.Authorization;
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
        const string cookieKey = ".AspNetCore.Custom.Auth.Cookies";
        const string nameIdentifierKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        
        var cookie = HttpContext.Request.Cookies[cookieKey];
        if (cookie is null)
        {
            return;
        }

        bool isMyCookie = false;
        
        byte[] bytes = Convert.FromBase64String(cookie);
        var serializedJson = Encoding.ASCII.GetString(bytes);
        var groupAuthorizationModel = JsonSerializer.Deserialize<GroupAuthorizationModel>(serializedJson);
        
        if (groupAuthorizationModel is null)
        {
            throw new NullObjectException();
        }
        
        var userId = HttpContext.User.FindFirst(c => c.Type == nameIdentifierKey)!.Value;
        isMyCookie = userId == groupAuthorizationModel.UserId;
        bool isAuthenticated = User.Identity?.IsAuthenticated ?? false;
        
        if (!isAuthenticated || !isMyCookie)
        {
            
            HttpContext.Response.Cookies.Delete(".AspNetCore.Custom.Auth.Cookies");
        }
    }
}

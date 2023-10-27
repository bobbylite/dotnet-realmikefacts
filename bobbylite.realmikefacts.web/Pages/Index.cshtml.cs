using System.Text;
using System.Text.Json;
using Ardalis.GuardClauses;
using bobbylite.realmikefacts.web.Models.Authorization;
using bobbylite.realmikefacts.web.Services.Cookie;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace bobbylite.realmikefacts.web.Pages;

/// <summary>
/// Index page model.
/// </summary>
public class IndexModel : PageModel
{
    const string NameIdentifierKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
    
    private readonly ILogger<IndexModel> _logger;
    private readonly IAuthorizationCookieService _authorizationCookieService;

    /// <summary>
    /// Initializes a new instance of the <see cref="IndexModel"/> class.
    /// </summary>
    /// <param name="logger">Logger from DI.</param>
    /// <param name="authorizationCookieService"></param>
    public IndexModel(ILogger<IndexModel> logger, IAuthorizationCookieService authorizationCookieService)
    {
        _logger = Guard.Against.Null(logger);
        _authorizationCookieService = Guard.Against.Null(authorizationCookieService);
    }

    /// <summary>
    /// Runs on get.
    /// </summary>
    public void OnGet()
    {
        if (_authorizationCookieService.DoesCookieExist() is false)
        {
            return;
        }

        bool isMyCookie = false;

        var cookie = _authorizationCookieService.GetCookie();
        byte[] bytes = Convert.FromBase64String(cookie);
        var serializedJson = Encoding.ASCII.GetString(bytes);
        var groupAuthorizationModel = JsonSerializer.Deserialize<GroupAuthorizationModel>(serializedJson);
        
        if (groupAuthorizationModel is null)
        {
            throw new NullObjectException();
        }

        var userId = HttpContext.User.FindFirst(c => c.Type == NameIdentifierKey);

        if (userId is null)
        {
            return;
        }
        
        isMyCookie = userId.Value == groupAuthorizationModel.UserId;
        bool isAuthenticated = User.Identity?.IsAuthenticated ?? false;
        
        if (!isAuthenticated || !isMyCookie)
        {
            _authorizationCookieService.DeleteCookie();
        }
    }
}

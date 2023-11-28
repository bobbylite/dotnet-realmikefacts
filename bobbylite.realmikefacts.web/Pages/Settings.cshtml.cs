using System.Globalization;
using Ardalis.GuardClauses;
using bobbylite.realmikefacts.web.Constants;
using bobbylite.realmikefacts.web.Models.AccessRequest;
using bobbylite.realmikefacts.web.Services.AccessRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace bobbylite.realmikefacts.web.Pages;

/// <summary>
/// Settings razor page model.
/// </summary>
[Authorize(Policy = PolicyNames.Users)]
public class Settings : PageModel
{
    private readonly ILogger<Settings> _logger;
    private readonly AccessRequestService _accessRequestService;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AdministratorsModel"/> class.
    /// </summary>
    /// <param name="logger">Logger from DI.</param>
    /// <param name="accessRequestService"></param>
    public Settings(ILogger<Settings> logger, IAccessRequestService accessRequestService)
    {
        _logger = Guard.Against.Null(logger);
        Guard.Against.Null(accessRequestService);
        
        _accessRequestService = (AccessRequestService)accessRequestService;
    }

    /// <summary>
    /// GET method executed.
    /// </summary>
    public void OnGet()
    {
        
    }

    /// <summary>
    /// POST method executed.
    /// </summary>
    /// <returns></returns>
    public async Task OnPost()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        
        if (!string.IsNullOrEmpty(userId))
        {
            var requestedResource = new GroupResourceReqeuest
            {
                Id = "ff9a9b37-2e83-47a9-98ad-eed35d8ca2de",
                DisplayName = "Administrators"
            };
            var accessRequestId = await _accessRequestService.CreateRequest(new AccessRequestModel
            {
                Id = Guid.NewGuid().ToString(),
                RequestedResource = requestedResource,
                Timestamp = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                UserId = userId,
                DisplayName = User.Identity?.Name
            });

            return;
        }
    }
}
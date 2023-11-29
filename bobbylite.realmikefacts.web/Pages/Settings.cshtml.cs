using System.Globalization;
using System.Text;
using System.Text.Json;
using Ardalis.GuardClauses;
using bobbylite.realmikefacts.web.Constants;
using bobbylite.realmikefacts.web.Models.AccessRequest;
using bobbylite.realmikefacts.web.Models.Authorization;
using bobbylite.realmikefacts.web.Services.AccessRequest;
using Microsoft.AspNetCore.Authorization;
using bobbylite.realmikefacts.web.Services.Cookie;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Graph;
using bobbylite.realmikefacts.web.Services.Graph;
using Microsoft.Graph.Models;
using Microsoft.AspNetCore.Mvc;

namespace bobbylite.realmikefacts.web.Pages;

/// <summary>
/// Settings razor page model.
/// </summary>
[Authorize(Policy = PolicyNames.Users)]
public class Settings : PageModel
{
    private readonly ILogger<Settings> _logger;
    private readonly AccessRequestService _accessRequestService;
    private readonly IAuthorizationCookieService _authorizationCookieService;
    private readonly IGraphService _graphService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdministratorsModel"/> class.
    /// </summary>
    /// <param name="logger">Logger from DI.</param>
    /// <param name="accessRequestService"></param>
    /// <param name="authorizationCookieService"></param>
    /// <param name="graphService"></param>
    public Settings(ILogger<Settings> logger, 
        IAccessRequestService accessRequestService,
        IAuthorizationCookieService authorizationCookieService,
        IGraphService graphService)
    {
        _authorizationCookieService = Guard.Against.Null(authorizationCookieService);
        _logger = Guard.Against.Null(logger);
        _graphService = Guard.Against.Null(graphService);
        Guard.Against.Null(accessRequestService);
        
        _accessRequestService = (AccessRequestService)accessRequestService;
        Groups = new List<Group>();
    }

    /// <summary>
    /// Groups list.
    /// </summary>
    [BindProperty] 
    public IEnumerable<Group> Groups { get; set;}

    /// <summary>
    /// Group Ids.
    /// </summary>
    [BindProperty]
    public string GroupInformation { get; set;} = string.Empty;

    /// <summary>
    /// GET method executed.
    /// </summary>
    public async Task<IActionResult> OnGet()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        
        if (string.IsNullOrEmpty(userId))
        {
            throw new NullOrEmptyStringException(nameof(userId));
        }
        

        var groupCollection = await _graphService.GetAllAvailableGroups();
        var groups = groupCollection?.Value?.ToList();

        if (groups is null)
        {
            return Page();
        }

        var groupMemberships = await _graphService.GetAllGroupMemberships(userId);

        if (groupMemberships is null || groupMemberships.ToList().Count == 0)
        {
            Groups = groups;
            return Page();
        }

        foreach(var groupMembership in groupMemberships.ToList())
        {
            groups.RemoveAll(g => g.Id == groupMembership.Id);
        }

        Groups = groups;
        return Page();
    }

    /// <summary>
    /// POST method executed.
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> OnPost()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        var groupInformation = JsonSerializer.Deserialize<GroupInformation>(GroupInformation);

        if (groupInformation is null)
        { 
            throw new NullObjectException(nameof(groupInformation));
        }

        if (!string.IsNullOrEmpty(userId))
        {
            var requestedResource = new GroupResourceReqeuest
            {
                Id = groupInformation.Id,
                DisplayName = groupInformation.DisplayName
            };
            var accessRequestId = await _accessRequestService.CreateRequest(new AccessRequestModel
            {
                Id = Guid.NewGuid().ToString(),
                RequestedResource = requestedResource,
                Timestamp = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                UserId = userId,
                DisplayName = User.Identity?.Name
            });
        }
        
        var groupCollection = await _graphService.GetAllAvailableGroups();
        var groups = groupCollection?.Value?.ToList();

        if (groups is null)
        {
            return Page();
        }

        var requests = _accessRequestService.GetAllAccessRequests();

        foreach(var accessRequest in requests.Values)
        {
            if (userId != accessRequest.UserId)
            {
                continue;
            }

            groups.RemoveAll(g => g.Id == accessRequest.RequestedResource?.Id);
        }

        Groups = groups;

        return Page();
    }

    /// <summary>
    /// Gets the pending access requests
    /// </summary>
    public IEnumerable<AccessRequestModel> GetPendingAccessRequests()
    { 
        var userId = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        
        var usersAccessRequests = _accessRequestService.GetAllAccessRequests().Values
            .Where(r => r.UserId == userId).ToList();

        if (usersAccessRequests is null)
        {
             throw new NullObjectException(nameof(usersAccessRequests));
        }

        return usersAccessRequests;
    }
}
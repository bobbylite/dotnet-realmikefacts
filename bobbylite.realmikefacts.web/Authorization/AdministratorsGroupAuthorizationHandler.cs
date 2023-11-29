using Ardalis.GuardClauses;
using bobbylite.realmikefacts.web.Constants;
using bobbylite.realmikefacts.web.Models.Authorization;
using bobbylite.realmikefacts.web.Services.Cookie;
using bobbylite.realmikefacts.web.Services.Graph;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Graph.Models;

namespace bobbylite.realmikefacts.web.Authorization;

/// <summary>
/// Authorization handler for administrators group policy.
/// </summary>
public class AdministratorsGroupAuthorizationHandler : AuthorizationHandler<AdministratorsGroupRequirement>
{
    const string NameIdentifierKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
    const string GroupId = GroupIds.Administrators;
    
    private readonly IGraphService _graphService;
    private readonly IAuthorizationCookieService _authorizationCookieService;

    /// <summary>
    /// Initializes an instance of <see cref="AdministratorsGroupAuthorizationHandler"/>
    /// </summary>
    /// <param name="graphService"></param>
    /// <param name="authorizationCookieService"></param>
    public AdministratorsGroupAuthorizationHandler(
        IGraphService graphService, 
        IAuthorizationCookieService authorizationCookieService)
    {
        _graphService = Guard.Against.Null(graphService);
        _authorizationCookieService = Guard.Against.Null(authorizationCookieService);
    }
    
    /// <inheritdoc />
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AdministratorsGroupRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated == false)
        {
            _authorizationCookieService.DeleteCookie();;
            return;
        }

        var userId = context.User.FindFirst(c => c.Type == NameIdentifierKey)!.Value;
        bool isMember = false;

        if (_authorizationCookieService.DoesCookieExist())
        {
            var cookie = _authorizationCookieService.GetCookie();
            isMember = await _authorizationCookieService.DetermineGroupMembership(cookie, GroupId, userId);
            
            if (!isMember)
            {
                return;
            }
            
            context.Succeed(requirement);
            return;
        }

        var directoryObjects = await _graphService.GetAllGroupMemberships(userId);
        List<GroupInformation> groupInformation = new List<GroupInformation>();
        
        foreach (var directoryObject in directoryObjects)
        {
            if (directoryObject.OdataType != "#microsoft.graph.group")
            {
                continue;
            }
            
            Group group = (Group)directoryObject;

            if (GroupId == group.Id)
            {
                isMember = true;
            }
            
            groupInformation.Add(new GroupInformation
            {
                Id = directoryObject.Id,
                DisplayName = group.DisplayName,
                Description = group.Description
            });
        }
        
        if (isMember)
        {
            _authorizationCookieService.AddGroupAuthorizationToCookie(groupAuthorizationModel: new GroupAuthorizationModel
            {
                Groups = groupInformation,
                UserId = userId
            });
            context.Succeed(requirement);
            return;
        }

        if (groupInformation.Count == 0)
        {
            _authorizationCookieService.CreateCookie(userId);
            return;
        }
            
        _authorizationCookieService.AddGroupAuthorizationToCookie(groupAuthorizationModel: new GroupAuthorizationModel
        {
            Groups = groupInformation,
            UserId = userId
        });
    }
}
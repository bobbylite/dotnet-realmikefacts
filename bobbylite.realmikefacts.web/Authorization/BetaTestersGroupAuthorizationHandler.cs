using Ardalis.GuardClauses;
using bobbylite.realmikefacts.web.Constants;
using bobbylite.realmikefacts.web.Models.Authorization;
using bobbylite.realmikefacts.web.Services.Cookie;
using bobbylite.realmikefacts.web.Services.Graph;
using Microsoft.AspNetCore.Authorization;

namespace bobbylite.realmikefacts.web.Authorization;

/// <summary>
/// Authorization handler for beta testers group policy.
/// </summary>
public class BetaTestersGroupAuthorizationHandler : AuthorizationHandler<BetaTestersGroupRequirement>
{
    const string NameIdentifierKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
    const string GroupId = GroupIds.BetaTesters;
    
    private readonly IGraphService _graphService;
    private readonly IAuthorizationCookieService _authorizationCookieService;

    /// <summary>
    /// Initializes an instance of <see cref="BetaTestersGroupAuthorizationHandler"/>
    /// </summary>
    /// <param name="graphService"></param>
    /// <param name="authorizationCookieService"></param>
    public BetaTestersGroupAuthorizationHandler(
        IGraphService graphService, 
        IAuthorizationCookieService authorizationCookieService)
    {
        _graphService = Guard.Against.Null(graphService);
        _authorizationCookieService = Guard.Against.Null(authorizationCookieService);
    }
    
    /// <inheritdoc />
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        BetaTestersGroupRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated == false)
        {
            _authorizationCookieService.DeleteCookie();;
            return;
        }

        var userId = context.User.FindFirst(c => c.Type == NameIdentifierKey)!.Value;
        string cookie = _authorizationCookieService.GetCookie();
        bool isMember = false;

        if (!string.IsNullOrEmpty(cookie))
        {
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
            if (GroupId == directoryObject.Id)
            {
                isMember = true;
            }
            
            groupInformation.Add(new GroupInformation
            {
                GroupId = directoryObject.Id
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
            
        _authorizationCookieService.CreateCookie(userId);
    }
}
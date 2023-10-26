using System.Text;
using System.Text.Json;
using Ardalis.GuardClauses;
using bobbylite.realmikefacts.web.Models.Authorization;
using bobbylite.realmikefacts.web.Services.Graph;
using Microsoft.AspNetCore.Authorization;

namespace bobbylite.realmikefacts.web.Authorization;

/// <summary>
/// Authorization handler for beta testers group policy.
/// </summary>
public class AdministratorsGroupAuthorizationHandler : AuthorizationHandler<AdministratorsGroupRequirement>
{
    private readonly IGraphService _graphService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes an instance of <see cref="AdministratorsGroupAuthorizationHandler"/>
    /// </summary>
    /// <param name="graphService"></param>
    /// <param name="httpContextAccessor"></param>
    public AdministratorsGroupAuthorizationHandler(IGraphService graphService, IHttpContextAccessor httpContextAccessor)
    {
        _graphService = Guard.Against.Null(graphService);
        _httpContextAccessor = Guard.Against.Null(httpContextAccessor);
    }
    
    /// <inheritdoc />
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AdministratorsGroupRequirement requirement)
    {
        const string nameIdentifierKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        const string groupId = "ff9a9b37-2e83-47a9-98ad-eed35d8ca2de";
        const string cookieKey = ".AspNetCore.Custom.Auth.Cookies";
        
        if (context.User.Identity?.IsAuthenticated == false)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete(cookieKey);
            return;
        }

        var userId = context.User.FindFirst(c => c.Type == nameIdentifierKey)!.Value;
        string cookie = _httpContextAccessor.HttpContext?.Request.Cookies[cookieKey]!;
        bool isMember = false;

        if (!string.IsNullOrEmpty(cookie))
        {
            isMember = await DetermineGroupMembership(cookie, groupId, userId);
            
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
            if (groupId == directoryObject.Id)
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
            AddGroupAuthorizationToCookie(groupAuthorizationModel: new GroupAuthorizationModel
            {
                Groups = groupInformation,
                UserId = userId
            });
            context.Succeed(requirement);
            return;
        }
            
        CreateCookie(userId);
    }

    /// <summary>
    /// Determines group membership. 
    /// </summary>
    /// <param name="cookie"></param>
    /// <param name="groupId"></param>
    /// <param name="userId"></param>
    /// <returns>true if user is a member of group. false if use is not a member of group.</returns>
    public async Task<bool> DetermineGroupMembership(string cookie, string groupId, string userId)
    {
        Guard.Against.Null(cookie);
        Guard.Against.Null(groupId);
        Guard.Against.Null(userId);
        
        var groupAuthorizationModel = GetGroupsFromCookie(cookie);
        var isMyCookie = groupAuthorizationModel.UserId == userId;
        bool isMember;

        if (!isMyCookie)
        {
            _httpContextAccessor?.HttpContext?.Response.Cookies.Delete(".AspNetCore.Custom.Auth.Cookies");
            isMember = await _graphService.DoesUserBelongToGroup(userId, groupId);
            if (isMember)
            {
                CreateCookie(userId: userId, groupId: groupId);
                return true;
            }
        }

        if (groupAuthorizationModel.Groups is null)
        {
            return false;
        }

        var matchedGroupIds = groupAuthorizationModel?.Groups?.Where(g => g.GroupId! == groupId);

        isMember = matchedGroupIds?.ToList().Count is not 0;

        return isMember && isMyCookie;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cookie"></param>
    /// <returns></returns>
    public GroupAuthorizationModel GetGroupsFromCookie(string cookie)
    {
        Guard.Against.Null(cookie);
        
        byte[] bytes = Convert.FromBase64String(cookie);
        var serializedJson = Encoding.ASCII.GetString(bytes);
        var groupAuthorizationModel = JsonSerializer.Deserialize<GroupAuthorizationModel>(serializedJson);

        if (groupAuthorizationModel is null)
        {
            throw new NullObjectException();
        }

        return groupAuthorizationModel;
    }
    
    /// <summary>
    /// 
    /// </summary>
    public void CreateCookie(string userId)
    {
        Guard.Against.Null(userId);
        
        var groupAuthorizationModel = new GroupAuthorizationModel
        {
            Groups = null,
            UserId = userId
        };
        AddGroupAuthorizationToCookie(groupAuthorizationModel);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public void CreateCookie(string userId, string groupId)
    {
        Guard.Against.Null(userId);
        Guard.Against.Null(groupId);
        
        var groupAuthorizationModel = new GroupAuthorizationModel
        {
            Groups = new List<GroupInformation>
            {
                new GroupInformation
                {
                    GroupId = groupId
                }
            },
            UserId = userId
        };
        AddGroupAuthorizationToCookie(groupAuthorizationModel);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupAuthorizationModel"></param>
    public void AddGroupAuthorizationToCookie(GroupAuthorizationModel groupAuthorizationModel)
    {
        Guard.Against.Null(groupAuthorizationModel);
        
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.Now.AddMinutes(60)
        };
        
        SerializeAndBase64EncodeCookie(groupAuthorizationModel, cookieOptions);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupAuthorizationModel"></param>
    /// <param name="cookieOptions"></param>
    public void SerializeAndBase64EncodeCookie(GroupAuthorizationModel groupAuthorizationModel, CookieOptions cookieOptions)
    {
        Guard.Against.Null(groupAuthorizationModel);
        Guard.Against.Null(cookieOptions);
        
        var newSerializedJson3 = JsonSerializer.Serialize(groupAuthorizationModel);
        var newBytes3 = Encoding.ASCII.GetBytes(newSerializedJson3);
        var base64SerializedJson3 = Convert.ToBase64String(newBytes3);
        
        _httpContextAccessor.HttpContext?.Response.Cookies.Append(".AspNetCore.Custom.Auth.Cookies", base64SerializedJson3, cookieOptions);
    }
}
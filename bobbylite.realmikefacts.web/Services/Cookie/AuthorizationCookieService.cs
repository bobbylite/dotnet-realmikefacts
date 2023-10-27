using System.Text;
using System.Text.Json;
using Ardalis.GuardClauses;
using bobbylite.realmikefacts.web.Constants;
using bobbylite.realmikefacts.web.Models.Authorization;
using bobbylite.realmikefacts.web.Services.Graph;

namespace bobbylite.realmikefacts.web.Services.Cookie;

/// <summary>
/// Service for handling authorization cookies. 
/// </summary>
public class AuthorizationCookieService : IAuthorizationCookieService
{
    const string CookieKey = CookieKeys.AuthorizationCookieKey;
    
    private readonly IGraphService _graphService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes an instance of <see cref="AuthorizationCookieService"/>
    /// </summary>
    /// <param name="graphService"></param>
    /// <param name="httpContextAccessor"></param>
    public AuthorizationCookieService(IGraphService graphService, IHttpContextAccessor httpContextAccessor)
    {
        _graphService = Guard.Against.Null(graphService);
        _httpContextAccessor = Guard.Against.Null(httpContextAccessor);
    }


    /// <inheritdoc />
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
            _httpContextAccessor?.HttpContext?.Response.Cookies.Delete(CookieKey);
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


    /// <inheritdoc />
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
    
    /// <inheritdoc />
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

    /// <inheritdoc />
    public void DeleteCookie()
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete(CookieKey);
    }

    /// <inheritdoc />
    public string GetCookie()
    {
        var cookie = _httpContextAccessor.HttpContext?.Request.Cookies[CookieKey];

        if (cookie is null)
        {
            throw new NullOrEmptyStringException(nameof(cookie));
        }

        return cookie;
    }

    /// <inheritdoc />
    public bool DoesCookieExist()
    {
        var cookie = _httpContextAccessor.HttpContext?.Request.Cookies[CookieKey];
        var doesCookieExist = !string.IsNullOrEmpty(cookie);

        return doesCookieExist;
    }


    /// <inheritdoc />
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

    /// <inheritdoc />
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

    /// <inheritdoc />
    public void SerializeAndBase64EncodeCookie(GroupAuthorizationModel groupAuthorizationModel, CookieOptions cookieOptions)
    {
        Guard.Against.Null(groupAuthorizationModel);
        Guard.Against.Null(cookieOptions);
        
        var newSerializedJson = JsonSerializer.Serialize(groupAuthorizationModel);
        var newBytes = Encoding.ASCII.GetBytes(newSerializedJson);
        var base64SerializedJson = Convert.ToBase64String(newBytes);
        
        _httpContextAccessor.HttpContext?.Response.Cookies.Append(CookieKey, base64SerializedJson, cookieOptions);
    }
}
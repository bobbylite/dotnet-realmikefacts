using bobbylite.realmikefacts.web.Models.Authorization;

namespace bobbylite.realmikefacts.web.Services.Cookie;

/// <summary>
/// Interface for <see cref="AuthorizationCookieService"/>
/// </summary>
public interface IAuthorizationCookieService
{
    /// <summary>
    /// Determines group membership. 
    /// </summary>
    /// <param name="cookie"></param>
    /// <param name="groupId"></param>
    /// <param name="userId"></param>
    /// <returns><see cref="Task"/> of type <see cref="bool"/> that represents group membership status.</returns>
    public Task<bool> DetermineGroupMembership(string cookie, string groupId, string userId);

    /// <summary>
    /// Deserialize cookie to get groups.
    /// </summary>
    /// <param name="cookie"></param>
    /// <returns><see cref="GroupAuthorizationModel"/></returns>
    public GroupAuthorizationModel GetGroupsFromCookie(string cookie);

    /// <summary>
    /// Creates new cookie with no groups.
    /// </summary>
    /// <param name="userId"></param>
    public void CreateCookie(string userId);
    
    /// <summary>
    /// Deletes authorization cookie with no groups.
    /// </summary>
    public void DeleteCookie();

    /// <summary>
    /// Gets authorization cookie.
    /// </summary>
    /// <returns>string representation fo authorization cookie.</returns>
    public string GetCookie();

    /// <summary>
    /// Determines whether the authorization cookie exists. 
    /// </summary>
    /// <returns></returns>
    public bool DoesCookieExist();

    /// <summary>
    /// Creates new cookie with group id.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="groupId"></param>
    void CreateCookie(string userId, string groupId);

    /// <summary>
    /// Creates new cookie from <see cref="AuthorizationCookieService"/>.
    /// </summary>
    /// <param name="groupAuthorizationModel"></param>
    public void AddGroupAuthorizationToCookie(GroupAuthorizationModel groupAuthorizationModel);

    /// <summary>
    /// Serializes <see cref="GroupAuthorizationModel"/> and adds to cookie as base64 encoded string.
    /// </summary>
    /// <param name="groupAuthorizationModel"></param>
    /// <param name="cookieOptions"></param>
    public void SerializeAndBase64EncodeCookie(GroupAuthorizationModel groupAuthorizationModel,
        CookieOptions cookieOptions);
}
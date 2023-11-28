using Microsoft.Graph.Models;

namespace bobbylite.realmikefacts.web.Services.Graph;

/// <summary>
/// Interface for <see cref="GraphService"/>
/// </summary>
public interface IGraphService
{ 
    /// <summary>
    /// Determins is a user belongs to groups.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="groupId"></param>
    /// <returns></returns>
    public Task<bool> DoesUserBelongToGroup(string userId, string groupId);

    /// <summary>
    /// Gets all memberships for a specified user.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public Task<IEnumerable<DirectoryObject>> GetAllGroupMemberships(string userId);

    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <returns><see cref="Task"/></returns>
    public Task<IEnumerable<User>> GetAllUsers();

    /// <summary>
    /// Adds <see cref="User"/> to group.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="groupId"></param>
    /// <returns></returns>
    public Task AddUserToGroup(string userId, string groupId);
}
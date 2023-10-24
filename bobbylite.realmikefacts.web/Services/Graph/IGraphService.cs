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
}
namespace bobbylite.realmikefacts.web.Services.Graph;

/// <summary>
/// Interface for <see cref="GraphService"/>
/// </summary>
public interface IGraphService
{ 
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public Task<bool> DoesUserExistInAdministratorsGroup(string userId);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public Task<bool> DoesUserExistInBetaTestersGroup(string userId);
}
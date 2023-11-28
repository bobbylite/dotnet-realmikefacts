using bobbylite.realmikefacts.web.Models.AccessRequest;
using bobbylite.realmikefacts.web.Services.Graph;

namespace bobbylite.realmikefacts.web.Services.AccessRequest;

/// <summary>
/// Interface for <see cref="AccessRequestService"/>.
/// </summary>
public interface IAccessRequestService
{ 
    /// <summary>
    /// Gets all access requests
    /// </summary>
    /// <returns><see cref="Task"/></returns>
    public IDictionary<string, AccessRequestModel> GetAllAccessRequests();

    /// <summary>
    /// Creates an access request.
    /// </summary>
    /// <returns><see cref="string"/> that represents the access request id.</returns>
    public Task<string> CreateRequest(AccessRequestModel accessRequestModel);

    /// <summary>
    /// Fulfills an accessRequest
    /// </summary>
    /// <returns></returns>
    public Task FulfillRequest(string accessRequestId, GroupResourceReqeuest group);

    /// <summary>
    /// Provisions group access using the <see cref="GraphService"/>
    /// </summary>
    /// <returns><see cref="Task"/></returns>
    public Task AzureGroupAccessProvisioning();
}
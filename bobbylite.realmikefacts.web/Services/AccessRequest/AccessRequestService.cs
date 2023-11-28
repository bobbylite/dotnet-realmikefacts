using Ardalis.GuardClauses;
using bobbylite.realmikefacts.web.Models.AccessRequest;
using bobbylite.realmikefacts.web.Services.Graph;

namespace bobbylite.realmikefacts.web.Services.AccessRequest;

/// <summary>
/// Service for managing access requests.
/// </summary>
public class AccessRequestService : IAccessRequestService 
{ 
    private readonly ILogger<AccessRequestService> _logger;
    private readonly IGraphService _graphService;


    private readonly Dictionary<string, AccessRequestModel> _accessRequestDictionary;

    /// <summary>
    /// Instantiates a <see cref="AccessRequestService"/>
    /// </summary>
    public AccessRequestService(ILogger<AccessRequestService> logger,
        IGraphService graphService)
    { 
        _logger = Guard.Against.Null(logger);
        _graphService = Guard.Against.Null(graphService);

        _accessRequestDictionary = new Dictionary<string, AccessRequestModel>();
    }

    /// <inheritdoc/>
    public Task AzureGroupAccessProvisioning()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<string> CreateRequest(AccessRequestModel accessRequestModel)
    {
        Guard.Against.Null(accessRequestModel?.Id);

        if (accessRequestModel?.RequestedResource is null)
        {
            throw new NullOrEmptyStringException(nameof(accessRequestModel.RequestedResource));
        }

        if (string.IsNullOrEmpty(accessRequestModel?.Timestamp))
        {
            throw new NullOrEmptyStringException(nameof(accessRequestModel.Timestamp));
        }

        if (string.IsNullOrEmpty(accessRequestModel?.UserId))
        {
            throw new NullOrEmptyStringException(nameof(accessRequestModel.UserId));
        }

        var users = await _graphService.GetAllUsers();

        _accessRequestDictionary.Add(accessRequestModel.Id, accessRequestModel);

        return accessRequestModel.Id;
    }
    
    /// <inheritdoc/>
    public async Task FulfillRequest(string accessRequestId, GroupResourceReqeuest group)
    {
        Guard.Against.NullOrEmpty(accessRequestId);
        Guard.Against.Null(group);

        foreach(var accessRequest in _accessRequestDictionary.Where(r => r.Key == accessRequestId))
        {
            accessRequest.Value.AzureGroupAccessProvisioning = new AzureGroupAccessProvisioning
            {
                GroupName = group.DisplayName
            };

            await _graphService.AddUserToGroup(userId: accessRequest.Value.UserId!, groupId: group.Id!);
            _accessRequestDictionary.Remove(accessRequest.Key);
        }
    }

    /// <inheritdoc/>
    public IDictionary<string, AccessRequestModel> GetAllAccessRequests()
    {
        return _accessRequestDictionary;
    }
}
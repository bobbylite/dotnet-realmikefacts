using Ardalis.GuardClauses;
using Azure.Identity;
using bobbylite.realmikefacts.web.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Graph;

namespace bobbylite.realmikefacts.web.Services.Graph;

/// <summary>
/// Service for Microsoft Graph API.
/// </summary>
public class GraphService : IGraphService
{
    private readonly ILogger<GraphService> _logger;
    private readonly AzureOptions _azureOptions;

    /// <summary>
    /// Initializes an instance of <see cref="GraphService"/>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="azureOptions"></param>
    public GraphService(ILogger<GraphService> logger, IOptions<AzureOptions> azureOptions)
    {
        _logger = Guard.Against.Null(logger);
        _azureOptions = Guard.Against.Null(azureOptions.Value);
    }

    /// <inheritdoc />
    public async Task<bool> DoesUserBelongToGroup(string userId, string groupId)
    {
        Guard.Against.NullOrEmpty(userId);
        Guard.Against.Null(groupId);
        
        var scopes = new[] { "https://graph.microsoft.com/.default" };
        var clientSecretCredential = new ClientSecretCredential(_azureOptions.TenantId, _azureOptions.ClientId,
            _azureOptions.ClientSecret);
        var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

        var directoryObjectCollection = await graphClient.Users[userId].TransitiveMemberOf.GetAsync();
        var directoryObjects = directoryObjectCollection?.Value;

        if (directoryObjects is null)
        {
            throw new NullObjectException();
        }
        
        var group = directoryObjects.Find(obj => obj.Id == groupId);

        return group is not null;
    }
}
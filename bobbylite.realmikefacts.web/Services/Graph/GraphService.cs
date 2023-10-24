using Ardalis.GuardClauses;
using Azure.Identity;
using bobbylite.realmikefacts.web.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;

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

    /// <summary>
    /// Determines whether a user is a member of the Administrators group.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<bool> DoesUserExistInAdministratorsGroup(string userId)
    {
        var scopes = new[] { "https://graph.microsoft.com/.default" };
        var clientSecretCredential = new ClientSecretCredential(_azureOptions.TenantId, _azureOptions.ClientId,
            _azureOptions.ClientSecret);
        var graphClient = new GraphServiceClient(clientSecretCredential, scopes);
        var groups = await GetGroups(graphClient);
        
        foreach (var group in groups.ToList().Where(g => g.Id == "ff9a9b37-2e83-47a9-98ad-eed35d8ca2de"))
        {
            var groupId = group.Id ?? throw new NullOrEmptyStringException();
            return await DetermineMembershipStatus(graphClient, groupId, userId);
        }

        return false;
    }
    
    /// <summary>
    /// Determines whether a user is a member of the BetaTesters group.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<bool> DoesUserExistInBetaTestersGroup(string userId)
    {
        var scopes = new[] { "https://graph.microsoft.com/.default" };
        var clientSecretCredential = new ClientSecretCredential(_azureOptions.TenantId, _azureOptions.ClientId,
            _azureOptions.ClientSecret);
        var graphClient = new GraphServiceClient(clientSecretCredential, scopes);
        var groups = await GetGroups(graphClient);

        foreach (var group in groups.Where(g => g.Id == "14c0cb9c-4c9d-4f25-9184-6fa53fdb296d"))
        {
            var groupId = group.Id ?? throw new NullOrEmptyStringException();
            return await DetermineMembershipStatus(graphClient, groupId, userId);
        }

        return false;
    }
    
    private static async Task<IEnumerable<Group>> GetGroups(GraphServiceClient graphClient)
    {
        var groups = await graphClient.Groups.GetAsync();

        var groupList = groups?.Value?.ToList();

        if (groupList is null)
        {
            throw new NullObjectException();
        }

        return groupList;
    }

    private async Task<bool> DetermineMembershipStatus(GraphServiceClient graphClient, string groupId, string userId)
    {
        var groupMembersResponse = 
            await graphClient.Groups[groupId].Members.GetAsync();
            
        var groupMembers = groupMembersResponse?.Value;
            
        if (groupMembers is null)
        {
            _logger.LogError("Unsuccessful get group members operation.");
            throw new NullObjectException();
        }

        var userHasMembership = groupMembers.Find(m => m.Id == userId);

        return userHasMembership is not null;
    }
}
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
    private readonly AzureOptions _azureOptions;
    
    /// <summary>
    /// Initializes an instance of <see cref="GraphService"/>
    /// </summary>
    /// <param name="azureOptions"></param>
    public GraphService(IOptions<AzureOptions> azureOptions)
    {
        _azureOptions = Guard.Against.Null(azureOptions.Value);
    }
    
    /// <summary>
    /// Gets all users associated with all groups.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task GetGroupMembers()
    {
        var scopes = new[] { "https://graph.microsoft.com/.default" };
        var clientSecretCredential = new ClientSecretCredential(_azureOptions.TenantId, _azureOptions.ClientId,
            _azureOptions.ClientSecret);
        var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

        var groups = await graphClient.Groups.GetAsync();
        var groupList = groups?.Value;

        foreach (var group in groupList!)
        {
            var groupMembers = 
                await graphClient.Groups[group.Id].Members.GetAsync();
        }
    }
}
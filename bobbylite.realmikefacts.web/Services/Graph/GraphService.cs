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
    /// <param name="azureOptions"></param>
    public GraphService(ILogger<GraphService> logger, IOptions<AzureOptions> azureOptions)
    {
        _logger = Guard.Against.Null(logger);
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

        var groups = await graphClient.Groups.GetAsync();
        var groupList = groups?.Value;

        foreach (var group in groupList!)
        {
            if (group.Id != "ff9a9b37-2e83-47a9-98ad-eed35d8ca2de")
            {
                continue;
            }
            
            var groupMembers = 
                await graphClient.Groups[group.Id].Members.GetAsync();
            
            if (groupMembers?.Value is null)
            {
                _logger.LogError("Unsuccessful get group members operation.");
                throw new NotSuccessfulHttpRequestException();
            }

            foreach (var member in groupMembers.Value)
            {
                if (member.Id == userId)
                {
                    return true;
                }
            }

            return false;
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

        var groups = await graphClient.Groups.GetAsync();
        var groupList = groups?.Value;

        foreach (var group in groupList!)
        {
            if (group.Id != "14c0cb9c-4c9d-4f25-9184-6fa53fdb296d")
            {
                continue;
            }
            
            var groupMembers = 
                await graphClient.Groups[group.Id].Members.GetAsync();

            if (groupMembers?.Value is null)
            {
                _logger.LogError("Unsuccessful get group members operation.");
                throw new NotSuccessfulHttpRequestException();
            }

            foreach (var member in groupMembers.Value)
            {
                if (member.Id == userId)
                {
                    return true;
                }
            }

            return false;
        }

        return false;
    }
}
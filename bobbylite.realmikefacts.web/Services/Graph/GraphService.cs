using Ardalis.GuardClauses;
using Azure.Identity;
using bobbylite.realmikefacts.web.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Auth;
using Microsoft.Graph.Models.ODataErrors;

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

    /// <inheritdoc/>
    public async Task AddUserToGroup(string userId, string groupId)
    { 
        Guard.Against.NullOrEmpty(userId);
        Guard.Against.NullOrEmpty(groupId);

        var scopes = new[] { "https://graph.microsoft.com/.default" };
        var clientSecretCredential = new ClientSecretCredential(_azureOptions.TenantId, _azureOptions.ClientId,
            _azureOptions.ClientSecret);
        var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

        var oDataId = $"https://graph.microsoft.com/v1.0/directoryObjects/{userId}";
        var requestBody = new ReferenceCreate
        {
            OdataId = oDataId,
        };

        try 
        {
            await graphClient.Groups[groupId].Members.Ref.PostAsync(requestBody);
        }
        catch (ODataError e)
        {
            if (e.Error?.Message == "One or more added object references already exist for the following modified properties: 'members'.")
            {
                return;
            }
            
            throw new InvalidOperationException(e.Error?.Message);
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<User>> GetAllUsers()
    { 
        var scopes = new[] { "https://graph.microsoft.com/.default" };
        var clientSecretCredential = new ClientSecretCredential(_azureOptions.TenantId, _azureOptions.ClientId,
            _azureOptions.ClientSecret);
        var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

        var directoryObjectCollection = await graphClient.Users.GetAsync();

        if (directoryObjectCollection?.Value is null)
        { 
            throw new NullObjectException(nameof(directoryObjectCollection));
        }

        var userList = new List<User>();
        foreach(var user in directoryObjectCollection.Value.ToList())
        {
            userList.Add(new User
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                GivenName = user.GivenName
            });
        }

        return userList;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<DirectoryObject>> GetAllGroupMemberships(string userId)
    {
        Guard.Against.NullOrEmpty(userId);
        
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
        
        return directoryObjects;
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
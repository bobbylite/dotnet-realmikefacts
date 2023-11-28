namespace bobbylite.realmikefacts.web.Models.AccessRequest;

/// <summary>
/// Model that represents an access request.
/// </summary>
public class AccessRequestModel 
{ 
    /// <summary>
    /// Id of the access request.
    /// </summary>
    public string? Id { get; set;}

    /// <summary>
    /// Resource that access is being requested for.
    /// </summary>
    public GroupResourceReqeuest? RequestedResource { get; set;}

    /// <summary>
    /// Creation timestamp for access request.
    /// </summary>
    public string? Timestamp { get; set; }

    /// <summary>
    /// User id of the entity who owns the access request.
    /// </summary>
    public string? UserId { get; set;}

    /// <summary>
    /// Display name of the entity who owns the access request.
    /// </summary>
    public string? DisplayName { get; set;}

    /// <summary>
    /// Represents a provisioning request.
    /// </summary>
    public AzureGroupAccessProvisioning? AzureGroupAccessProvisioning { get; set;}
}

/// <summary>
/// Model that represents provisioning request
/// </summary>
public class AzureGroupAccessProvisioning
{ 
    /// <summary>
    /// Represents the name of the group to be provisioned to this user.
    /// </summary>
    public string? GroupName { get; set; }

    /// <summary>
    /// Represents the username of the entity requesting access.
    /// </summary>
    public string? Username { get; set; }

}

/// <summary>
/// Represents a group resource access request.
/// </summary>
public class GroupResourceReqeuest
{
    /// <summary>
    /// Name of the group.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Id of the group.
    /// </summary>
    public string? Id { get; set; }
}
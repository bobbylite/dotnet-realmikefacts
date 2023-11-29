using System.Text.Json.Serialization;
using Ardalis.GuardClauses;

namespace bobbylite.realmikefacts.web.Models.Authorization;

/// <summary>
/// Group authorization model intended to be used with cookies.
/// </summary>
public class GroupAuthorizationModel
{
    /// <summary>
    /// Groups.
    /// </summary>
    [JsonPropertyName("groups")]
    public List<GroupInformation>? Groups { get; set; }
    
    /// <summary>
    /// Group name.
    /// </summary>
    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }
}

/// <summary>
/// Group information.
/// </summary>
public class GroupInformation
{
    /// <summary>
    /// Group Id.
    /// </summary>
    [JsonPropertyName("group_id")]
    public string? Id { get; set; }

    /// <summary>
    /// Display name.
    /// </summary>
    [JsonPropertyName("group_display_name")]
    public string? DisplayName { get; set; }

    /// <summary>
    /// Description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
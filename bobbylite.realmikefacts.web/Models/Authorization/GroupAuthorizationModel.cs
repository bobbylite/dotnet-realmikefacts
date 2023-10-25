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
    public List<GroupInformation>? Groups { get; set; }
}

/// <summary>
/// Group information.
/// </summary>
public class GroupInformation
{
    /// <summary>
    /// Group name.
    /// </summary>
    [JsonPropertyName("group_name")]
    public string? GroupName { get; set; }
    
    /// <summary>
    /// Group Id.
    /// </summary>
    [JsonPropertyName("group_id")]
    public string? GroupId { get; set; }
}
using System.Text.Json.Serialization;

namespace bobbylite.realmikefacts.web.Models.Token;

/// <summary>
/// Token endpoint request
/// </summary>
public class TokenEndpointRequest
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("grant_type")]
    public string? GrantType { get; set; }
    
    /// <summary>
    /// The username for the authentication credentials.
    /// </summary>
    //[JsonPropertyName("userName")]
    //public string? Username { get; set; }
    
    /// <summary>
    /// The password for the authentication credentials.
    /// </summary>
    //[JsonPropertyName("password")]
    //public string? Password { get; set; }
}
using System.Text.Json.Serialization;

namespace bobbylite.realmikefacts.web.Models.Token;

/// <summary>
/// Token endpoint request
/// </summary>
public class TokenEndpointRequest
{
    /// <summary>
    /// Flow type for OAuth 2.0 request.
    /// </summary>
    [JsonPropertyName("grant_type")]
    public string? GrantType { get; set; }
}
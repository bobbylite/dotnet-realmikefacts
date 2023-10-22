using System.Text.Json.Serialization;

namespace bobbylite.realmikefacts.web.Models.Token;

/// <summary>
/// Json object representing the token response from ping.
/// </summary>
public class TokenEndpointResponse
{
    /// <summary>
    /// Json property representing the requested access token.
    /// </summary>
    [JsonPropertyName("access_token")]
    public string? AccessToken {get; set;}

    /// <summary>
    /// Json property representing the type of token.
    /// </summary>
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = "Bearer";

    /// <summary>
    /// Json property representing the expiration time of the token.
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; } = 3600;
}
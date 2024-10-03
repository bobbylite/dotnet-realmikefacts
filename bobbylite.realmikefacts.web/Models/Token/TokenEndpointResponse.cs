using System.Text.Json.Serialization;

namespace bobbylite.realmikefacts.web.Models.Token;

/// <summary>
/// Json object representing the token response from ms.
/// </summary>
public class TokenEndpointResponse
{
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

    /// <summary>
    /// Json property representing the requested access token.
    /// </summary>
    [JsonPropertyName("access_token")]
    public string? AccessToken {get; set;}

    /// <summary>
    /// Json property representing the scope of the token.
    /// </summary>
    [JsonPropertyName("scope")]
    public string Scope { get; set; } = string.Empty;

    /// <summary>
    /// Json property representing the refresh token.
    /// </summary>
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; } = string.Empty;
}
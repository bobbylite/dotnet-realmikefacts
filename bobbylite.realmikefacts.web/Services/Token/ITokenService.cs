using bobbylite.realmikefacts.web.Models.Token;

namespace bobbylite.realmikefacts.web.Services.Token;

/// <summary>
/// Interface for <see cref="TokenService"/>
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Sets the token property after making an authentication request.
    /// </summary>
    /// <returns></returns>
    public Task<TokenEndpointResponse> SetAccessToken();

    /// <summary>
    /// Refreshes the access token.
    /// </summary>
    public Task<TokenEndpointResponse> RefreshAccessToken();
    
    /// <summary>
    /// Token.
    /// </summary>
    TokenEndpointResponse Token { get; set; }
}
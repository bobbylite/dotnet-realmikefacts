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
    public Task SetAccessToken();
    
    /// <summary>
    /// Token.
    /// </summary>
    TokenEndpointResponse Token { get; set; }
}
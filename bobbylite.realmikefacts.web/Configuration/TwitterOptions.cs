namespace bobbylite.realmikefacts.web.Configuration;

/// <summary>
/// Options for twitter support.
/// </summary>
public class TwitterOptions
{
    /// <summary>
    /// Section key for PingOne options.
    /// </summary>
    public const string SectionKey = "Twitter";
    
    /// <summary>
    /// Twitter base url.
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// Twitter token.
    /// </summary>
    public string Token { get; set; } = string.Empty;
    
    /// <summary>
    /// Twitter client id.
    /// </summary>
    public string ClientId { get; set; } = string.Empty;
    
    /// <summary>
    /// Twitter client secret.
    /// </summary>
    public string ClientSecret { get; set; } = string.Empty;
}
namespace bobbylite.realmikefacts.web.Configuration;

/// <summary>
/// AzureAd options.
/// </summary>
public class AzureOptions
{
    /// <summary>
    /// Section key for Twitter options.
    /// </summary>
    public const string SectionKey = "AzureAd";
    
    /// <summary>
    /// Instance Id.
    /// </summary>
    public string Instance { get; set; } = string.Empty;
    
    /// <summary>
    /// Azure domain.
    /// </summary>
    public string Domain { get; set; } = string.Empty;
    
    /// <summary>
    /// OAuth2 client credential client id.
    /// </summary>
    public string ClientId { get; set; } = string.Empty;
    
    /// <summary>
    /// Azure tenant Id.
    /// </summary>
    public string TenantId { get; set; } = string.Empty;
    
    /// <summary>
    /// OAuth2 client credential client secret.
    /// </summary>
    public string ClientSecret { get; set; } = string.Empty;
    
    /// <summary>
    /// Azure B2C sign up and sign in policy id.
    /// </summary>
    public string SignUpSignInPolicyId { get; set; } = string.Empty;
    
    /// <summary>
    /// Callback url for OAuth2 flow.
    /// </summary>
    public string CallbackPath { get; set; } = string.Empty;
}
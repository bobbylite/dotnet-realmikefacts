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
    /// 
    /// </summary>
    public string Instance { get; set; } = string.Empty;
    
    /// <summary>
    /// 
    /// </summary>
    public string Domain { get; set; } = string.Empty;
    
    /// <summary>
    /// 
    /// </summary>
    public string ClientId { get; set; } = string.Empty;
    
    /// <summary>
    ///
    /// </summary>
    public string TenantId { get; set; } = string.Empty;
    
    /// <summary>
    /// 
    /// </summary>
    public string ClientSecret { get; set; } = string.Empty;
    
    /// <summary>
    /// 
    /// </summary>
    public string SignUpSignInPolicyId { get; set; } = string.Empty;
    
    /// <summary>
    /// 
    /// </summary>
    public string CallbackPath { get; set; } = string.Empty;
}
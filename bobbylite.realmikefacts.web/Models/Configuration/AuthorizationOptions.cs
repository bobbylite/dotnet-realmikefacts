namespace bobbylite.realmikefacts.web.Models.Configuration;

/// <summary>
/// Authorization options.
/// </summary>
public class AuthorizationOptions
{
    /// <summary>
    /// A claim required by an authorization policy.
    /// </summary>
    public class AuthorizationClaim
    {
        /// <summary>
        /// Gets or sets the claim type.
        /// </summary>
        public string ClaimType { get; set; } = "";

        /// <summary>
        /// Gets or sets allowed values.
        /// </summary>
        public List<string> AllowedValues { get; set; } = new ();
    }

    /// <summary>
    /// Authorization Policy.
    /// </summary>
    public class AuthorizationPolicy
    {
        /// <summary>
        /// Gets or sets the required claims of the policy.
        /// </summary>
        public List<AuthorizationClaim> RequiredClaims { get; set; } = new ();
    }

    /// <summary>
    /// Section key represents an appsettings.json value.
    /// </summary>
    public const string SectionKey = "Authorization";

    /// <summary>
    /// Gets or sets the policies.
    /// </summary>
    public Dictionary<string, AuthorizationPolicy> Policies { get; set; } = new ();
}
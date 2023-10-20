namespace bobbylite.realmikefacts.web.Models.Configuration;

/// <summary>
/// The configuration options for authorization of requested web resources.
/// </summary>
public class AuthorizationOptions
{
    /// <summary>
    /// A claim required by an authorization policy.
    /// </summary>
    public class AuthorizationClaim
    {
        /// <summary>
        /// Gets or sets the claim type required - e.g. "groups".
        /// </summary>
        public string ClaimType { get; set; } = "";

        /// <summary>
        /// Gets or sets values the claim must process one or more of for evaluation to succeed.
        /// </summary>
        public List<string> AllowedValues { get; set; } = new ();
    }

    /// <summary>
    /// A policy definition for enforcing authorization.
    /// </summary>
    public class AuthorizationPolicy
    {
        /// <summary>
        /// Gets or sets the claim(s) required by the authorization policy.
        /// </summary>
        public List<AuthorizationClaim> RequiredClaims { get; set; } = new ();
    }

    /// <summary>
    /// The section key used to define the options bound to this class (e.g. in appsettings.json).
    /// </summary>
    public const string SectionKey = "Authorization";

    /// <summary>
    /// Gets or sets the policies defined for enforcing authorization based on required claims.
    /// </summary>
    public Dictionary<string, AuthorizationPolicy> Policies { get; set; } = new ();
}
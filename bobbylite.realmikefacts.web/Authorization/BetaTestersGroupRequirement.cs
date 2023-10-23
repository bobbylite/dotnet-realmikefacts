using Microsoft.AspNetCore.Authorization;

namespace bobbylite.realmikefacts.web.Authorization;

/// <summary>
/// Requirement for authorizing beta testers group policy.
/// </summary>
public class BetaTestersGroupRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// Initializes an instance of <see cref="BetaTestersGroupRequirement"/>
    /// </summary>
    public BetaTestersGroupRequirement()
    {
    }
}
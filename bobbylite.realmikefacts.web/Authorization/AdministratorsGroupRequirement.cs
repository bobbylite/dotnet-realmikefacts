using Microsoft.AspNetCore.Authorization;

namespace bobbylite.realmikefacts.web.Authorization;

/// <summary>
/// Requirement for authorizing beta testers group policy.
/// /// </summary>
public class AdministratorsGroupRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// Initializes an instance of <see cref="AdministratorsGroupRequirement"/>
    /// </summary>
    public AdministratorsGroupRequirement()
    {
    }
}
using Ardalis.GuardClauses;
using bobbylite.realmikefacts.web.Services.Graph;
using Microsoft.AspNetCore.Authorization;

namespace bobbylite.realmikefacts.web.Authorization;

/// <summary>
/// 
/// </summary>
public class AdministratorsGroupAuthorizationHandler : AuthorizationHandler<AdministratorsGroupRequirement>
{
    private readonly IGraphService _graphService;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="graphService"></param>
    public AdministratorsGroupAuthorizationHandler(IGraphService graphService)
    {
        _graphService = Guard.Against.Null(graphService);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="requirement"></param>
    /// <returns></returns>
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AdministratorsGroupRequirement requirement)
    {
        var nameIdentifierKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        if (context.User.HasClaim(c => c.Type == nameIdentifierKey))
        {
            var userId = context.User.FindFirst(c => c.Type == nameIdentifierKey)!.Value;

            var result = await _graphService.DoesUserExistInAdministratorsGroup(userId);

            if (result)
            {
                context.Succeed(requirement);
            }
        }
    }
}
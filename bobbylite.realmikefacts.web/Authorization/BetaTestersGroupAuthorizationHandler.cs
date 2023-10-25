using System.Text;
using System.Text.Json;
using Ardalis.GuardClauses;
using bobbylite.realmikefacts.web.Models.Authorization;
using bobbylite.realmikefacts.web.Services.Graph;
using Microsoft.AspNetCore.Authorization;

namespace bobbylite.realmikefacts.web.Authorization;

/// <summary>
/// Authorization handler for beta testers group policy.
/// </summary>
public class BetaTestersGroupAuthorizationHandler : AuthorizationHandler<BetaTestersGroupRequirement>
{
    private readonly IGraphService _graphService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes an instance of <see cref="BetaTestersGroupAuthorizationHandler"/>
    /// </summary>
    /// <param name="graphService"></param>
    /// <param name="httpContextAccessor"></param>
    public BetaTestersGroupAuthorizationHandler(IGraphService graphService, IHttpContextAccessor httpContextAccessor)
    {
        _graphService = Guard.Against.Null(graphService);
        _httpContextAccessor = Guard.Against.Null(httpContextAccessor);
    }
    
    /// <inheritdoc />
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        BetaTestersGroupRequirement requirement)
    {
        string checkCookie = _httpContextAccessor.HttpContext?.Request.Cookies[".AspNetCore.Custom.Auth.Cookies"]!;

        if (!string.IsNullOrEmpty(checkCookie))
        {
            byte[] bytes = Convert.FromBase64String(checkCookie);
            var serializedJson = Encoding.ASCII.GetString(bytes);
            var deserializedJson = JsonSerializer.Deserialize<GroupAuthorizationModel>(serializedJson);

            foreach (var group in deserializedJson?.Groups!)
            {
                if (group.GroupId == "14c0cb9c-4c9d-4f25-9184-6fa53fdb296d")
                {
                    context.Succeed(requirement);
                    return;
                }
            }
        }
        
        var nameIdentifierKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        if (context.User.HasClaim(c => c.Type == nameIdentifierKey))
        {
            var userId = context.User.FindFirst(c => c.Type == nameIdentifierKey)!.Value;

            var result = await _graphService.DoesUserBelongToGroup(userId, "14c0cb9c-4c9d-4f25-9184-6fa53fdb296d");

            if (result)
            {
                if (!string.IsNullOrEmpty(checkCookie))
                {
                    // Create a new cookie
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Lax,
                    };
                    
                    byte[] bytes2 = Convert.FromBase64String(checkCookie);
                    var serializedJson2 = Encoding.ASCII.GetString(bytes2);
                    var deserializedJson = JsonSerializer.Deserialize<GroupAuthorizationModel>(serializedJson2);
                    
                    deserializedJson?.Groups?.Add(new GroupInformation
                    {
                        GroupId = "14c0cb9c-4c9d-4f25-9184-6fa53fdb296d"
                    });
                    
                    var serializedJson = JsonSerializer.Serialize(deserializedJson);
                    var bytes = Encoding.ASCII.GetBytes(serializedJson);
                    var base64SerializedJson = Convert.ToBase64String(bytes);

                    _httpContextAccessor.HttpContext?.Response.Cookies.Append(".AspNetCore.Custom.Auth.Cookies", base64SerializedJson, cookieOptions);
                }
                else
                {
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Lax,
                    };
                    
                    var newGroupCookieModel = new GroupAuthorizationModel
                    {
                        Groups = new List<GroupInformation>
                        {
                            new GroupInformation
                            {
                                GroupId = "14c0cb9c-4c9d-4f25-9184-6fa53fdb296d"
                            }
                        }
                    };
                    
                    var newSerializedJson = JsonSerializer.Serialize(newGroupCookieModel);
                    var newBytes = Encoding.ASCII.GetBytes(newSerializedJson);
                    var base64SerializedJson = Convert.ToBase64String(newBytes);

                    _httpContextAccessor.HttpContext?.Response.Cookies.Append(".AspNetCore.Custom.Auth.Cookies", base64SerializedJson, cookieOptions);
                }
                
                context.Succeed(requirement);
            }
        }
    }
}
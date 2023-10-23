using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using AppAuthorizationOptions = bobbylite.realmikefacts.web.Models.Configuration.AuthorizationOptions;

namespace bobbylite.realmikefacts.web.Extensions;

/// <summary>
/// Extension methods for <see cref="WebApplicationBuilder"/>.
/// </summary>
public static class WebApplicationBuilderExtensions
{
    /// <summary>
    /// Binds appsettings and environment variable configurations.
    /// </summary>
    /// <param name="webApplication"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder webApplication)
    {
        Guard.Against.Null(webApplication);

        webApplication.Configuration
            .AddJsonFile("appsettings.json", true, false)
            .AddEnvironmentVariables();
        
        webApplication.Services.AddConfiguration(webApplication.Configuration);

        return webApplication;
    }
    
    /// <summary>
    /// Add Microsoft Identity to <see cref="WebApplicationBuilder"/>
    /// </summary>
    /// <param name="webApplicationBuilder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder webApplicationBuilder)
    {
        Guard.Against.Null(webApplicationBuilder);

        webApplicationBuilder
            .Services
            .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .Services
            .AddMicrosoftIdentityWebAppAuthentication(webApplicationBuilder.Configuration);

        webApplicationBuilder.Services.AddRazorPages()
            .AddMicrosoftIdentityUI();

        return webApplicationBuilder;
    }
    

    /// <summary>
    /// Adds Services to &lt;see cref="WebApplicationBuilder"/&gt;
    /// </summary>
    /// <param name="webApplicationBuilder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder webApplicationBuilder)
    {
        Guard.Against.Null(webApplicationBuilder);

        webApplicationBuilder.Services.AddServices(webApplicationBuilder.Configuration);

        return webApplicationBuilder;
    }

    /// <summary>
    /// Adds HttpClients to <see cref="WebApplicationBuilder"/>
    /// </summary>
    /// <param name="webApplicationBuilder"></param>
    public static WebApplicationBuilder AddHttpClients(this WebApplicationBuilder webApplicationBuilder)
    {
        Guard.Against.Null(webApplicationBuilder);

        webApplicationBuilder.Services.AddHttpClients(webApplicationBuilder.Configuration);

        return webApplicationBuilder;
    }
    
    /// <summary>
    /// Adds authorization to <see cref="WebApplicationBuilder"/>
    /// </summary>
    /// <param name="webApplicationBuilder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddAuthorization(this WebApplicationBuilder webApplicationBuilder)
    {
        Guard.Against.Null(webApplicationBuilder);

        var authorizationOptions = new AppAuthorizationOptions();
        webApplicationBuilder.Configuration.Bind(AppAuthorizationOptions.SectionKey, authorizationOptions);

        webApplicationBuilder.Services.AddAuthorization(options =>
        {
            foreach (var policyName in authorizationOptions.Policies.Keys)
            {
                options.AddPolicy(policyName, policy =>
                {
                    authorizationOptions.Policies[policyName].RequiredClaims.ForEach(claim =>
                    {
                        policy.RequireClaim(claim.ClaimType, claim.AllowedValues);
                    });
                });
            }
        });
        
        return webApplicationBuilder;
    }
}

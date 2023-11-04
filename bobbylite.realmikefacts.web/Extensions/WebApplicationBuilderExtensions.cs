using Ardalis.GuardClauses;
using bobbylite.realmikefacts.web.Authorization;
using bobbylite.realmikefacts.web.Constants;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
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
    /// Adds CORS to web application. Documentation: https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-7.0
    /// </summary>
    /// <param name="webApplicationBuilder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddCors(this WebApplicationBuilder webApplicationBuilder)
    {
        Guard.Against.Null(webApplicationBuilder);
        
        const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
        
        webApplicationBuilder.Services.AddCors(options =>
        {
            options.AddPolicy(name: myAllowSpecificOrigins,
                policy  =>
                {
                    policy.WithOrigins("https://realmikefacts.azurewebsites.net",
                        "https://realmikefacts.b2clogin.com");
                });
        });
        
        return webApplicationBuilder;
    }
    
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

        webApplicationBuilder
            .AddAzureAdAuthorization()
            .AddGroupAuthorization();
        
        return webApplicationBuilder;
    }

    private static WebApplicationBuilder AddAzureAdAuthorization(this WebApplicationBuilder webApplicationBuilder)
    {
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

    private static WebApplicationBuilder AddGroupAuthorization(this WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyNames.AdministratorsGroup, policy => policy.Requirements.Add(new AdministratorsGroupRequirement()));
            options.AddPolicy(PolicyNames.BetaTestersGroup, policy => policy.Requirements.Add(new BetaTestersGroupRequirement()));
        });

        webApplicationBuilder.Services.AddGroupAuthorization();
        
        return webApplicationBuilder;
    }
}

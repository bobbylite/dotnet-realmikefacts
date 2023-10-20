using bobbylite.realmikefacts.web.Constants;
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
    /// Adds MVC capabilities to the specified instance of <see cref="WebApplicationBuilder"/>.
    /// </summary>
    /// <param name="webApplicationBuilder">The web application builder to configure.</param>
    /// <returns>The instance of WebApplicationBuilder so that the result can be used with method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if any arguments are null.</exception>
    public static WebApplicationBuilder AddMvc(this WebApplicationBuilder webApplicationBuilder)
    {
        ArgumentNullException.ThrowIfNull(webApplicationBuilder);

        IMvcBuilder mvcBuilder = webApplicationBuilder.Services
            .AddControllersWithViews();

        if (webApplicationBuilder.Environment.IsDevelopment())
        {
            mvcBuilder.AddRazorRuntimeCompilation();
        }

        return webApplicationBuilder;
    }

    /// <summary>
    /// Add Microsoft Identity support to the specified instance of <see cref="WebApplicationBuilder"/>.
    /// </summary>
    /// <param name="webApplicationBuilder">The web application builder to configure.</param>
    /// <returns>The instance of WebApplicationBuilder so that the result can be used with method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if any arguments are null.</exception>
    public static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder webApplicationBuilder)
    {
        ArgumentNullException.ThrowIfNull(webApplicationBuilder);

        webApplicationBuilder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(webApplicationBuilder.Configuration);

        webApplicationBuilder.Services.AddRazorPages() // Razor Pages are required for the MS Identity UI
            .AddMicrosoftIdentityUI();

        return webApplicationBuilder;
    }

    /// <summary>
    /// Add authorization support to the specified instance of <see cref="WebApplicationBuilder"/>.
    /// </summary>
    /// <param name="webApplicationBuilder">The web application builder to configure.</param>
    /// <returns>The instance of WebApplicationBuilder so that the result can be used with method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if any arguments are null.</exception>
    public static WebApplicationBuilder AddAuthorization(this WebApplicationBuilder webApplicationBuilder)
    {
        ArgumentNullException.ThrowIfNull(webApplicationBuilder);

        var authorizationOptions = new AppAuthorizationOptions();
        webApplicationBuilder.Configuration.Bind(AppAuthorizationOptions.SectionKey, authorizationOptions);

        webApplicationBuilder.Services.AddAuthorization(options =>
        {
            foreach (string policyName in authorizationOptions.Policies.Keys)
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

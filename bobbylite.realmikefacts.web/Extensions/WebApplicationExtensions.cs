namespace bobbylite.realmikefacts.web.Extensions;

/// <summary>
/// Extension methods for <see cref="WebApplication"/>.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Adds middleware to the specified <see cref="WebApplication"/>.
    /// </summary>
    /// <param name="webApplication">The web application to configure.</param>
    /// <returns>The instance of WebApplication so that the result can be used with method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if any arguments are null.</exception>
    public static WebApplication AddMiddleware(this WebApplication webApplication)
    {
        ArgumentNullException.ThrowIfNull(webApplication);

        if (!webApplication.Environment.IsDevelopment())
        {
            webApplication.UseExceptionHandler("/Home/Error");
            webApplication.UseHsts();
        }

        webApplication.UseHttpsRedirection();
        webApplication.UseStaticFiles();

        webApplication.UseRouting();

        webApplication.UseAuthentication();
        webApplication.UseAuthorization();

        return webApplication;
    }

    /// <summary>
    /// Adds HTTP endpoints (both MVC and Razor Pages) to the specified <see cref="WebApplication"/>.
    /// </summary>
    /// <param name="webApplication">The web application to configure.</param>
    /// <returns>The instance of WebApplication so that the result can be used with method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if any arguments are null.</exception>
    public static WebApplication AddEndpoints(this WebApplication webApplication)
    {
        ArgumentNullException.ThrowIfNull(webApplication);

        webApplication.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        webApplication.MapRazorPages();

        return webApplication;
    }
}
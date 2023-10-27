using Ardalis.GuardClauses;
using bobbylite.realmikefacts.web.Authorization;
using bobbylite.realmikefacts.web.Configuration;
using bobbylite.realmikefacts.web.Constants;
using bobbylite.realmikefacts.web.Services.Cookie;
using bobbylite.realmikefacts.web.Services.Graph;
using bobbylite.realmikefacts.web.Services.OpenAI;
using bobbylite.realmikefacts.web.Services.Token;
using bobbylite.realmikefacts.web.Services.Twitter;
using Microsoft.AspNetCore.Authorization;

namespace bobbylite.realmikefacts.web.Extensions;

/// <summary>
/// 
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds group authorization using custom authorization handlers.
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <returns></returns>
    public static IServiceCollection AddGroupAuthorization(this IServiceCollection serviceCollection)
    {
        Guard.Against.Null(serviceCollection);
        
        serviceCollection
            .AddSingleton<IAuthorizationHandler, AdministratorsGroupAuthorizationHandler>()
            .AddSingleton<IAuthorizationHandler, BetaTestersGroupAuthorizationHandler>()
            .AddHttpContextAccessor();
        
        return serviceCollection;
    }
    
    /// <summary>
    /// Configuration for options and appsettings.
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="configuration"></param>
    /// <returns><see cref="ServiceCollection"/></returns>
    public static IServiceCollection AddConfiguration(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        Guard.Against.Null(serviceCollection);
        Guard.Against.Null(configuration);

        serviceCollection.Configure<OpenAiOptions>(configuration.GetSection(OpenAiOptions.SectionKey))
            .Configure<TwitterOptions>(configuration.GetSection(TwitterOptions.SectionKey))
            .Configure<AzureOptions>(configuration.GetSection(AzureOptions.SectionKey));
        
        serviceCollection.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddConsole();
        });

        return serviceCollection;
    }
    
    /// <summary>
    /// Adds <see cref="HttpClient"/> to be initialized from <see cref="IHttpClientFactory"/>.
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddHttpClients(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        Guard.Against.Null(serviceCollection);
        Guard.Against.Null(configuration);

        serviceCollection
            .AddOpenAiHttpClients()
            .AddTwitterHttpClients();

        return serviceCollection;
    }
    
    /// <summary>
    /// Adds services to dependency injection.
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        Guard.Against.Null(serviceCollection);
        Guard.Against.Null(configuration);

        serviceCollection.AddSingleton<IGraphService, GraphService>();
        serviceCollection.AddSingleton<IOpenAiService, OpenAiService>();
        serviceCollection.AddSingleton<ITokenService, TokenService>();
        serviceCollection.AddSingleton<ITwitterService, TwitterService>();
        serviceCollection.AddTransient<IAuthorizationCookieService, AuthorizationCookieService>();

        return serviceCollection;
    }
    
    private static IServiceCollection AddOpenAiHttpClients(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddHttpClient(HttpClientNames.OpenAi);
        
        return serviceCollection;
    }
    
    private static IServiceCollection AddTwitterHttpClients(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddHttpClient(HttpClientNames.TwitterApi)
            .Services
            .AddHttpClient(HttpClientNames.TwitterTokenApi);
        
        return serviceCollection;
    }
}
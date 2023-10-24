using Ardalis.GuardClauses;
using bobbylite.realmikefacts.web.Authorization;
using bobbylite.realmikefacts.web.Configuration;
using bobbylite.realmikefacts.web.Constants;
using bobbylite.realmikefacts.web.Services.Graph;
using bobbylite.realmikefacts.web.Services.OpenAI;
using bobbylite.realmikefacts.web.Services.Token;
using bobbylite.realmikefacts.web.Services.Twitter;
using Microsoft.AspNetCore.Authentication.Cookies;
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
        
        serviceCollection.AddSingleton<IAuthorizationHandler, AdministratorsGroupAuthorizationHandler>();
        serviceCollection.AddSingleton<IAuthorizationHandler, BetaTestersGroupAuthorizationHandler>();
        
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

        serviceCollection.Configure<OpenAiOptions>(configuration.GetSection(OpenAiOptions.SectionKey));
        serviceCollection.Configure<TwitterOptions>(configuration.GetSection(TwitterOptions.SectionKey));
        serviceCollection.Configure<AzureOptions>(configuration.GetSection(AzureOptions.SectionKey));
        
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

        serviceCollection.AddOpenAiHttpClients();
        serviceCollection.AddTwitterHttpClients();

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
using Ardalis.GuardClauses;
using bobbylite.realmikefacts.web.Configuration;
using bobbylite.realmikefacts.web.Constants;
using bobbylite.realmikefacts.web.Services.Token;
using bobbylite.realmikefacts.web.Services.Twitter;

namespace bobbylite.realmikefacts.web.Extensions;

/// <summary>
/// 
/// </summary>
public static class ServiceCollectionExtensions
{
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
        
        serviceCollection.Configure<TwitterOptions>(configuration.GetSection(TwitterOptions.SectionKey));
        
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
        
        serviceCollection.AddSingleton<ITwitterService, TwitterService>();
        serviceCollection.AddSingleton<ITokenService, TokenService>();

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
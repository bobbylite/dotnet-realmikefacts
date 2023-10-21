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
    /// Registers the options types used for configuration by the application with the specified service collection
    /// and binds the options types the given configuration instance.
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="configuration"></param>
    /// <returns><see cref="ServiceCollection"/></returns>
    public static IServiceCollection AddConfiguration(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        Guard.Against.Null(serviceCollection);
        Guard.Against.Null(configuration);
        
        // Add configuration service
        serviceCollection.Configure<TwitterOptions>(configuration.GetSection(TwitterOptions.SectionKey));

        // Add logging service
        serviceCollection.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddConsole();
        });

        return serviceCollection;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="configuration"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    public static IServiceCollection AddHttpClients(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        Guard.Against.Null(serviceCollection);
        Guard.Against.Null(configuration);

        serviceCollection.AddTwitterHttpClients();

        return serviceCollection;
    }
    
    /// <summary>
    /// 
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
using bobbylite.realmikefacts.web.Authorization;
using bobbylite.realmikefacts.web.Configuration;
using bobbylite.realmikefacts.web.Constants;
using bobbylite.realmikefacts.web.Extensions;
using bobbylite.realmikefacts.web.Services.Cookie;
using bobbylite.realmikefacts.web.Services.Graph;
using bobbylite.realmikefacts.web.Services.OpenAI;
using bobbylite.realmikefacts.web.Services.Token;
using bobbylite.realmikefacts.web.Services.Twitter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;

namespace bobbylite.realmikefacts.tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddGroupAuthorization_AddsAuthorizationHandlers()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();

        // Act
        serviceCollection.AddLogging();
        serviceCollection.AddSingleton<IAuthorizationCookieService, AuthorizationCookieService>();
        serviceCollection.AddSingleton<IGraphService, GraphService>();
        serviceCollection.AddGroupAuthorization();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var handlers = serviceProvider.GetServices<IAuthorizationHandler>();
        var handlersArray = handlers.ToArray();

        // Assert
        Assert.NotNull(handlersArray[0]);
        Assert.NotNull(handlersArray[1]);
        Assert.IsType<AdministratorsGroupAuthorizationHandler>(handlersArray[0]);
        Assert.IsType<BetaTestersGroupAuthorizationHandler>(handlersArray[1]);
    }

    [Fact]
    public void AddConfiguration_ConfiguresOptions()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var configurationMock = new Mock<IConfigurationManager>();
        var configuration = configurationMock.Object;

        configurationMock
            .Setup(m => m.GetSection(It.IsAny<string>()))
            .Returns(new Mock<IConfigurationSection>().Object);

        configuration
            .AddJsonFile("appsettings.test.json", true, false);

        // Act
        serviceCollection.AddConfiguration(configuration);

        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var openAiOptions = serviceProvider.GetService<IOptions<OpenAiOptions>>();
        var twitterOptions = serviceProvider.GetService<IOptions<TwitterOptions>>();
        var azureOptions = serviceProvider.GetService<IOptions<AzureOptions>>();
        
        Assert.NotNull(openAiOptions);
        Assert.NotNull(twitterOptions);
        Assert.NotNull(azureOptions);
    }

    [Fact]
    public void AddHttpClients_AddsHttpClients()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var configuration = new Mock<IConfiguration>().Object;

        // Act
        serviceCollection.AddHttpClients(configuration);

        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var openAiHttpClient = serviceProvider.GetService<IHttpClientFactory>()?.CreateClient(HttpClientNames.OpenAi);
        var twitterHttpClient = serviceProvider.GetService<IHttpClientFactory>()?.CreateClient(HttpClientNames.TwitterApi);
        var twitterTokenHttpClient = serviceProvider.GetService<IHttpClientFactory>()?.CreateClient(HttpClientNames.TwitterTokenApi);
        
        Assert.NotNull(openAiHttpClient);
        Assert.NotNull(twitterHttpClient);
        Assert.NotNull(twitterTokenHttpClient);
    }

    [Fact]
    public void AddServices_AddsServices()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var configuration = new Mock<IConfiguration>().Object;

        // Act
        serviceCollection.AddLogging();
        serviceCollection.AddHttpClients(configuration);
        serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        serviceCollection.AddServices(configuration);

        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var graphService = serviceProvider.GetService<IGraphService>();
        var openAiService = serviceProvider.GetService<IOpenAiService>();
        var tokenService = serviceProvider.GetService<ITokenService>();
        var twitterService = serviceProvider.GetService<ITwitterService>();
        var authorizationCookieService = serviceProvider.GetService<IAuthorizationCookieService>();
        
        Assert.NotNull(graphService);
        Assert.NotNull(openAiService);
        Assert.NotNull(tokenService);
        Assert.NotNull(twitterService);
        Assert.NotNull(authorizationCookieService);
    }
}
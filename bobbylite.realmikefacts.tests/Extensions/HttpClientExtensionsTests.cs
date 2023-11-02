using System.Net.Http.Headers;
using System.Text;
using bobbylite.realmikefacts.web.Extensions;
using Microsoft.Net.Http.Headers;

namespace bobbylite.realmikefacts.tests.Extensions;

public class HttpClientExtensionsTests
{
    [Fact]
    public void AddAuthorization_AddsAuthorizationHeader()
    {
        // Arrange
        var httpClient = new HttpClient();
        var apiToken = "deadbeef";

        // Act
        var result = httpClient.AddAuthorization(apiToken);

        // Assert
        Assert.NotNull(result);
        Assert.True(httpClient.DefaultRequestHeaders.Contains("Authorization"));
        var authorizationHeader = httpClient.DefaultRequestHeaders.GetValues("Authorization").First();
        Assert.Equal($"Bearer {apiToken}", authorizationHeader);
        Assert.True(httpClient.DefaultRequestHeaders.Contains(HeaderNames.Connection));
        Assert.True(httpClient.DefaultRequestHeaders.Contains(HeaderNames.Host));
    }

    [Fact]
    public void AddBasicAuthorizationHeaders_AddsBasicAuthorizationHeader()
    {
        // Arrange
        var httpClient = new HttpClient();
        var username = "dead";
        var password = "beef";

        // Act
        var result = httpClient.AddBasicAuthorizationHeaders(username, password);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(httpClient.DefaultRequestHeaders.Authorization);
        Assert.IsType<AuthenticationHeaderValue>(httpClient.DefaultRequestHeaders.Authorization);
        var authHeaderValue = (AuthenticationHeaderValue)httpClient.DefaultRequestHeaders.Authorization;
        Assert.Equal("Basic", authHeaderValue.Scheme);
        Assert.Equal(Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}")), authHeaderValue.Parameter);
        Assert.True(httpClient.DefaultRequestHeaders.Contains(HeaderNames.Connection));
        Assert.True(httpClient.DefaultRequestHeaders.Contains(HeaderNames.Host));
    }
}
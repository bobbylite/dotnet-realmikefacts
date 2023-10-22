using System.Net.Http.Headers;
using System.Text;
using Ardalis.GuardClauses;
using Microsoft.Net.Http.Headers;

namespace bobbylite.realmikefacts.web.Extensions;

/// <summary>
/// Extension methods for <see cref="HttpClient"/>
/// </summary>
public static class HttpClientExtensions
{
    /// <summary>
    /// Adds basic authorization for twitter api.
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="username"></param>
    /// <param name="password"></param>
    public static HttpClient AddBasicAuthorizationHeaders(this HttpClient httpClient, string username, string password)
    {
        Guard.Against.Null(httpClient);
        Guard.Against.Null(username);
        Guard.Against.Null(password);
        
        string base64AuthString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64AuthString);
        httpClient.DefaultRequestHeaders.Add(HeaderNames.Connection, "keep-alive");
        httpClient.DefaultRequestHeaders.Add(HeaderNames.Host, "api.twitter.com");

        return httpClient;
    }
}
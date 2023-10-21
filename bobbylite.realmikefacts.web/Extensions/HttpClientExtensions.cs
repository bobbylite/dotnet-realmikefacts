using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using Microsoft.Net.Http.Headers;

namespace bobbylite.realmikefacts.web.Extensions;

/// <summary>
/// Extension methods for <see cref="HttpClient"/>
/// </summary>
public static class HttpClientExtensions
{
    /// <summary>
    /// Adds authorization to the http client instance. 
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="accessToken"></param>
    public static void AddAuthorization(this HttpClient httpClient, string accessToken)
    {
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, MediaTypeNames.Application.Json);
        httpClient.DefaultRequestHeaders.Add(HeaderNames.Connection, "keep-alive");
    }

    /// <summary>
    /// Adds basic authorization for twitter api.
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="username"></param>
    /// <param name="password"></param>
    public static void AddBasicAuthorization(this HttpClient httpClient, string username, string password)
    {
        string base64AuthString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64AuthString);
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64AuthString);
        httpClient.DefaultRequestHeaders.Add(HeaderNames.Connection, "keep-alive");
        httpClient.DefaultRequestHeaders.Add(HeaderNames.Host, "api.twitter.com");
    }
}
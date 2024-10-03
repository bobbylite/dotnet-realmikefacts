using System.Text.Json.Serialization;

namespace bobbylite.realmikefacts.web.Models.Token;

/// <summary>
/// Model representing a tweet.
/// </summary>
public class TweetModel
{
    /// <summary>
    /// Gets or sets the tweet message.
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
}
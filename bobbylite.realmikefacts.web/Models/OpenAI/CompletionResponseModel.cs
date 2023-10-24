using System.Text.Json.Serialization;

namespace bobbylite.realmikefacts.web.Models.OpenAI;

/// <summary>
/// Model that represents completion request via OpenAI
/// </summary>
public class CompletionResponseModel
{
    /// <summary>
    /// Choices for model.
    /// </summary>
    [JsonPropertyName("choices")]
    public IEnumerable<TextObject>? Choices { get; set; }
}

/// <summary>
/// Text for model.
/// </summary>
public class TextObject
{
    /// <summary>
    /// Text.
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; set; }
}
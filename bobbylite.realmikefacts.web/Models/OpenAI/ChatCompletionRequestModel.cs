using System.Text.Json.Serialization;

namespace bobbylite.realmikefacts.web.Models.OpenAI;

/// <summary>
/// OpenAI chat completion request model.
/// </summary>
public class ChatCompletionRequestModel
{
    /// <summary>
    /// OpenAI model version
    /// </summary>
    [JsonPropertyName("model")]
    public string? Model { get; set; }
    
    /// <summary>
    /// List of messages.
    /// </summary>
    [JsonPropertyName("messages")]
    public IEnumerable<Message>? Messages { get; set; }
    
    /// <summary>
    /// Lower values for temperature result in more consistent outputs,
    /// while higher values generate more diverse and creative results.
    /// Select a temperature value based on the desired trade-off
    /// between coherence and creativity for your specific application.
    /// </summary>
    [JsonPropertyName("temperature")]
    public double? Temperature { get; set; }
    
    /// <summary>
    /// An alternative to sampling with temperature, called nucleus sampling,
    /// where the model considers the results of the tokens with top_p probability mass.
    /// So 0.1 means only the tokens comprising the top 10% probability mass are considered.
    /// </summary>
    [JsonPropertyName("top_p")]
    public float TopP { get; set; }

    /// <summary>
    /// Stream options.
    /// </summary>
    [JsonPropertyName("stream")]
    public bool Stream { get; set; } = false;
    
    /// <summary>
    /// The completions API can provide a limited number of log probabilities
    /// associated with the most likely tokens for each output token.
    /// This feature is controlled by using the logprobs field.
    /// This can be useful in some cases to assess the confidence of the model in its output.
    /// </summary>
    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; }
}

/// <summary>
/// OpenAI chat completion message.
/// </summary>
public class Message
{
    /// <summary>
    /// Role type.
    /// </summary>
    [JsonPropertyName("role")]
    public string? Role { get; set; }
    
    
    /// <summary>
    /// Content for message.
    /// </summary>
    [JsonPropertyName("content")]
    public string? Content { get; set; }
}

/// <summary>
/// 
/// </summary>
public class OpenAiRoles
{
    /// <summary>
    /// Assistant role.
    /// </summary>
    public const string Assistant = "assistant";

    /// <summary>
    /// User role.
    /// </summary>
    public const string User = "user";
}
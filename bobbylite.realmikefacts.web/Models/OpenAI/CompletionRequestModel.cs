using System.Text.Json.Serialization;

namespace bobbylite.realmikefacts.web.Models.OpenAI;

/// <summary>
/// Model that represents a completion request to be used with OpenAI API.
/// </summary>
public class CompletionRequestModel
{
    /// <summary>
    /// The completions API can provide a limited number of log probabilities
    /// associated with the most likely tokens for each output token.
    /// This feature is controlled by using the logprobs field.
    /// This can be useful in some cases to assess the confidence of the model in its output.
    /// </summary>
    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; }
    
    /// <summary>
    /// Prompt text for OpenAI completion API endpoint.
    /// </summary>
    [JsonPropertyName("prompt")]
    public string? Prompt { get; set; }
    
    /// <summary>
    /// Model version to train.
    /// </summary>
    [JsonPropertyName("model")]
    public string? Model { get; set; }
    
    /// <summary>
    /// Lower values for temperature result in more consistent outputs,
    /// while higher values generate more diverse and creative results.
    /// Select a temperature value based on the desired trade-off
    /// between coherence and creativity for your specific application.
    /// </summary>
    [JsonPropertyName("temperature")]
    public double? Temperature { get; set; }
}
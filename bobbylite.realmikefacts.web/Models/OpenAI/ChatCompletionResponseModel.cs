using System.Text.Json.Serialization;

namespace bobbylite.realmikefacts.web.Models.OpenAI;

/// <summary>
/// OpenAI chat completion response model for deserialization.
/// </summary>
public class ChatCompletionResponseModel
{
    /// <summary>
    /// Id.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    
    /// <summary>
    /// Object.
    /// </summary>
    [JsonPropertyName("object")]
    public string? ChatCompletionObject { get; set; }
    
    /// <summary>
    /// Created
    /// </summary>
    [JsonPropertyName("created")]
    public long? Created { get; set; }
    
    /// <summary>
    /// Model.
    /// </summary>
    [JsonPropertyName("model")]
    public string? Model { get; set; }
    
    /// <summary>
    /// Choices.
    /// </summary>
    [JsonPropertyName("choices")]
    public List<Choice>? Choices { get; set; }
    
    /// <summary>
    /// Usage.
    /// </summary>
    [JsonPropertyName("usage")]
    public Usage? Usage { get; set; }
}

/// <summary>
/// OpenAI chat completion choice model.
/// </summary>
public class Choice
{
    /// <summary>
    /// Index.
    /// </summary>
    [JsonPropertyName("index")]
    public int? Index { get; set; }
    
    /// <summary>
    /// Message.
    /// </summary>
    [JsonPropertyName("message")]
    public Message? Message { get; set; }
    
    /// <summary>
    /// FinishReason.
    /// </summary>
    [JsonPropertyName("finish_reason")]
    public string? FinishReason { get; set; }
}

/// <summary>
/// OpenAI chat completion usage property. 
/// </summary>
public class Usage
{
    /// <summary>
    /// Prompt tokens.
    /// </summary>
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; set; }
    
    /// <summary>
    /// Completion tokens.
    /// </summary>
    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; set; }
    
    /// <summary>
    /// Total tokens.
    /// </summary>
    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }
}
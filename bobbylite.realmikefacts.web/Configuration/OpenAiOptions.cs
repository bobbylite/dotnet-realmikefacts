namespace bobbylite.realmikefacts.web.Configuration;

/// <summary>
/// Options for OpenAI configuration and support.
/// </summary>
public class OpenAiOptions
{
    /// <summary>
    /// Section key for OpenAI options.
    /// </summary>
    public const string SectionKey = "OpenAI";
    
    /// <summary>
    /// OpenAI base url.
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// Access token for OpenAI API.
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;
    
    /// <summary>
    /// The completions API can provide a limited number of log probabilities
    /// associated with the most likely tokens for each output token.
    /// This feature is controlled by using the logprobs field.
    /// This can be useful in some cases to assess the confidence of the model in its output.
    /// </summary>
    public string MaxTokens { get; set; } = string.Empty;

    /// <summary>
    /// Lower values for temperature result in more consistent outputs,
    /// while higher values generate more diverse and creative results.
    /// Select a temperature value based on the desired trade-off
    /// between coherence and creativity for your specific application.
    /// </summary>
    public string Temperature { get; set; } = string.Empty;

    /// <summary>
    /// Model version to train.
    /// </summary>
    public string Model { get; set; } = string.Empty;

}
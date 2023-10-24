using bobbylite.realmikefacts.web.Models.OpenAI;

namespace bobbylite.realmikefacts.web.Services.OpenAI;

/// <summary>
/// Interface for <see cref="OpenAiService"/>
/// </summary>
public interface IOpenAiService
{
    /// <summary>
    /// Utilizes the GPT 3.5 turbo model to respond to text prompts.
    /// </summary>
    /// <returns></returns>
    public Task<ChatCompletionResponseModel> CreateCompletions(string promptText);
}
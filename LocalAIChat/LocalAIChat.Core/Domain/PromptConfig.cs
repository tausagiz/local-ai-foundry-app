namespace LocalAIChat.Core.Domain;

public class PromptConfig
{
    public string SystemPrompt { get; set; } = string.Empty;

    public string ModelAlias { get; set; } = string.Empty;
}
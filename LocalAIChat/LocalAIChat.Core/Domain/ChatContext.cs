namespace LocalAIChat.Core.Domain;

public class ChatContext
{
    public List<ChatMessage> Messages { get; set; } = [];

    public PromptConfig Profile { get; set; } = new();

    public List<string> Memory { get; set; } = [];
}
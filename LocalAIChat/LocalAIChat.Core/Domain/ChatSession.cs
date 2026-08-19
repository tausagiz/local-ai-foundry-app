namespace LocalAIChat.Core.Domain;

public class ChatSession
{
    public Guid Id { get; set; }

    public string ProfileName { get; set; } = string.Empty;

    public List<ChatMessage> Messages { get; set; } = [];
}
namespace LocalAIChat.Core.Domain;

public class ChatMessage
{
    public ChatMessage()
    {
    }

    public ChatMessage(string role, string content)
    {
        Role = role;
        Content = content;
        Timestamp = DateTime.UtcNow;
    }

    public string Role { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; }
}
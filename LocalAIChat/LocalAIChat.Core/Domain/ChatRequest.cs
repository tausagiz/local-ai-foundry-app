namespace LocalAIChat.Core.Domain;

public class ChatRequest
{
    public string Text { get; set; } = string.Empty;

    public ChatMode? ForceMode { get; set; }

    public string ProfileName { get; set; } = string.Empty;
}
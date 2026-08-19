namespace LocalAIChat.Core.Domain;

public class ChatResult
{
    public string Response { get; set; } = string.Empty;

    public ChatStats Stats { get; set; } = new();
}
namespace LocalAIChat.Core.Domain;

public class ChatStats
{
    public string ModelAlias { get; set; } = string.Empty;

    public ChatMode Mode { get; set; }

    public int InputTokens { get; set; }

    public int OutputTokens { get; set; }

    public long DurationMs { get; set; }

    public bool UsedOnline { get; set; }
}
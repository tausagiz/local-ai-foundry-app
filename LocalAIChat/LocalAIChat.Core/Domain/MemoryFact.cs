namespace LocalAIChat.Core.Domain;

public class MemoryFact
{
    public MemoryFact(string value)
    {
        Value = value;
        CreatedAt = DateTime.UtcNow;
    }

    public string Value { get; set; }

    public DateTime CreatedAt { get; set; }
}
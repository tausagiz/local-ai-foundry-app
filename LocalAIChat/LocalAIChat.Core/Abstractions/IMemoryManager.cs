namespace LocalAIChat.Core.Abstractions;

public interface IMemoryManager
{
    Task AddFactAsync(Guid sessionId, string fact);

    Task<IReadOnlyList<string>> GetFactsAsync();
}
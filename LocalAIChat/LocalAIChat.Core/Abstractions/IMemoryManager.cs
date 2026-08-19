namespace LocalAIChat.Core.Abstractions;

public interface IMemoryManager
{
    Task AddFactAsync(string fact);

    Task<IReadOnlyList<string>> GetFactsAsync();
}
using LocalAIChat.Core.Abstractions;
using LocalAIChat.Core.Domain;

namespace LocalAIChat.Core.Memory;

public class MemoryManager : IMemoryManager
{
    private readonly Dictionary<Guid, List<MemoryFact>> _factsBySession = [];

    public Task AddFactAsync(Guid sessionId, string fact)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fact);

        if (!_factsBySession.TryGetValue(sessionId, out var facts))
        {
            facts = [];
            _factsBySession[sessionId] = facts;
        }

        facts.Add(new MemoryFact(fact));
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<string>> GetFactsAsync()
    {
        var facts = _factsBySession.Values
            .SelectMany(sessionFacts => sessionFacts)
            .Select(memoryFact => memoryFact.Value)
            .ToList();

        return Task.FromResult<IReadOnlyList<string>>(facts);
    }

    public Task<IReadOnlyList<string>> GetFactsAsync(Guid sessionId)
    {
        var facts = _factsBySession.TryGetValue(sessionId, out var sessionFacts)
            ? sessionFacts.Select(memoryFact => memoryFact.Value).ToList()
            : [];

        return Task.FromResult<IReadOnlyList<string>>(facts);
    }
}
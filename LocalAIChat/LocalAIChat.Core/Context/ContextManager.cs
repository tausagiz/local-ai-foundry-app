using LocalAIChat.Core.Abstractions;
using LocalAIChat.Core.Domain;

namespace LocalAIChat.Core.Context;

public class ContextManager : IContextManager
{
    private readonly int _maxMessages;

    public ContextManager(int maxMessages = 20)
    {
        if (maxMessages < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxMessages));
        }

        _maxMessages = maxMessages;
    }

    public Task<ChatContext> BuildContextAsync(ChatSession session)
    {
        ArgumentNullException.ThrowIfNull(session);

        var messages = session.Messages
            .TakeLast(_maxMessages)
            .ToList();

        return Task.FromResult(new ChatContext
        {
            Messages = messages
        });
    }

    public Task ApplySummariesIfNeededAsync(ChatSession session)
    {
        ArgumentNullException.ThrowIfNull(session);

        return Task.CompletedTask;
    }
}
using LocalAIChat.Core.Domain;

namespace LocalAIChat.Core.Abstractions;

public interface IContextManager
{
    Task<ChatContext> BuildContextAsync(ChatSession session);

    Task ApplySummariesIfNeededAsync(ChatSession session);
}
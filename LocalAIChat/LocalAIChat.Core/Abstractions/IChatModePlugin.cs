using LocalAIChat.Core.Domain;

namespace LocalAIChat.Core.Abstractions;

public interface IChatModePlugin
{
    string Name { get; }

    Task<ChatResult> ExecuteAsync(ChatRequest request, ChatContext context);
}
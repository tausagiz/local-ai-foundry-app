using LocalAIChat.Core.Domain;

namespace LocalAIChat.Core.Abstractions;

public interface IChatEngine
{
    Task<ChatResult> SendMessageAsync(ChatRequest request, ChatSession session);
}
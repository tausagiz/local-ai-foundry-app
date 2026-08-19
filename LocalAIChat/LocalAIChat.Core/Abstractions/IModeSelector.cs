using LocalAIChat.Core.Domain;

namespace LocalAIChat.Core.Abstractions;

public interface IModeSelector
{
    ChatMode SelectMode(ChatRequest request, ChatSession session);
}
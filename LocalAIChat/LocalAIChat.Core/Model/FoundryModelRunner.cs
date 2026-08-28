using LocalAIChat.Core.Abstractions;
using LocalAIChat.Core.Domain;

namespace LocalAIChat.Core.Model;

public class FoundryModelRunner : IFoundryModelRunner
{
    public Task<ChatResult> RunAsync(string alias, ChatContext context, ChatRequest request)
    {
        return Task.FromResult(new ChatResult
        {
            Response = "[stub] Odpowiedź modelu",
            Stats = new ChatStats
            {
                ModelAlias = alias,
                Mode = alias == "chat-fast" ? ChatMode.Fast : ChatMode.Main,
                OutputTokens = 3
            }
        });
    }
}
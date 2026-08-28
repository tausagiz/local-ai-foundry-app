using LocalAIChat.Core.Abstractions;
using LocalAIChat.Core.Domain;

namespace LocalAIChat.Core.Model;

public class FoundryModelRunner : IFoundryModelRunner
{
    public Task<ChatResult> RunAsync(string alias, ChatContext context, ChatRequest request)
    {
        // TODO: Resolve the local model by alias and invoke it through Foundry Local/WinML.
        return Task.FromResult(new ChatResult
        {
            Response = "[stub] Odpowiedź modelu",
            Stats = new ChatStats
            {
                ModelAlias = alias,
                Mode = alias switch
                {
                    "chat-fast" => ChatMode.Fast,
                    "chat-smart" => ChatMode.Smart,
                    _ => ChatMode.Main
                },
                OutputTokens = 3
            }
        });
    }
}
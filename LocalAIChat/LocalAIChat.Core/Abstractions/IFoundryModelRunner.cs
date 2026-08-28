using LocalAIChat.Core.Domain;

namespace LocalAIChat.Core.Abstractions;

public interface IFoundryModelRunner
{
    // TODO: Replace the runner implementation with a Foundry Local WinML call.
    Task<ChatResult> RunAsync(string alias, ChatContext context, ChatRequest request);
}
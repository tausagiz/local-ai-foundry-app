using LocalAIChat.Core.Domain;

namespace LocalAIChat.Core.Abstractions;

public interface IFoundryModelRunner
{
    Task<ChatResult> RunAsync(string alias, ChatContext context, ChatRequest request);
}
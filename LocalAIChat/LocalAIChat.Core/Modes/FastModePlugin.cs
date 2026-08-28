using LocalAIChat.Core.Abstractions;
using LocalAIChat.Core.Domain;

namespace LocalAIChat.Core.Modes;

public class FastModePlugin : IChatModePlugin
{
    private readonly IFoundryModelRunner _modelRunner;

    public FastModePlugin(IFoundryModelRunner modelRunner)
    {
        _modelRunner = modelRunner;
    }

    public string Name => ChatMode.Fast.ToString();

    public Task<ChatResult> ExecuteAsync(ChatRequest request, ChatContext context)
    {
        return _modelRunner.RunAsync("chat-fast", context, request);
    }
}
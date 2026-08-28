using LocalAIChat.Core.Abstractions;
using LocalAIChat.Core.Domain;

namespace LocalAIChat.Core.Modes;

public class SmartModePlugin : IChatModePlugin
{
    private readonly IFoundryModelRunner _modelRunner;

    public SmartModePlugin(IFoundryModelRunner modelRunner)
    {
        _modelRunner = modelRunner;
    }

    public string Name => ChatMode.Smart.ToString();

    public Task<ChatResult> ExecuteAsync(ChatRequest request, ChatContext context)
    {
        // TODO: Let Foundry Local choose the best model or tool for the request.
        return _modelRunner.RunAsync("chat-smart", context, request);
    }
}
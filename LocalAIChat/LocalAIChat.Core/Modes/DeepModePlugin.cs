using LocalAIChat.Core.Abstractions;
using LocalAIChat.Core.Domain;

namespace LocalAIChat.Core.Modes;

public class DeepModePlugin : IChatModePlugin
{
    private readonly IFoundryModelRunner _modelRunner;

    public DeepModePlugin(IFoundryModelRunner modelRunner)
    {
        _modelRunner = modelRunner;
    }

    public string Name => ChatMode.DeepReasoning.ToString();

    public Task<ChatResult> ExecuteAsync(ChatRequest request, ChatContext context)
    {
        return _modelRunner.RunAsync("chat-deep", context, request);
    }
}
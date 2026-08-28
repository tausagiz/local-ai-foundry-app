using LocalAIChat.Core.Abstractions;
using LocalAIChat.Core.Domain;

namespace LocalAIChat.Core.Modes;

public class MainModePlugin : IChatModePlugin
{
    private readonly IFoundryModelRunner _modelRunner;

    public MainModePlugin(IFoundryModelRunner modelRunner)
    {
        _modelRunner = modelRunner;
    }

    public string Name => ChatMode.Main.ToString();

    public Task<ChatResult> ExecuteAsync(ChatRequest request, ChatContext context)
    {
        return _modelRunner.RunAsync("chat-main", context, request);
    }
}
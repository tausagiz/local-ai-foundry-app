using LocalAIChat.Core.Abstractions;
using LocalAIChat.Core.Domain;

namespace LocalAIChat.Core.Modes;

public class SearchModePlugin : IChatModePlugin
{
    private readonly IFoundryModelRunner _modelRunner;

    public SearchModePlugin(IFoundryModelRunner modelRunner)
    {
        _modelRunner = modelRunner;
    }

    public string Name => ChatMode.SearchOnline.ToString();

    public Task<ChatResult> ExecuteAsync(ChatRequest request, ChatContext context)
    {
        return _modelRunner.RunAsync("chat-search", context, request);
    }
}
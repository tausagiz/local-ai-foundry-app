using LocalAIChat.Core.Abstractions;
using LocalAIChat.Core.Domain;

namespace LocalAIChat.Core.Engine;

public class ChatEngine : IChatEngine
{
    private readonly IModeSelector _modeSelector;
    private readonly IEnumerable<IChatModePlugin> _plugins;
    private readonly IContextManager _contextManager;
    private readonly IMemoryManager _memoryManager;
    private readonly IStatsCollector _statsCollector;

    public ChatEngine(
        IModeSelector modeSelector,
        IEnumerable<IChatModePlugin> plugins,
        IContextManager contextManager,
        IMemoryManager memoryManager,
        IStatsCollector statsCollector)
    {
        _modeSelector = modeSelector;
        _plugins = plugins;
        _contextManager = contextManager;
        _memoryManager = memoryManager;
        _statsCollector = statsCollector;
    }

    public async Task<ChatResult> SendMessageAsync(ChatRequest request, ChatSession session)
    {
        var context = await _contextManager.BuildContextAsync(session);
        var mode = _modeSelector.SelectMode(request, session);
        var plugin = _plugins.First(p => p.Name == mode.ToString());
        var result = await plugin.ExecuteAsync(request, context);

        await _contextManager.ApplySummariesIfNeededAsync(session);
        await _memoryManager.AddFactAsync(session.Id, ExtractFacts(result));
        _statsCollector.Record(result.Stats);
        session.Messages.Add(new ChatMessage("assistant", result.Response));

        return result;
    }

    private static string ExtractFacts(ChatResult result)
    {
        return result.Response;
    }
}
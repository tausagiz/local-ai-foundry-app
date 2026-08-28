using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using LocalAIChat.Core.Domain;

namespace LocalAIChat.UI.ViewModels;

public sealed class ContextInspectorViewModel : ViewModelBase
{
    private readonly Dictionary<Guid, List<string>> contextHistoryBySession = [];
    private readonly Dictionary<Guid, List<string>> usedPluginsBySession = [];

    public ObservableCollection<ChatMessage> Messages { get; } = [];

    public ObservableCollection<string> UsedPlugins { get; } = [];

    public ObservableCollection<string> ContextHistory { get; } = [];

    public string SystemPrompt { get; private set; } = "-";

    public void Refresh(ChatSession? session, string systemPrompt)
    {
        Messages.Clear();
        SystemPrompt = string.IsNullOrWhiteSpace(systemPrompt) ? "-" : systemPrompt;

        if (session is null)
        {
            UsedPlugins.Clear();
            ContextHistory.Clear();
            OnPropertyChanged(nameof(SystemPrompt));
            return;
        }

        UsedPlugins.Clear();
        ContextHistory.Clear();
        if (usedPluginsBySession.TryGetValue(session.Id, out var usedPlugins))
        {
            foreach (var plugin in usedPlugins)
            {
                UsedPlugins.Add(plugin);
            }
        }

        if (contextHistoryBySession.TryGetValue(session.Id, out var contextHistory))
        {
            foreach (var entry in contextHistory)
            {
                ContextHistory.Add(entry);
            }
        }

        foreach (var message in session.Messages)
        {
            Messages.Add(message);
        }

        OnPropertyChanged(nameof(SystemPrompt));
    }

    public void RecordContext(ChatSession session, string pluginName)
    {
        var usedPlugins = usedPluginsBySession.GetValueOrDefault(session.Id) ?? [];
        usedPluginsBySession[session.Id] = usedPlugins;
        if (!usedPlugins.Contains(pluginName))
        {
            usedPlugins.Add(pluginName);
            UsedPlugins.Add(pluginName);
        }

        var entry = $"{DateTime.Now:HH:mm:ss}  {session.Messages.Count} wiadomości  |  {pluginName}";
        var contextHistory = contextHistoryBySession.GetValueOrDefault(session.Id) ?? [];
        contextHistoryBySession[session.Id] = contextHistory;
        contextHistory.Insert(0, entry);
        ContextHistory.Insert(0, entry);
    }
}
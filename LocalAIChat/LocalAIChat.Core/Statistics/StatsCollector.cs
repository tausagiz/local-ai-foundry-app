using LocalAIChat.Core.Abstractions;
using LocalAIChat.Core.Domain;

namespace LocalAIChat.Core.Statistics;

public class StatsCollector : IStatsCollector
{
    private readonly List<ChatStats> _stats = [];

    public void Record(ChatStats stats)
    {
        _stats.Add(stats);
    }
}
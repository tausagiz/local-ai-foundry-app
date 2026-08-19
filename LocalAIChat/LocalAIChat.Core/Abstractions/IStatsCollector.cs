using LocalAIChat.Core.Domain;

namespace LocalAIChat.Core.Abstractions;

public interface IStatsCollector
{
    void Record(ChatStats stats);
}
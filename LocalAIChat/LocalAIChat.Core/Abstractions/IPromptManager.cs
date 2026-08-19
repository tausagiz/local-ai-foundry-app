using LocalAIChat.Core.Domain;

namespace LocalAIChat.Core.Abstractions;

public interface IPromptManager
{
    PromptConfig GetProfile(string name);

    void SaveProfile(string name, PromptConfig profile);
}
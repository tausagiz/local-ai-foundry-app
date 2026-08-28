using LocalAIChat.Core.Abstractions;
using LocalAIChat.Core.Domain;

namespace LocalAIChat.Core.Prompts;

public class PromptManager : IPromptManager
{
    private readonly Dictionary<string, PromptConfig> _profiles = new(StringComparer.OrdinalIgnoreCase)
    {
        ["PL_Analytical"] = new PromptConfig
        {
            SystemPrompt = "Odpowiadaj po polsku, rzeczowo i analitycznie.",
            ModelAlias = "chat-main"
        }
    };

    public PromptConfig GetProfile(string name)
    {
        return _profiles.TryGetValue(name, out var profile)
            ? profile
            : new PromptConfig();
    }

    public void SaveProfile(string name, PromptConfig profile)
    {
        _profiles[name] = profile;
    }
}
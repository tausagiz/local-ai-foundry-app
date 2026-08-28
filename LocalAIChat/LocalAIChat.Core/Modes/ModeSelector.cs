using LocalAIChat.Core.Abstractions;
using LocalAIChat.Core.Domain;

namespace LocalAIChat.Core.Modes;

public class ModeSelector : IModeSelector
{
    private const int ShortPromptMaxLength = 200;

    public ChatMode SelectMode(ChatRequest request, ChatSession session)
    {
        if (request.ForceMode is not null)
        {
            return request.ForceMode.Value;
        }

        return request.Text.Length <= ShortPromptMaxLength
            ? ChatMode.Fast
            : ChatMode.Main;
    }
}
using LocalAIChat.Core.Abstractions;
using LocalAIChat.Core.Domain;

namespace LocalAIChat.Core.Modes;

public class ModeSelector : IModeSelector
{
    private const int ShortPromptMaxLength = 200;

    private static readonly string[] FactQuestionPrefixes =
    [
        "co ",
        "kto ",
        "gdzie ",
        "kiedy ",
        "ile ",
        "jaki ",
        "jaka ",
        "jakie ",
        "czy "
    ];

    public ChatMode SelectMode(ChatRequest request, ChatSession session)
    {
        if (request.Mode is not null)
        {
            return request.Mode.Value;
        }

        return IsFactQuery(request)
            || IsLongAnalytical(request)
            ? ChatMode.Smart
            : request.Text.Length <= ShortPromptMaxLength
            ? ChatMode.Fast
            : ChatMode.Main;
    }

    public bool IsFactQuery(ChatRequest request)
    {
        var text = request.Text.TrimStart();
        return FactQuestionPrefixes.Any(prefix => text.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
    }

    public bool IsLongAnalytical(ChatRequest request)
    {
        return request.Text.Length > ShortPromptMaxLength;
    }
}
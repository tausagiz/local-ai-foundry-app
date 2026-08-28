using LocalAIChat.Core.Domain;
using LocalAIChat.Core.Model;

namespace LocalAIChat.Tests;

public class FoundryModelRunnerTests
{
    [Theory]
    [InlineData(ChatMode.Fast, "Hello", "phi-3.5-mini")]
    [InlineData(ChatMode.Main, "Hello", "phi-4")]
    [InlineData(ChatMode.DeepReasoning, "Hello", "phi-4-reasoning")]
    [InlineData(ChatMode.Smart, "krótka odpowiedź", "phi-3.5-mini")]
    public void ResolveModelAlias_MapsModeToRealFoundryModel(ChatMode mode, string promptText, string expected)
    {
        var actual = FoundryModelRunner.ResolveModelAlias(mode, promptText);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ResolveModelAlias_UsesPhi4_ForLongSmartPrompts()
    {
        var prompt = "Analizuję ten problem bardzo szczegółowo: " + new string('x', 250);

        var actual = FoundryModelRunner.ResolveModelAlias(ChatMode.Smart, prompt);

        Assert.Equal("phi-4", actual);
    }

    [Theory]
    [InlineData("chat-fast", "phi-3.5-mini")]
    [InlineData("chat-main", "phi-4")]
    [InlineData("chat-smart", "phi-3.5-mini")]
    [InlineData("phi-4-reasoning", "phi-4-reasoning")]
    public void ResolveModelAlias_MapsLegacyAliasToActualFoundryModel(string alias, string expected)
    {
        var actual = FoundryModelRunner.ResolveModelAlias(alias, "krótka odpowiedź");

        Assert.Equal(expected, actual);
    }
}

using System.Diagnostics;
using System.Text.Json;
using LocalAIChat.Core.Abstractions;
using LocalAIChat.Core.Domain;

namespace LocalAIChat.Core.Model;

public class FoundryModelRunner : IFoundryModelRunner
{
    private const string DefaultModel = "phi-3.5-mini";

    public async Task<ChatResult> RunAsync(string alias, ChatContext context, ChatRequest request)
    {
        var resolvedAlias = ResolveModelAlias(alias, request.Text);
        var start = DateTime.UtcNow;
        var prompt = BuildPrompt(context, request);

        try
        {
            var responseText = await InvokeFoundryAsync(resolvedAlias, prompt);
            var durationMs = (long)(DateTime.UtcNow - start).TotalMilliseconds;

            return new ChatResult
            {
                Response = responseText.Trim(),
                Stats = new ChatStats
                {
                    ModelAlias = resolvedAlias,
                    Mode = ResolveMode(alias),
                    InputTokens = EstimateTokens(prompt),
                    OutputTokens = EstimateTokens(responseText),
                    DurationMs = durationMs
                }
            };
        }
        catch (Exception ex)
        {
            var durationMs = (long)(DateTime.UtcNow - start).TotalMilliseconds;
            return new ChatResult
            {
                Response = $"[FoundryLocal] Nie udało się uruchomić modelu {resolvedAlias}: {ex.Message}",
                Stats = new ChatStats
                {
                    ModelAlias = resolvedAlias,
                    Mode = ResolveMode(alias),
                    InputTokens = EstimateTokens(prompt),
                    DurationMs = durationMs
                }
            };
        }
    }

    public static string ResolveModelAlias(ChatMode mode, string? promptText = null)
    {
        return mode switch
        {
            ChatMode.Fast => "phi-3.5-mini",
            ChatMode.Main => "phi-4",
            ChatMode.DeepReasoning => "phi-4-reasoning",
            ChatMode.Smart => string.IsNullOrWhiteSpace(promptText) || promptText.Length <= 200 ? "phi-3.5-mini" : "phi-4",
            _ => DefaultModel
        };
    }

    public static string ResolveModelAlias(string alias, string? promptText = null)
    {
        if (string.IsNullOrWhiteSpace(alias))
        {
            return DefaultModel;
        }

        var normalized = alias.Trim();

        return normalized.ToLowerInvariant() switch
        {
            "chat-fast" => "phi-3.5-mini",
            "chat-main" => "phi-4",
            "chat-deep" => "phi-4-reasoning",
            "chat-search" => "phi-4",
            "chat-smart" => string.IsNullOrWhiteSpace(promptText) || promptText.Length <= 200 ? "phi-3.5-mini" : "phi-4",
            "phi-3.5-mini" or "phi-3.5-mini-instruct" => "phi-3.5-mini",
            "phi-4" or "phi-4-instruct" => "phi-4",
            "phi-4-reasoning" or "phi-4-reasoning-instruct" or "deepreasoning" => "phi-4-reasoning",
            _ => normalized
        };
    }

    public static ChatMode ResolveMode(string alias)
    {
        return alias?.Trim().ToLowerInvariant() switch
        {
            "chat-fast" => ChatMode.Fast,
            "chat-smart" => ChatMode.Smart,
            "chat-main" => ChatMode.Main,
            "chat-deep" => ChatMode.DeepReasoning,
            "chat-search" => ChatMode.SearchOnline,
            "phi-4-reasoning" or "phi-4-reasoning-instruct" or "deepreasoning" => ChatMode.DeepReasoning,
            _ => ChatMode.Main
        };
    }

    private static string BuildPrompt(ChatContext context, ChatRequest request)
    {
        var lines = new List<string>();

        if (!string.IsNullOrWhiteSpace(context.Profile.SystemPrompt))
        {
            lines.Add($"System: {context.Profile.SystemPrompt}");
        }

        foreach (var message in context.Messages)
        {
            if (!string.IsNullOrWhiteSpace(message.Content))
            {
                lines.Add($"{message.Role}: {message.Content}");
            }
        }

        if (!string.IsNullOrWhiteSpace(request.Text))
        {
            lines.Add($"User: {request.Text}");
        }

        return string.Join(Environment.NewLine, lines);
    }

    private static async Task<string> InvokeFoundryAsync(string modelAlias, string prompt)
    {
        var arguments = $"complete \"{EscapeArgument(modelAlias)}\" \"{EscapeArgument(prompt)}\" -m 256 -t 0.7 -o json";
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "foundry",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        var stdout = await process.StandardOutput.ReadToEndAsync();
        var stderr = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException(string.IsNullOrWhiteSpace(stderr) ? "Foundry Local returned a non-zero exit code." : stderr.Trim());
        }

        if (string.IsNullOrWhiteSpace(stdout))
        {
            throw new InvalidOperationException("Foundry Local returned no JSON response.");
        }

        using var json = JsonDocument.Parse(stdout);
        var text = json.RootElement.TryGetProperty("text", out var value) ? value.GetString() : null;

        return string.IsNullOrWhiteSpace(text) ? stdout.Trim() : text.Trim();
    }

    private static string EscapeArgument(string value)
    {
        return value.Replace("\"", "\\\"");
    }

    private static int EstimateTokens(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return 0;
        }

        return Math.Max(1, value.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Length);
    }
}
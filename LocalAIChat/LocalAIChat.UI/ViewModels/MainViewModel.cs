using System.Threading.Tasks;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LocalAIChat.Core.Abstractions;
using LocalAIChat.Core.Domain;

namespace LocalAIChat.UI.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly IChatEngine _engine;
    private readonly IPromptManager _promptManager;

    [ObservableProperty]
    private SessionItemViewModel? selectedSession;

    [ObservableProperty]
    private string userInput = string.Empty;

    [ObservableProperty]
    private string selectedMode = "smart";

    [ObservableProperty]
    private string chatOutput = string.Empty;

    [ObservableProperty]
    private string currentModel = "-";

    [ObservableProperty]
    private string currentMode = "-";

    [ObservableProperty]
    private int inputTokens;

    [ObservableProperty]
    private int outputTokens;

    [ObservableProperty]
    private long generationDurationMs;

    [ObservableProperty]
    private int contextMessageCount;

    public MainViewModel(IChatEngine engine, IPromptManager promptManager)
    {
        _engine = engine;
        _promptManager = promptManager;
        SessionList = new SessionListViewModel();
        SessionList.PropertyChanged += OnSessionListPropertyChanged;
        SelectedSession = SessionList.SelectedSession;
    }

    public SessionListViewModel SessionList { get; }

    public ContextInspectorViewModel ContextInspector { get; } = new();

    public string[] ModeOptions { get; } = ["fast", "main", "deep", "smart", "search"];

    partial void OnSelectedSessionChanged(SessionItemViewModel? value)
    {
        ChatOutput = value?.ChatOutput ?? string.Empty;
        ContextInspector.Refresh(
            value?.Session,
            value is null ? string.Empty : _promptManager.GetProfile(value.Session.ProfileName).SystemPrompt);
        ResetStats();
    }

    private void OnSessionListPropertyChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == nameof(SessionListViewModel.SelectedSession))
        {
            SelectedSession = SessionList.SelectedSession;
        }
    }

    [RelayCommand]
    public async Task SendAsync()
    {
        if (string.IsNullOrWhiteSpace(UserInput) || SelectedSession is null)
        {
            return;
        }

        var input = UserInput.Trim();
        var session = SelectedSession.Session;
        session.Messages.Add(new ChatMessage("user", input));
        SelectedSession.UpdateTitleFromFirstMessage();

        var request = new ChatRequest
        {
            Text = input,
            Mode = ParseMode(SelectedMode)
        };

        ContextInspector.RecordContext(session, request.Mode?.ToString() ?? "Smart");

        var result = await _engine.SendMessageAsync(request, session);

        SelectedSession.ChatOutput += $"\nTy: {input}\nAI: {result.Response}\n";
        ChatOutput = SelectedSession.ChatOutput;
        CurrentModel = result.Stats.ModelAlias;
        CurrentMode = GetModeLabel(result.Stats.Mode);
        InputTokens = result.Stats.InputTokens;
        OutputTokens = result.Stats.OutputTokens;
        GenerationDurationMs = result.Stats.DurationMs;
        ContextMessageCount = session.Messages.Count;
        ContextInspector.Refresh(
            session,
            _promptManager.GetProfile(session.ProfileName).SystemPrompt);
        UserInput = string.Empty;
    }

    private void ResetStats()
    {
        CurrentModel = "-";
        CurrentMode = "-";
        InputTokens = 0;
        OutputTokens = 0;
        GenerationDurationMs = 0;
        ContextMessageCount = 0;
    }

    private static string GetModeLabel(ChatMode mode)
    {
        return mode switch
        {
            ChatMode.DeepReasoning => "deep",
            _ => mode.ToString().ToLowerInvariant()
        };
    }

    private static ChatMode ParseMode(string mode)
    {
        return mode.ToLowerInvariant() switch
        {
            "fast" => ChatMode.Fast,
            "main" => ChatMode.Main,
            "deep" => ChatMode.DeepReasoning,
            "search" => ChatMode.SearchOnline,
            _ => ChatMode.Smart
        };
    }
}

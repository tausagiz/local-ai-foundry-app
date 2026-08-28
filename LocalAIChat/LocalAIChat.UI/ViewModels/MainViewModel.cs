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

    [ObservableProperty]
    private SessionItemViewModel? selectedSession;

    [ObservableProperty]
    private string userInput = string.Empty;

    [ObservableProperty]
    private string chatOutput = string.Empty;

    public MainViewModel(IChatEngine engine)
    {
        _engine = engine;
        SessionList = new SessionListViewModel();
        SessionList.PropertyChanged += OnSessionListPropertyChanged;
        SelectedSession = SessionList.SelectedSession;
    }

    public SessionListViewModel SessionList { get; }

    partial void OnSelectedSessionChanged(SessionItemViewModel? value)
    {
        ChatOutput = value?.ChatOutput ?? string.Empty;
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
            ForceMode = ChatMode.Smart
        };

        var result = await _engine.SendMessageAsync(request, session);

        SelectedSession.ChatOutput += $"\nTy: {input}\nAI: {result.Response}\n";
        ChatOutput = SelectedSession.ChatOutput;
        UserInput = string.Empty;
    }
}

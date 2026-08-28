using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LocalAIChat.Core.Abstractions;
using LocalAIChat.Core.Domain;

namespace LocalAIChat.UI.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly IChatEngine _engine;
    private readonly ChatSession _session = new()
    {
        Id = Guid.NewGuid()
    };

    [ObservableProperty]
    private string userInput = string.Empty;

    [ObservableProperty]
    private string chatOutput = string.Empty;

    public MainViewModel(IChatEngine engine)
    {
        _engine = engine;
    }

    [RelayCommand]
    public async Task SendAsync()
    {
        if (string.IsNullOrWhiteSpace(UserInput))
        {
            return;
        }

        var request = new ChatRequest
        {
            Text = UserInput,
            ForceMode = ChatMode.Smart
        };

        var result = await _engine.SendMessageAsync(request, _session);

        ChatOutput += $"\nTy: {UserInput}\nAI: {result.Response}\n";
        UserInput = string.Empty;
    }
}

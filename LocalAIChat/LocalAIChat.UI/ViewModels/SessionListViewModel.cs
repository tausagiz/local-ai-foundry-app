using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LocalAIChat.Core.Domain;

namespace LocalAIChat.UI.ViewModels;

public partial class SessionListViewModel : ViewModelBase
{
    public SessionListViewModel()
    {
        Sessions = new ObservableCollection<SessionItemViewModel>();
        CreateSession();
    }

    public ObservableCollection<SessionItemViewModel> Sessions { get; }

    [ObservableProperty]
    private SessionItemViewModel? selectedSession;

    [RelayCommand]
    private void CreateSession()
    {
        var session = new SessionItemViewModel(
            new ChatSession
            {
                Id = Guid.NewGuid(),
                ProfileName = "PL_Analytical"
            },
            "Nowa rozmowa");

        Sessions.Insert(0, session);
        SelectedSession = session;
    }
}
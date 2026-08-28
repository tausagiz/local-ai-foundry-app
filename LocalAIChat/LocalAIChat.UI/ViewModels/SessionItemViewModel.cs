using System;
using LocalAIChat.Core.Domain;

namespace LocalAIChat.UI.ViewModels;

public partial class SessionItemViewModel : ViewModelBase
{
    public SessionItemViewModel(ChatSession session, string title)
    {
        Session = session;
        Title = title;
    }

    public ChatSession Session { get; }

    public string Title { get; private set; }

    public string ChatOutput { get; set; } = string.Empty;

    public void UpdateTitleFromFirstMessage()
    {
        var firstMessage = Session.Messages.Find(message => message.Role == "user");
        if (firstMessage is null || Title != "Nowa rozmowa")
        {
            return;
        }

        Title = firstMessage.Content.Length >  thirtyFiveCharacters
            ? $"{firstMessage.Content[..thirtyFiveCharacters]}..."
            : firstMessage.Content;
        OnPropertyChanged(nameof(Title));
    }

    private const int thirtyFiveCharacters = 35;
}
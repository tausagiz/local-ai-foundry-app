using System;
using System.Collections.Generic;
using LocalAIChat.Core.Domain;

namespace LocalAIChat.UI.Services;

public static class SessionManager
{
    public static ChatSession CurrentSession { get; private set; }

    static SessionManager()
    {
        CurrentSession = new ChatSession
        {
            Id = Guid.NewGuid(),
            ProfileName = "PL_Analytical",
            Messages = new List<ChatMessage>()
        };
    }
}

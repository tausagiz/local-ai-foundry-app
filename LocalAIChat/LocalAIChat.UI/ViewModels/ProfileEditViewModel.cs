using CommunityToolkit.Mvvm.ComponentModel;
using LocalAIChat.Core.Domain;

namespace LocalAIChat.UI.ViewModels;

public partial class ProfileEditViewModel : ViewModelBase
{
    public ProfileEditViewModel(string profileName, PromptConfig profile)
    {
        ProfileName = profileName;
        SystemPrompt = profile.SystemPrompt;
        ModelAlias = profile.ModelAlias;
    }

    public string ProfileName { get; }

    [ObservableProperty]
    private string systemPrompt;

    [ObservableProperty]
    private string modelAlias;

    public PromptConfig ToPromptConfig()
    {
        return new PromptConfig
        {
            SystemPrompt = SystemPrompt.Trim(),
            ModelAlias = ModelAlias.Trim()
        };
    }
}
using Avalonia.Controls;
using LocalAIChat.UI.ViewModels;

namespace LocalAIChat.UI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void EditProfile_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is not MainViewModel viewModel)
        {
            return;
        }

        var editViewModel = new ProfileEditViewModel(
            viewModel.CurrentProfileName,
            viewModel.GetCurrentProfile());
        var dialog = new ProfileEditWindow
        {
            DataContext = editViewModel
        };

        var saved = await dialog.ShowDialog<bool?>(this);
        if (saved == true)
        {
            viewModel.SaveCurrentProfile(editViewModel.ToPromptConfig());
        }
    }
}
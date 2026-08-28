using Avalonia.Controls;
using Avalonia.Interactivity;

namespace LocalAIChat.UI.Views;

public partial class ProfileEditWindow : Window
{
    public ProfileEditWindow()
    {
        InitializeComponent();
    }

    private void Save_Click(object? sender, RoutedEventArgs e)
    {
        Close(true);
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        Close(false);
    }
}
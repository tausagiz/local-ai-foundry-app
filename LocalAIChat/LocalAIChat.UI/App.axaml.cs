using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using LocalAIChat.Core.Abstractions;
using LocalAIChat.Core.Context;
using LocalAIChat.Core.Engine;
using LocalAIChat.Core.Memory;
using LocalAIChat.Core.Model;
using LocalAIChat.Core.Modes;
using LocalAIChat.Core.Prompts;
using LocalAIChat.Core.Statistics;
using LocalAIChat.UI.ViewModels;
using LocalAIChat.UI.Views;
using Microsoft.Extensions.DependencyInjection;

namespace LocalAIChat.UI;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Services = ConfigureServices();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = Services.GetRequiredService<MainViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IFoundryModelRunner, FoundryModelRunner>();
        services.AddSingleton<IContextManager, ContextManager>();
        services.AddSingleton<IMemoryManager, MemoryManager>();
        services.AddSingleton<IModeSelector, ModeSelector>();
        services.AddSingleton<IStatsCollector, StatsCollector>();
        services.AddSingleton<IPromptManager, PromptManager>();
        services.AddSingleton<IChatEngine, ChatEngine>();

        services.AddSingleton<IChatModePlugin, FastModePlugin>();
        services.AddSingleton<IChatModePlugin, MainModePlugin>();
        services.AddSingleton<IChatModePlugin, DeepModePlugin>();
        services.AddSingleton<IChatModePlugin, SearchModePlugin>();
        services.AddSingleton<IChatModePlugin, SmartModePlugin>();

        services.AddSingleton<MainViewModel>();

        return services.BuildServiceProvider();
    }
}
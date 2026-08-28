using LocalAIChat.Core.Abstractions;
using LocalAIChat.Core.Context;
using LocalAIChat.Core.Domain;
using LocalAIChat.Core.Engine;
using LocalAIChat.Core.Memory;
using LocalAIChat.Core.Modes;
using LocalAIChat.Core.Model;
using LocalAIChat.Core.Prompts;
using LocalAIChat.Core.Statistics;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddSingleton<IChatEngine, ChatEngine>();
services.AddSingleton<IModeSelector, ModeSelector>();
services.AddSingleton<IChatModePlugin, FastModePlugin>();
services.AddSingleton<IChatModePlugin, MainModePlugin>();
services.AddSingleton<IChatModePlugin, SmartModePlugin>();
services.AddSingleton<IContextManager, ContextManager>();
services.AddSingleton<IMemoryManager, MemoryManager>();
services.AddSingleton<IStatsCollector, StatsCollector>();
services.AddSingleton<IFoundryModelRunner, FoundryModelRunner>();
services.AddSingleton<IPromptManager, PromptManager>();

using var serviceProvider = services.BuildServiceProvider();
var chatEngine = serviceProvider.GetRequiredService<IChatEngine>();
var session = new ChatSession
{
	Id = Guid.NewGuid(),
	ProfileName = "PL_Analytical"
};
var request = new ChatRequest
{
	ProfileName = session.ProfileName,
	Text = "Napisz krótkie powitanie."
};

var result = await chatEngine.SendMessageAsync(request, session);
Console.WriteLine(result.Response);

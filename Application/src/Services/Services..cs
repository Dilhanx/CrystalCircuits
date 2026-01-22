using Microsoft.Extensions.DependencyInjection;

namespace CrystalCircuits.Application.Services;

static class Service
{
    public static IServiceProvider Instance { get; set; } = null!;
    public static void Register()
    {

        ServiceCollection collection = new();
        collection.AddSingleton<PluginService>();
        collection.AddSingleton<CommandService>();
        Instance = collection.BuildServiceProvider();
    }
}

using Microsoft.Extensions.DependencyInjection;

namespace CrystalCircuits.Application.Services;

static class Service
{
    public static IServiceProvider Instance { get; set; } = null!;

    public static void Register(IServiceCollection collection)
    {
        collection.AddSingleton<PluginService>();
        Instance = collection.BuildServiceProvider();
    }
}
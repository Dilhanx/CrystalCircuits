
using System.Reflection;
using System.Runtime.Loader;
using CrystalCircuits.Core.Modules;

namespace CrystalCircuits.Application.Services;

class PluginService
{
    public List<Type> Plugins = [];


    public PluginService(string path = "Plugins")
    {
        LoadPlugins(path);
    }

    private void LoadPlugins(string path)
    {

        foreach (var type in Assembly.Load("Modules").GetTypes().Where(t => typeof(IModule).IsAssignableFrom(t)))
        {
            Plugins.Add(type);
        }
        AssemblyLoadContext assemblyLoadContext = new("Plugins");
        string[] files = Directory.GetFiles(path, "*.dll");

        foreach (string file in files)
        {
            Assembly assembly = assemblyLoadContext.LoadFromAssemblyPath(Directory.GetCurrentDirectory() + "\\" + file);
            foreach (var type in assembly.GetTypes().Where(t => typeof(IModule).IsAssignableFrom(t)))
            {
                Plugins.Add(type);
            }
        }
    }
    public IModule CreateModule(Type Plugin)
    {
        return (Activator.CreateInstance(Plugin) as IModule)!;
    }
}
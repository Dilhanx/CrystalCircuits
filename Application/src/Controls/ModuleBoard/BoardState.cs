using CrystalCircuits.Core.Modules;
using CrystalCircuits.Core.Connections;

namespace CrystalCircuits.Application.Controls.ModuleBoards;

class BoardState
{
    public List<IModule> Modules { get; private set; } = [];
    public List<IConnection> Connections { get; private set; } = [];

    public void AddModule(Type Plugin)
    {
        Modules.Add(Service.Instance.GetService<PluginService>()!.CreateModule(Plugin));
    }
    public void Save()
    {

    }
    public void Load()
    {

    }
}
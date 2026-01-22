using CrystalCircuits.Core.Modules;
using CrystalCircuits.Core.Connections;
using Avalonia.Platform.Storage;
using MessagePack;

namespace CrystalCircuits.Application.Controls.ModuleBoards;

[MessagePackObject(AllowPrivate = true)]
partial class BoardState
{
    [Key(0)]
    public List<IModule> Modules { get; private set; } = [];
    [Key(1)]
    public List<IConnection> Connections { get; private set; } = [];

    public void AddModule(Type Plugin)
    {
        Modules.Add(Service.Instance.GetService<PluginService>()!.CreateModule(Plugin));
    }
    public void New()
    {
        Modules = [];
        Connections = [];
    }
    public void Save(IStorageFile file)
    {
        File.WriteAllBytes(file.Path.AbsolutePath, MessagePackSerializer.Typeless.Serialize(this));
    }
    public void Load(IStorageFile file)
    {
        byte[] bytes = File.ReadAllBytes(file.Path.AbsolutePath);
        BoardState tempState = (BoardState)MessagePackSerializer.Typeless.Deserialize(bytes)!;
        Modules = tempState.Modules;
        Modules.ForEach(Module =>
        {
            Module.State.Hover = false;
            Module.State.Selected = false;
        });
        Connections = tempState.Connections;
    }
}

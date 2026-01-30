using CrystalCircuits.Application.Controls.ModuleBoards;
using CrystalCircuits.Core.Modules;

namespace CrystalCircuits.Application.Commands;

class MoveModuleCommand : ICommand
{
    private readonly Dictionary<IModule, Point> moduleStartPosition;
    private readonly Dictionary<IModule, Point> moduleEndPosition = [];
    public MoveModuleCommand(Dictionary<IModule, Point> moduleStartPosition)
    {
        moduleStartPosition.ToList().ForEach(module => moduleEndPosition.Add(module.Key, module.Key.View.Position));
        this.moduleStartPosition = moduleStartPosition;
    }
    public bool Do()
    {
        moduleEndPosition.ToList().ForEach(module => module.Key.View.Position = module.Value);
        return true;
    }
    public bool Undo()
    {
        moduleStartPosition.ToList().ForEach(module => module.Key.View.Position = module.Value);
        return true;
    }
}
using CrystalCircuits.Application.Controls.ModuleBoards;
using CrystalCircuits.Core.Modules;

namespace CrystalCircuits.Application.Commands;

class MoveModuleCommand : ICommand
{
    private readonly Dictionary<IModule, Point> moduleStartPosition;
    private readonly Dictionary<IModule, Point> moduleEndPosition = [];
    public MoveModuleCommand(Dictionary<IModule, Point> moduleStartPosition)
    {
        foreach (var module in moduleStartPosition)
        {
            moduleEndPosition.Add(module.Key, module.Key.View.Position);
        }

        this.moduleStartPosition = moduleStartPosition;
    }
    public bool Do()
    {
        foreach (var module in moduleEndPosition)
        {
            module.Key.View.Position = module.Value;
        }
        return true;
    }
    public bool Undo()
    {
        foreach (var module in moduleStartPosition)
        {
            module.Key.View.Position = module.Value;
        }
        return true;
    }
}
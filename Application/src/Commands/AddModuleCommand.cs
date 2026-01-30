using CrystalCircuits.Application.Controls.ModuleBoards;
using CrystalCircuits.Core.Modules;

namespace CrystalCircuits.Application.Commands;

class AddModuleCommand(BoardState boardState, Type type, Point position, Selection selection) : ICommand
{
    IModule? module;
    private readonly Dictionary<IModule, Point> moduleStartPosition = [];
    public bool Do()
    {
        module ??= Activator.CreateInstance(type) as IModule;
        module!.View.Position = position;
        boardState.Modules.Add(module);
        boardState.Modules.ForEach(module => moduleStartPosition.Add(module, module.View.Position));
        selection.Untangle([module]);
        return true;
    }
    public bool Undo()
    {
        moduleStartPosition.ToList().ForEach(module => module.Key.View.Position = module.Value);
        moduleStartPosition.Clear();
        module!.State.Clear();
        boardState.Modules.Remove(module!);
        return true;
    }
}
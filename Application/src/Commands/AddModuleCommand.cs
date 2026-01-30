using CrystalCircuits.Application.Controls.ModuleBoards;
using CrystalCircuits.Core.Modules;

namespace CrystalCircuits.Application.Commands;

class AddModuleCommand(BoardState boardState, Type type, Point position, Selection selection) : ICommand
{
    IModule? module;
    public bool Do()
    {
        module ??= Activator.CreateInstance(type) as IModule;
        module!.View.Position = position;
        boardState.Modules.Add(module);
        selection.Untangle([module]);
        return true;
    }
    public bool Undo()
    {
        boardState.Modules.Remove(module!);
        return true;
    }
}
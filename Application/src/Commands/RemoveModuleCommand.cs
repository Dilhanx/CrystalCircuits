using CrystalCircuits.Application.Controls.ModuleBoards;
using CrystalCircuits.Core.Modules;
using DynamicData;

namespace CrystalCircuits.Application.Commands;

class RemoveModuleCommand(BoardState boardState, List<IModule> modules, Selection selection) : ICommand
{
    public bool Do()
    {
        if (modules.Count == 0) return false;
        boardState.Modules.RemoveMany(modules);
        selection.DeselectAll();
        return true;
    }
    public bool Undo()
    {
        modules?.ForEach(boardState.Modules.Add);
        return true;
    }
}
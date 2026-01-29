using CrystalCircuits.Application.Controls.ModuleBoards;
using CrystalCircuits.Core.Modules;

namespace CrystalCircuits.Application.Commands;

class RemoveModuleCommand(BoardState boardState, List<IModule> modules) : ICommand
{
    public bool Do()
    {
        if (modules.Count == 0) return false;
        modules.ForEach(module => boardState.Modules.Remove(module));
        return true;
    }
    public bool Undo()
    {
        modules?.ForEach(boardState.Modules.Add);
        return true;
    }
}
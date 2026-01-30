using CrystalCircuits.Application.Controls.ModuleBoards;
using CrystalCircuits.Core.Modules;
using DynamicData;

namespace CrystalCircuits.Application.Commands;

class CutModuleCommand(BoardState boardState,Selection selection) : ICommand
{
    List<IModule>? oldClipBoard;
    List<IModule>? newClipBoard;
    List<IModule>? cutModules;

    public bool Do()
    {
        if (newClipBoard is null)
            oldClipBoard = selection.clipBoard;
        if (newClipBoard is null)
            newClipBoard = selection.Clone(selection.Selected);
        else
            selection.Clone(newClipBoard);
        cutModules ??= selection.Selected;
        selection.DeselectAll();
        cutModules.ForEach(module => module.State.Clear());
        boardState.Modules.RemoveMany(cutModules);
        return true;
    }
    public bool Undo()
    {
        newClipBoard = selection.Clone(oldClipBoard!);
        boardState.Modules.Add(cutModules!);
        return true;
    }
}
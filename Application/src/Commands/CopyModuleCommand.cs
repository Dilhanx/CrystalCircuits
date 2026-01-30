using CrystalCircuits.Application.Controls.ModuleBoards;
using CrystalCircuits.Core.Modules;

namespace CrystalCircuits.Application.Commands;

class CopyModuleCommand(Selection selection) : ICommand
{
    List<IModule>? oldClipBoard;
    List<IModule>? newClipBoard;
    public bool Do()
    {
        if (newClipBoard is null)
            oldClipBoard = selection.clipBoard;
        if (newClipBoard is null)
            newClipBoard = selection.Clone(selection.Selected);
        else
            selection.Clone(newClipBoard);
        return true;
    }
    public bool Undo()
    {
        newClipBoard = selection.Clone(oldClipBoard!);
        return true;
    }
}
using CrystalCircuits.Application.Controls.ModuleBoards;
using CrystalCircuits.Core.Modules;
using DynamicData;

namespace CrystalCircuits.Application.Commands;

class PasteModuleCommand(BoardState boardState, Selection selection) : ICommand
{
    List<IModule>? clipBoard;
    Point? mousePosition;
    private readonly Dictionary<IModule, Point> moduleStartPosition = [];


    public bool Do()
    {
        clipBoard ??= selection.clipBoard;
        mousePosition ??= selection.MouseCanvasPosition;

        Rect bounds = new();
        Dictionary<IModule, Point> offset = [];

        clipBoard.ForEach(module => bounds = bounds.Union(module.View.Rect));
        clipBoard.ForEach(module => offset.Add(module, module.View.Position - bounds.Center));

        clipBoard.ForEach(clipBoard => clipBoard.View.Position = (Point)(offset[clipBoard] + mousePosition));
        boardState.Modules.Add(clipBoard);

        boardState.Modules.ForEach(module => moduleStartPosition.Add(module, module.View.Position));
        selection.Untangle(clipBoard);
        selection.Clone(clipBoard);
        return true;
    }
    public bool Undo()
    {
        moduleStartPosition.ToList().ForEach(module => module.Key.View.Position = module.Value);
        moduleStartPosition.Clear();
        clipBoard!.ForEach(module=> module.State.Clear());
        boardState.Modules.Remove(clipBoard!);
        return true;
    }
}
using System.Collections;

namespace CrystalCircuits.Application.Services;

class CommandService
{
    private Stack<ICommand> doCommands = [];
    private Stack<ICommand> undoCommands = [];
    public Stack<ICommand> Commands { get => new Stack<ICommand>(doCommands.Concat(undoCommands)); }
    public void Do(ICommand command, params object[] parameter)
    {
        if (command.Do())
        {
            doCommands.Push(command);
            undoCommands = [];
        }
    }
    public void Undo()
    {
        if (doCommands.Count == 0) return;
        ICommand command = doCommands.Pop();
        if (command.Undo())
        {
            undoCommands.Push(command);
        }
    }

    public void Redo()
    {
        if (undoCommands.Count == 0) return;
        ICommand command = undoCommands.Pop();
        if (command.Do())
        {
            doCommands.Push(command);
        }
    }
    public void Clear()
    {
        doCommands.Clear();
        undoCommands.Clear();
    }

}
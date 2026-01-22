namespace CrystalCircuits.Application.Commands;

interface ICommand
{
    public bool Do();
    public bool Undo();
}
namespace CrystalCircuits.Core.Component;

interface IComponent
{
    public string Name { get; init; }
    public string Description { get; init; }

    public void HandleInput();
    public void Draw();
}
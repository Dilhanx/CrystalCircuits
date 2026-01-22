namespace CrystalCircuits.Core.Modules;

public interface IModule
{
    public string Name { get; init; }
    public string Description { get; init; }
    public List<string> Tags { get; init; }

    public State State { get; init; }
    public View View { get; init; }

    public void Draw(DrawingContext context);
    public void Update();
}
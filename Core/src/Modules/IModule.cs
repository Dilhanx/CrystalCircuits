namespace CrystalCircuits.Core.Modules;


public interface IModule
{
    public string Name { get; init; }
    public string Description { get; init; }
    public List<string> Tags { get; init; }

    public void Update();
    public void Draw();
}
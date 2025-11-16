namespace Modules;

public class Class1 : IModule
{
    public string Name { get; init; } = "Class 1 x";
    public string Description { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }
    public List<string> Tags { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }

    public void Draw()
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        throw new NotImplementedException();
    }
}

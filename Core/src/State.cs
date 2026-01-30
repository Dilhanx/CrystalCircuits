namespace CrystalCircuits.Core;

public class State
{
    public bool Selected { get; set; } = false;
    public bool Hover { get; set; } = false;
    public bool Draw { get; set; } = true;

    public void Clear()
    {
        Selected = false;
        Hover = false;
    }
}
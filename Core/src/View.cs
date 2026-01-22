namespace CrystalCircuits.Core;

public class View
{
    public Size Size { get; set; } = new(0, 0);
    public Point Position { get; set; } = new(0, 0);

    public bool Contains(Point point) => point.X >= Position.X && point.X <= Position.X + Size.Width &&
                                         point.Y >= Position.Y && point.Y <= Position.Y + Size.Height;

    public Rect Rect => new(Position, Size);
}
using System.Globalization;
using Avalonia;
using Avalonia.Media;
using CrystalCircuits.Core;

namespace CrystalCircuits.Modules;

public class Test : IModule
{
    public int id = Random.Shared.Next();
    public string Name { get; init; } = "Test Module";
    public string Description { get; init; } = "A module to test board capabilities";
    public List<string> Tags { get; init; } = ["Test", "Utility"];
    public State State { get; init; } = new();
    public View View { get; init; } = new()
    {
        Size = new(Random.Shared.Next(225) + 25, Random.Shared.Next(225) + 25),
        Position = new(0, 0)
    };
    public void Draw(DrawingContext context)
    {
        if (State.Selected)
        {
            context.DrawRectangle(Brushes.Green, new Pen(0), new Rect(View.Size), 0);
        }
        else
        {
            context.DrawRectangle(Brushes.Red, new Pen(0), new Rect(View.Size), 0);
        }
        if (State.Hover)
        {
            // context.DrawRectangle(Brushes.Yellow, new Pen(0), new Rect(View.Size / 2), 0);
        }
        context.DrawText(
            new FormattedText(
            id.ToString(),
            CultureInfo.InvariantCulture, // Initial culture
            FlowDirection.LeftToRight,
            new Typeface("helvetica", FontStyle.Normal, FontWeight.ExtraLight),
            12,
            Brushes.Black),
            new Point(View.Size.Width / 2, View.Size.Height / 2)
        );
        context.DrawText(
            new FormattedText(
            View.Position.ToString(),
            CultureInfo.InvariantCulture, // Initial culture
            FlowDirection.LeftToRight,
            new Typeface("helvetica", FontStyle.Normal, FontWeight.ExtraLight),
            12,
            Brushes.Black),
            new Point(0, 0)
        );
    }

    public void Update()
    {
    }
}

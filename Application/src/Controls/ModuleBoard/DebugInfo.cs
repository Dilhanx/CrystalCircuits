using System.Globalization;
using Avalonia.Media;

namespace CrystalCircuits.Application.Controls.ModuleBoards;

class DebugInfo()
{
    int counter = 0;
    public void Reset()
    {
        counter = 0;
    }
    public void Draw(DrawingContext context, string text)
    {
        context.DrawText(new FormattedText(
               text,
               CultureInfo.InvariantCulture, // Initial culture
               FlowDirection.LeftToRight,
               new Typeface("Arial"),
               12.0,
               Brushes.Black), new Point(0, counter * 30)
               );
        counter++;
    }
    public void Draw(DrawingContext context, string text, Point position)
    {
        context.DrawText(new FormattedText(
               text,
               CultureInfo.InvariantCulture, // Initial culture
               FlowDirection.LeftToRight,
               new Typeface("Arial"),
               12.0,
               Brushes.Black), position
               );
    }

}
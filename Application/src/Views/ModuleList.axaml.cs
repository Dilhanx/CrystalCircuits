using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace CrystalCircuits.Application;


public partial class ModuleList : UserControl
{
    public ModuleList()
    {
        InitializeComponent();
    }
    public void ButtonClickHandler(object sender, RoutedEventArgs e)
    {
        // Code to execute when the button is clicked
        if (sender is Button button)
        {
            button.Content = "Button Clicked!";
        }
    }
}

public class MyCustomControl : Control
{
    public IBrush? Background { get; set; }

    public sealed override void Render(DrawingContext context)
    {
        if (Background != null)
        {
            var renderSize = Bounds.Size;
            context.FillRectangle(Background, new Rect(renderSize));
            context.DrawText(new FormattedText(
                "Hello, World!",
                CultureInfo.InvariantCulture, // Initial culture
                FlowDirection.LeftToRight,
                new Typeface("Arial"),
                12.0,
                Brushes.Black), new(0, 0)
                );
        }

        base.Render(context);
    }
}
using System.Diagnostics;
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
    public void Dump(DrawingContext context, BoardState boardState, Selection selection)
    {
        var PluginService = Service.Instance.GetService<PluginService>();
        foreach (var plugin in PluginService!.Plugins.Index())
        {
            context.DrawText(
                new FormattedText(
                    plugin.Item.Name,
                    CultureInfo.InvariantCulture, // Initial culture
                    FlowDirection.LeftToRight,
                    new Typeface("helvetica", FontStyle.Normal, FontWeight.ExtraLight),
                    12,
                    Brushes.Black
                ),
                new(100, plugin.Index * 25)
            );
        }
        foreach (var plugin in boardState.Modules.Index())
        {
            context.DrawText(
                new FormattedText(
                    plugin.Item.Name,
                    CultureInfo.InvariantCulture, // Initial culture
                    FlowDirection.LeftToRight,
                    new Typeface("helvetica", FontStyle.Normal, FontWeight.ExtraLight),
                    12,
                    Brushes.Black
                ),
                new(200, plugin.Index * 25)
            );
        }
        foreach (var plugin in Service.Instance.GetService<CommandService>()!.Commands.Index())
        {
            context.DrawText(
                new FormattedText(
                    plugin.Item.GetType().ToString(),
                    CultureInfo.InvariantCulture, // Initial culture
                    FlowDirection.LeftToRight,
                    new Typeface("helvetica", FontStyle.Normal, FontWeight.ExtraLight),
                    12,
                    Brushes.Black
                ),
                new(300, plugin.Index * 25)
            );
        }
        foreach (var plugin in selection.Selected.Index())
        {
            context.DrawText(
                new FormattedText(
                    plugin.Item.GetType().ToString(),
                    CultureInfo.InvariantCulture, // Initial culture
                    FlowDirection.LeftToRight,
                    new Typeface("helvetica", FontStyle.Normal, FontWeight.ExtraLight),
                    12,
                    Brushes.Black
                ),
                new(800, plugin.Index * 25)
            );
        }

        // Task.Run(() =>
        // {
        //     using (Process currentProcess = Process.GetCurrentProcess())
        //     {
        //         currentProcess.Refresh();
        //         long privateMemoryBytes = currentProcess.PrivateMemorySize64;
        //         double privateMemoryMB = Math.Round(privateMemoryBytes / (1024.0 * 1024.0), 2);
        //         long workingSetBytes = currentProcess.WorkingSet64;
        //         double workingSetMB = Math.Round(workingSetBytes / (1024.0 * 1024.0), 2);
        //         Draw(context, $"{privateMemoryMB} / {workingSetMB}");
        //     }
        // });
    }
}
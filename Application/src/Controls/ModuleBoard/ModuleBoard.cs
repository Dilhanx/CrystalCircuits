using System.Globalization;
using System.Numerics;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using CrystalCircuits.Application.Controls.ModuleBoards;

namespace CrystalCircuits.Application.Controls;

public class ModuleBoard : UserControl
{
    private readonly CameraController CameraController = new();
    private readonly DeltaTime DeltaTime = new();
    private readonly DebugInfo DebugInfo = new();

    public ModuleBoard()
    {
        Loaded += StartUpdateLoop!;
    }
    protected override void OnPointerMoved(PointerEventArgs e)
    {
        Service.Instance.GetService<InputService>()!.MousePosition = e.GetPosition(this);
        CameraController.Panning(e.GetPosition(this));
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        // if (e.Properties.IsLeftButtonPressed)
        // {
        //     var myFlyout = new Flyout
        //     {
        //         Placement = PlacementMode.Center,
        //         Content = new ModuleList()
        //     };
        //     myFlyout.ShowAt(this);
        // }
        if (e.Properties.IsMiddleButtonPressed)
        {
            CameraController.PanStart(e.GetPosition(this));
        }
        // if (e.Properties.IsRightButtonPressed)
        // {
        //     var menuItem = new MenuItem();
        //     menuItem.Header = "Test";
        //     menuItem.Click += OpenFileMenuItem_Click!;
        //     var flyout = new MenuFlyout()
        //     {
        //         Items =
        //         {
        //             new MenuItem() {Header = "Hello"},
        //             new MenuItem() {Header = "Wolrd"},
        //             menuItem
        //         },
        //         Placement = PlacementMode.Center,

        //     };
        //     flyout.ShowAt(this, true);
        // }
    }
    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        if (e.Properties.PointerUpdateKind == PointerUpdateKind.MiddleButtonReleased)
        {
            CameraController.PanEnd();
        }
    }
    private void OpenFileMenuItem_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Console.WriteLine("Test");
    }
    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        CameraController.ZoomCamera((float)e.Delta.Y);
    }
    private void StartUpdateLoop(object sender, RoutedEventArgs e)
    {
        TopLevel.GetTopLevel(this)?.RequestAnimationFrame(Update);
    }


    private void Update(TimeSpan elapsedTime)
    {
        DeltaTime.Calculate(elapsedTime);
        TopLevel.GetTopLevel(this)?.RequestAnimationFrame(Update);
        InvalidateVisual();
    }

    public sealed override void Render(DrawingContext context)
    {
        base.Render(context);
        context.DrawRectangle(Brushes.Transparent, new Pen(0), new Rect(Bounds.Size), 0);
        DebugInfo.Reset();
        DebugInfo.Draw(context, DeltaTime.Time.ToString());
        DebugInfo.Draw(context, DeltaTime.Fps.ToString());

        context.DrawText(new FormattedText(
                       "Hello, World!",
                       CultureInfo.InvariantCulture, // Initial culture
                       FlowDirection.LeftToRight,
                       new Typeface("Arial"),
                       12.0,
                       Brushes.Black), Service.Instance.GetService<InputService>().MousePosition
                       );
        var PluginService = Service.Instance.GetService<PluginService>();

        foreach (var plugin in PluginService.Plugins.Index())
        {
            context.DrawText(
                new FormattedText(
                    plugin.Item.Name,
                    CultureInfo.InvariantCulture, // Initial culture
                    FlowDirection.LeftToRight,
                    new Typeface("helvetica", FontStyle.Normal, FontWeight.ExtraLight),
                    32,
                    Brushes.Black
                ),
                new(100, plugin.Index * 25)
            );
        }
        context.PushTransform(Matrix.CreateScale(CameraController.Zoom));
        context.PushTransform(Matrix.CreateTranslation(CameraController.Position));
        var pen = new Pen(Brushes.Red, 10);
        context.DrawLine(pen, new Point(0, 0), new Point(1000, 2000));


    }
}
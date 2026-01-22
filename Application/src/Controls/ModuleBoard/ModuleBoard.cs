using System.Globalization;
using System.Numerics;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using CrystalCircuits.Application.Controls.ModuleBoards;
using ReactiveUI;

namespace CrystalCircuits.Application.Controls;

public class ModuleBoard : UserControl
{
    private readonly CameraController cameraController = new();
    private readonly DeltaTime deltaTime = new();
    private readonly DebugInfo debugInfo = new();
    private readonly BoardState boardState;
    private readonly Selection selection;


    public ModuleBoard()
    {
        Loaded += StartUpdateLoop!;
        SizeChanged += SizeChange;
        boardState = Service.Instance.GetService<ProjectService>()!.boardState;
        Focusable = true;
        selection = new(cameraController, boardState, deltaTime);
        Hotkeys.Add(this, KeyBindings, boardState, selection);

    }

    private void SizeChange(object? sender, SizeChangedEventArgs e)
    {
        cameraController.ViewportSize = Bounds.Size;
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        selection.MouseViewportPosition = e.GetPosition(this);
        cameraController.Panning(e.GetPosition(this));
        selection.PointerMoved(this, e);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        if (e.Properties.IsLeftButtonPressed)
            selection.PointerPressed(e);
        if (e.Properties.IsMiddleButtonPressed)
            cameraController.PanStart(this, e.GetPosition(this));
        if (e.Properties.IsRightButtonPressed)
        {
            var AddMenuItem = new MenuItem
            {
                Header = "Add",
                Command = ReactiveCommand.Create(() =>
                    {
                        var position = selection.MouseCanvasPosition;
                        Service.Instance.GetService<CommandService>()!.Do(new AddModuleCommand(boardState, typeof(Test), position));
                    })
            };

            var flyout = new MenuFlyout()
            {
                Items =
                {
                    AddMenuItem
                },
                Placement = PlacementMode.Center,

            };
            flyout.ShowAt(this, true);
        }
    }
    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        if (e.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonReleased)
            selection.PointerReleased(this, e);
        if (e.Properties.PointerUpdateKind == PointerUpdateKind.MiddleButtonReleased)
            cameraController.PanEnd(this);
    }
    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        cameraController.ZoomCamera((float)e.Delta.Y);
    }
    private void StartUpdateLoop(object sender, RoutedEventArgs e)
    {
        this.Focus();
        TopLevel.GetTopLevel(this)?.RequestAnimationFrame(Update);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {

    }
    protected override void OnKeyUp(KeyEventArgs e)
    {

    }

    private void Update(TimeSpan elapsedTime)
    {
        deltaTime.Calculate(elapsedTime);
        foreach (var module in boardState.Modules)
        {
            module.Update();
        }
        selection.Hover();

        TopLevel.GetTopLevel(this)?.RequestAnimationFrame(Update);
        InvalidateVisual();
    }
    public sealed override void Render(DrawingContext context)
    {
        base.Render(context);
        // context.DrawRectangle(new SolidColorBrush(new Color(50, 245, 245, 245)), new Pen(0), new Rect(Bounds.Size), 0);
        context.DrawRectangle(Brushes.Transparent, new Pen(0), new Rect(Bounds.Size), 0);

        context.PushTransform(Matrix.CreateTranslation(cameraController.Position));
        context.PushTransform(Matrix.CreateScale(cameraController.Zoom));

        foreach (var module in boardState.Modules)
        {
            context.PushTransform(Matrix.CreateTranslation(module.View.Position));
            if (module.View.Rect.Intersects(cameraController.Rect))
                module.Draw(context);
            context.PushTransform(Matrix.CreateTranslation(-module.View.Position));
        }
        selection.DrawBoxSelect(context);

        var pen = new Pen(Brushes.Red, 10);
        context.DrawLine(pen, new Point(0, 0), new Point(1000, 2000));

        context.PushTransform(Matrix.CreateScale(new Vector2(1 / cameraController.Zoom.X, 1 / cameraController.Zoom.Y)));
        context.PushTransform(Matrix.CreateTranslation(-cameraController.Position));


        debugInfo.Reset();
        debugInfo.Draw(context, $"{deltaTime.Time.ToString()} Time");
        debugInfo.Draw(context, $"{deltaTime.Fps.ToString()} Fps");
        debugInfo.Draw(context, $"Position {cameraController.Position.ToString()} ");
        debugInfo.Draw(context, $"Size {cameraController.ViewportSize.ToString()} ");
        debugInfo.Draw(context, $"Zoom {cameraController.Zoom.ToString()} ");
        debugInfo.Draw(context, $"MouseViewportPosition{selection.MouseViewportPosition.ToString()} ");
        debugInfo.Draw(context, $"MouseCanvasPosition {selection.MouseCanvasPosition.ToString()} ");
        debugInfo.Draw(context, $"{selection.boxSelect.TopLeft.ToString()} ");
        debugInfo.Draw(context, $"{selection.boxSelect.BottomRight.ToString()} ");

        debugInfo.Dump(context, boardState, selection);

    }
}
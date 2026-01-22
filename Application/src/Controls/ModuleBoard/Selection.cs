using Avalonia.Input;
using Avalonia.Media;
using CrystalCircuits.Core.Modules;

namespace CrystalCircuits.Application.Controls.ModuleBoards;

class Selection(CameraController cameraController, BoardState boardState, DeltaTime deltaTime)
{
    public List<IModule> Selected { get; set; } = [];
    public Point MouseViewportPosition { get; set; } = new Point();
    public Point MouseCanvasPosition
    {
        get
        {
            field = new((MouseViewportPosition.X - cameraController.Position.X) / cameraController.Zoom.X,
                        (MouseViewportPosition.Y - cameraController.Position.Y) / cameraController.Zoom.Y
                        );
            return field;
        }
        private set;
    } = new Point();

    private void Select(List<IModule> modules)
    {
        modules.ForEach(module => module.State.Selected = true);
        Selected = [.. modules];

    }
    private void Deselect(List<IModule> modules)
    {
        modules.ForEach(module => module.State.Selected = false);
        Selected = [.. Selected.Except(modules)];
    }
    public void SelectAll()
    {
        Select(boardState.Modules);
    }

    public void DeselectAll()
    {
        Deselect(Selected);
    }

    private bool hit = false;
    public Rect boxSelect = new();
    private bool boxSelecting = false;
    private bool moving = false;
    private Point moveStartPosition = new();
    private IModule? tempSelected;
    private Dictionary<IModule, Point> moduleStartPosition = [];
    public void PointerPressed(PointerPressedEventArgs e)
    {
        boxSelecting = false;
        hit = false;
        foreach (var module in boardState.Modules)
        {
            if (module.View.Contains(MouseCanvasPosition))
            {
                if (e.KeyModifiers == KeyModifiers.Shift)
                {
                    if (!module.State.Selected)
                        Select([.. Selected, module]);
                    else
                        Deselect([module]);
                }
                else if (e.KeyModifiers == KeyModifiers.Control)
                {
                    if (module.State.Selected)
                        Deselect([module]);
                }
                else
                {
                    if (module.State.Selected)
                    {
                        tempSelected = module;
                    }
                    else
                    {
                        DeselectAll();
                        Select([module]);
                    }

                }
                hit = true;
            }
        }
        if (hit)
        {
            if (e.KeyModifiers != KeyModifiers.Shift
            && e.KeyModifiers != KeyModifiers.Control
            && e.KeyModifiers != (KeyModifiers.Shift | KeyModifiers.Control))
            {
                moving = true;
                moveStartPosition = MouseCanvasPosition;
                boardState.Modules.ForEach(module => moduleStartPosition.Add(module, module.View.Position));
            }
        }
        else
        {
            if (e.KeyModifiers != KeyModifiers.Shift
            && e.KeyModifiers != KeyModifiers.Control
            && e.KeyModifiers != (KeyModifiers.Shift | KeyModifiers.Control))
                DeselectAll();
            boxSelecting = true;
            boxSelect = new(MouseCanvasPosition, MouseCanvasPosition);
        }

    }
    bool boxSelectingMoved = false;
    bool moved = false;
    public void PointerMoved(ModuleBoard moduleBoard, PointerEventArgs e)
    {
        if (boxSelecting)
        {
            boxSelectingMoved = true;
            boxSelect = new(boxSelect.TopLeft, MouseCanvasPosition);
            moduleBoard.Cursor = new Cursor(StandardCursorType.DragMove);
        }
        if (moving)
        {
            Layout();
            moved = true;

            moduleBoard.Cursor = new Cursor(StandardCursorType.SizeAll);
        }
    }
    public void PointerReleased(ModuleBoard moduleBoard, PointerReleasedEventArgs e)
    {
        if (boxSelectingMoved)
        {
            if (e.KeyModifiers == KeyModifiers.None || e.KeyModifiers == KeyModifiers.Shift)
            {
                foreach (var module in boardState.Modules)
                {
                    if (boxSelect.Normalize().Intersects(module.View.Rect))
                    {
                        if (!module.State.Selected)
                            Select([.. Selected, module]);
                    }
                }
            }
            if (e.KeyModifiers == KeyModifiers.Control)
            {
                foreach (var module in boardState.Modules)
                {
                    if (boxSelect.Normalize().Intersects(module.View.Rect))
                    {
                        if (module.State.Selected)
                            Deselect([module]);
                    }
                }
            }
        }
        if (moved)
        {
            Service.Instance.GetService<CommandService>()!.Do(new MoveModuleCommand(moduleStartPosition));
        }
        if (!boxSelectingMoved && !moved && hit && tempSelected is not null)
        {
            DeselectAll();
            Select([tempSelected]);
            tempSelected = null;
        }
        boxSelectingMoved = false;
        boxSelecting = false;
        moving = false;
        moved = false;
        moduleStartPosition = new();
        moduleBoard.Cursor = new Cursor(StandardCursorType.Arrow);
    }
    private void Layout()
    {
        Dictionary<IModule, bool> frozenModules = [];
        foreach (var module in moduleStartPosition)
        {
            frozenModules.Add(module.Key, false);
        }

        var moveDelta = MouseCanvasPosition - moveStartPosition;
        Selected.ForEach(module => module.View.Position = moduleStartPosition[module] + moveDelta);
        Selected.ForEach(module => frozenModules[module] = true);

        bool completed = false;
        while (!completed)
        {
            completed = true;
            foreach (var frozenModule in frozenModules)
            {
                foreach (var module in boardState.Modules)
                {
                    if (frozenModule.Key.View.Rect.Intersects(module.View.Rect))
                    {
                        
                    }
                }
            }
        }
    }
    public void DrawBoxSelect(DrawingContext context)
    {
        if (boxSelecting)
            context.DrawRectangle(new SolidColorBrush(new Color(25, 255, 255, 255)),
                                new Pen(new SolidColorBrush(new Color(255, 255, 255, 255)), 1),
                                boxSelect.Normalize(), 0);
    }

    public void Hover() => boardState.Modules.ForEach(module => module.State.Hover = module.View.Contains(MouseCanvasPosition));
    public bool Locked => moving | boxSelecting;


}
using System.Reflection;
using Avalonia.Input;
using Avalonia.Media;
using CrystalCircuits.Core.Modules;
using DynamicData;
using MessagePack;

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
                moduleStartPosition = new();
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
            LayoutMove();
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
    private enum Direction
    {
        None,
        Up,
        Down,
        Left,
        Right
    }
    private void LayoutMove()
    {
        List<IModule> frozenModules = [];

        var moveDelta = MouseCanvasPosition - moveStartPosition;
        Selected.ForEach(module => module.View.Position = moduleStartPosition[module] + moveDelta);
        Selected.ForEach(frozenModules.Add);

        List<IModule> unFrozenModules = [.. boardState.Modules.Except(frozenModules)];
        unFrozenModules.ForEach(module => module.View.Position = moduleStartPosition[module]);

        Layout(frozenModules, unFrozenModules);
    }
    public void Untangle(List<IModule> modules)
    {
        List<IModule> frozenModules = [.. modules];
        List<IModule> unFrozenModules = [.. boardState.Modules.Except(frozenModules)];
        Layout(frozenModules, unFrozenModules);
    }
    private void Layout(List<IModule> frozenModules, List<IModule> unFrozenModules)
    {
        HashSet<IModule> overlappingModules = [];
        Direction LastMove = Direction.None;
        bool completed;
        int breakCount = 0;
        do
        {
            completed = true;
            overlappingModules = [];
            frozenModules.ForEach(frozenModule => unFrozenModules.ForEach(unFrozenModule =>
            {
                if (frozenModule.View.Rect.Intersects(unFrozenModule.View.Rect))
                {
                    overlappingModules.Add(unFrozenModule);
                    completed = false;
                }
            }));
            var index = 0;
            foreach (var overlappingModule in overlappingModules)
            {
                index = 0;
                LastMove = Direction.None;
                while (index < frozenModules.Count)
                {
                    var frozenModule = frozenModules.ElementAt(index);
                    if (overlappingModule.View.Rect.Intersects(frozenModule.View.Rect))
                    {
                        var distX = overlappingModule.View.Position.X - frozenModule.View.Position.X;
                        var distY = overlappingModule.View.Position.Y - frozenModule.View.Position.Y;
                        if (Math.Abs(distX) > Math.Abs(distY) && (Math.Sign(distX) >= 0 || LastMove == Direction.Right))
                        {
                            overlappingModule.View.Position = new(frozenModule.View.Rect.Center.X + (frozenModule.View.Size.Width / 2),
                                            overlappingModule.View.Position.Y);
                            LastMove = Direction.Right;
                            index = -1;
                        }
                        else if (Math.Abs(distX) > Math.Abs(distY) && (Math.Sign(distX) < 0 || LastMove == Direction.Left))
                        {
                            overlappingModule.View.Position = new(frozenModule.View.Position.X - overlappingModule.View.Size.Width,
                            overlappingModule.View.Position.Y);
                            LastMove = Direction.Left;
                            index = -1;
                        }
                        else if (Math.Abs(distX) <= Math.Abs(distY) && (Math.Sign(distY) >= 0 || LastMove == Direction.Down))
                        {
                            overlappingModule.View.Position = new(overlappingModule.View.Position.X,
                            frozenModule.View.Rect.Center.Y + (frozenModule.View.Size.Height / 2));
                            LastMove = Direction.Down;
                            index = -1;
                        }
                        else if (Math.Abs(distX) <= Math.Abs(distY) && (Math.Sign(distY) < 0 || LastMove == Direction.Up))
                        {
                            overlappingModule.View.Position = new(overlappingModule.View.Position.X,
                            frozenModule.View.Position.Y - overlappingModule.View.Size.Height);
                            LastMove = Direction.Up;
                            index = -1;
                        }
                    }
                    index++;
                    breakCount++;
                    if (breakCount > (boardState.Modules.Count * boardState.Modules.Count * 10))
                    {
                        goto Infinite;
                    }
                }
                frozenModules.Add(overlappingModule);
                unFrozenModules.Remove(overlappingModule);
            }
        } while (!completed);
    Infinite:
        return;
    }


    public List<IModule> clipBoard = [];

    public List<IModule> Clone(List<IModule> modules)
    {
        var Serializer = MessagePackSerializer.Typeless.Serialize(modules);
        clipBoard = (List<IModule>)MessagePackSerializer.Typeless.Deserialize(Serializer)!;
        clipBoard.ForEach(module => module.State.Clear());


        return clipBoard;
    }


    public void DrawSelectedOutline(DrawingContext context)
    {
        if (Selected.Count >= 2)
            Selected.ForEach(module =>
            {
                if (module.State.Selected)
                    context.DrawRectangle(Service.Instance.GetService<SettingService>()!.Theme.SelectedModules.Background,
                        Service.Instance.GetService<SettingService>()!.Theme.SelectedModules.Border,
                        module.View.Rect, 0);
            });
    }
    public void DrawBoxSelect(DrawingContext context)
    {
        if (boxSelecting)
            context.DrawRectangle(Service.Instance.GetService<SettingService>()!.Theme.BoxSelect.Background,
                                Service.Instance.GetService<SettingService>()!.Theme.BoxSelect.Border,
                                boxSelect.Normalize(), 0);
    }

    public void Hover() => boardState.Modules.ForEach(module => module.State.Hover = module.View.Contains(MouseCanvasPosition));
    public bool Locked => moving | boxSelecting;


}
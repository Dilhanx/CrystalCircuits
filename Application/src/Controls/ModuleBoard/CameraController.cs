using System.Numerics;
using Avalonia.Input;

namespace CrystalCircuits.Application.Controls.ModuleBoards;

class CameraController
{

    public Point MousePosition = new(0, 0);
    public Point Position { get; private set; } = new(0, 0);
    Point panMouseStart;
    Point panCanvasStart;
    Boolean panning = false;
    public void PanStart(Point position)
    {
        panning = true;
        panCanvasStart = Position;
        panMouseStart = position;
    }
    public void Panning(Point position)
    {
        if (panning)
        {
            Position = position - panMouseStart + panCanvasStart;
        }
    }
    public void PanEnd()
    {
        panning = false;
    }
    public Vector2 Zoom
    {
        get => field;
        private set
        {
            field = value;
            field.X = Math.Clamp(value.X, 0.1f, 10);
            field.Y = Math.Clamp(value.Y, 0.1f, 10);
        }
    } = new(1, 1);
    public void ZoomCamera(float delta)
    {
        Zoom += new Vector2(delta, delta) * 0.1f;
    }
}
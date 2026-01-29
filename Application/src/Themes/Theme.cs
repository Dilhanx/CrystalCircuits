using Avalonia.Animation;
using Avalonia.Media;
using Avalonia.Styling;

namespace CrystalCircuits.Application.Themes;

interface ITheme
{
    public ControlTheme Application { get; init; }
    public ControlTheme Window { get; init; }
    public ControlTheme TitleBar { get; init; }
    public CanvasTheme BoxSelect { get; init; }
    public CanvasTheme SelectedModules { get; init; }

}

class ControlTheme
{
    public List<Style> Styles { get; init; } = [];
    public Transitions Transitions { get; init; } = [];
    public IDictionary<object, object?> Resources { get; init; } = new Dictionary<object, object?>();
}

class CanvasTheme
{
    public IBrush Background { get; init; } = Brushes.Transparent;
    public IBrush Foreground { get; init; } = Brushes.Transparent;
    public Pen Border { get; init; } = new Pen();

}
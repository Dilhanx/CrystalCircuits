using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Media;
using Avalonia.Styling;

namespace CrystalCircuits.Application.Themes;

class TransparentTheme : ITheme
{
    public ControlTheme Application { get; init; } = new()
    {
        Styles = [
        new Style(x => Selectors.Is<TopLevel>(x))
        {
            Setters =
        {
            new Setter(TemplatedControl.FontFamilyProperty, new FontFamily("avares://Application/src/Assets/Fonts/EthnocentricRgIt#Ethnocentric")),
        }},
        new Style(x => x.OfType<MenuItem>())
        {
            Setters =
        {
            new Setter(TemplatedControl.BackgroundProperty, Brushes.Transparent),
            new Setter(TemplatedControl.ForegroundProperty, Brushes.White),
            new Setter(Animatable.TransitionsProperty, new Transitions
            {
                new BrushTransition
                {
                    Property = TemplatedControl.BackgroundProperty,
                    Duration = TimeSpan.FromSeconds(0.25),
                    Easing = new CubicEaseInOut()
                }
            })
        }},
        new Style(x => x.OfType<Button>())
        {
            Setters =
        {
            new Setter(TemplatedControl.BackgroundProperty, Brushes.Transparent),
            new Setter(TemplatedControl.ForegroundProperty, Brushes.White),
            new Setter(Animatable.TransitionsProperty, new Transitions
            {
                new BrushTransition
                {
                    Property = TemplatedControl.BackgroundProperty,
                    Duration = TimeSpan.FromSeconds(0.25),
                    Easing = new CubicEaseInOut()
                }
            })
        }},
        ],
        Resources = new Dictionary<object, object?>
        {
            { "ButtonBackgroundPointerOver",new SolidColorBrush(new Color(50,255,255,255)) },
            { "ButtonBackgroundPressed", new SolidColorBrush(new Color(20,0,0,0)) },
            { "ButtonForegroundPointerOver",Brushes.White },
            { "ButtonForegroundPressed", Brushes.White },
            { "MenuFlyoutItemBackgroundPointerOver",new SolidColorBrush(new Color(50,255,255,255)) },
            { "MenuFlyoutItemBackgroundPressed", new SolidColorBrush(new Color(20,0,0,0)) },
            { "MenuFlyoutItemForegroundPointerOver",Brushes.White },
            { "MenuFlyoutItemForegroundPressed", Brushes.White },
            { "MenuFlyoutScrollerMargin", new Thickness(0) },
            { "MenuFlyoutPresenterBackground",new SolidColorBrush(new Color(20,0,0,0)) },
            { "MenuFlyoutPresenterBorderThemeThickness",new Thickness(0) },
            { "OverlayCornerRadius",new CornerRadius(0) },
        }
    };
    public ControlTheme Window { get; init; } = new()
    {
        Styles = [
        new Style(x => x.OfType<Window>())
        {
            Setters =
        {
            new Setter(TemplatedControl.BackgroundProperty, Brushes.Transparent),
            new Setter(TemplatedControl.ForegroundProperty, Brushes.White),
        }},
        new Style(x => x.OfType<DockPanel>())
        {
            Setters =
        {
            new Setter(TemplatedControl.BackgroundProperty, new SolidColorBrush(new Color(10,0,0,0))),
            new Setter(TemplatedControl.ForegroundProperty, Brushes.White),
            new Setter(Animatable.TransitionsProperty, new Transitions
            {
                new BrushTransition
                {
                    Property = TemplatedControl.BackgroundProperty,
                    Duration = TimeSpan.FromSeconds(0.25),
                    Easing = new CubicEaseInOut()
                }
            })
        }},
        new Style(x => x.OfType<DockPanel>().Class(":pointerover"))
        {
            Setters =
        {
            new Setter(TemplatedControl.BackgroundProperty, new SolidColorBrush(new Color(20,0,0,0))),
            new Setter(TemplatedControl.ForegroundProperty, Brushes.White),
        }},
        ],


    };
    public ControlTheme TitleBar { get; init; } = new()
    {
        Styles = [
        new Style(x => x.OfType<TopBar>().Child().OfType<Grid>())
        {
            Setters =
        {
            new Setter(TemplatedControl.BackgroundProperty, new SolidColorBrush(new Color(20,0,0,0))),
            new Setter(TemplatedControl.ForegroundProperty, Brushes.White),
            new Setter(Animatable.TransitionsProperty, new Transitions
            {
                new BrushTransition
                {
                    Property = TemplatedControl.BackgroundProperty,
                    Duration = TimeSpan.FromSeconds(0.25),
                    Easing = new CubicEaseInOut()
                }
            }),

        }},
        new Style(x => x.OfType<TopBar>().Child().OfType<Grid>().Class(":pointerover"))
        {
            Setters =
        {
            new Setter(TemplatedControl.BackgroundProperty, new SolidColorBrush(new Color(15,0,0,0))),
        }},
        ],
    };
    public CanvasTheme BoxSelect { get; init; } = new()
    {
        Background = new SolidColorBrush(new Color(10, 0, 0, 0)),
        Border = new Pen(new SolidColorBrush(new Color(255, 255, 255, 255)), 1)
    };
    public CanvasTheme SelectedModules { get; init; } = new()
    {
        Border = new Pen(new SolidColorBrush(new Color(255, 255, 255, 255)), 2)
    };
}


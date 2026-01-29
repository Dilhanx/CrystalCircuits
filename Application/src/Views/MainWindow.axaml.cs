using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.VisualTree;
namespace CrystalCircuits.Application;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ExtendClientAreaToDecorationsHint = true;
        ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.NoChrome;
        Styles.AddRange(Service.Instance.GetService<SettingService>()!.Theme!.Window.Styles);
        Service.Instance.GetService<SettingService>()!.Theme!.Window.Resources.ToList().ForEach(Resources.Add);

        Avalonia.Application.Current!.Styles.AddRange(Service.Instance.GetService<SettingService>()!.Theme!.Application.Styles);
        Service.Instance.GetService<SettingService>()!.Theme!.Application.Resources.ToList().ForEach(Avalonia.Application.Current!.Resources.Add);

        Dock.IsHitTestVisible = true;
    }
}
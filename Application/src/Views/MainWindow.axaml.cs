using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
namespace CrystalCircuits.Application;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Background = Brushes.Transparent;
        ExtendClientAreaToDecorationsHint = true;
        // SystemDecorations = SystemDecorations.BorderOnly;
        // ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.NoChrome;
    }

}
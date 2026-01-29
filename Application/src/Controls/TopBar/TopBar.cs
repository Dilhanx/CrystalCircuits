using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Layout;
using ReactiveUI;
using Avalonia.Input;
using Avalonia.Platform;
using Avalonia.Media.Imaging;
using System.Reflection;
using Avalonia.Styling;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Presenters;

namespace CrystalCircuits.Application.Controls;

public class TopBar : UserControl
{
    Label Title;
    public TopBar()
    {
        Styles.AddRange(Service.Instance.GetService<SettingService>()!.Theme!.TitleBar.Styles);
        Service.Instance.GetService<SettingService>()!.Theme!.TitleBar.Resources.ToList().ForEach(Resources.Add);


        Grid grid = new()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
        grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
        grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

        Menu menu = new();

        MenuItem File = new()
        {
            Header = "File",
        };
        File.Items.Add(new MenuItem()
        {
            Header = "New",
            Command = ReactiveCommand.Create(() =>
            {
                Service.Instance.GetService<ProjectService>()!.New();
            }),
        });
        File.Items.Add(new MenuItem()
        {
            Header = "Open",
            Command = ReactiveCommand.Create(async () =>
            {
                await Service.Instance.GetService<ProjectService>()!.LoadAsync(TopLevel.GetTopLevel(this)!);
            })
        });
        File.Items.Add(new MenuItem()
        {
            Header = "Save",
            Command = ReactiveCommand.Create(async () =>
            {
                await Service.Instance.GetService<ProjectService>()!.SaveAsync(TopLevel.GetTopLevel(this)!);
            })
        });
        File.Items.Add(new MenuItem()
        {
            Header = "Save As",
            Command = ReactiveCommand.Create(async () =>
            {
                await Service.Instance.GetService<ProjectService>()!.SaveAsAsync(TopLevel.GetTopLevel(this)!);
            })
        });
        File.Items.Add(new MenuItem()
        {
            Header = "Exit",
            Command = ReactiveCommand.Create(async () =>
            {
                if (Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
                {
                    desktopLifetime.Shutdown();
                }
            })
        });

        MenuItem Edit = new()
        {
            Header = "Edit",
        };

        menu.Items.Add(File);
        menu.Items.Add(Edit);
        Loaded += OnLoaded;

        Grid.SetRow(menu, 0);
        Grid.SetColumn(menu, 0);
        grid.Children.Add(menu);

        Title = new()
        {
            Content = "",
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            IsHitTestVisible = false
        };
        Grid.SetRow(Title, 0);
        Grid.SetColumn(Title, 1);
        grid.Children.Add(Title);

        StackPanel stackPanel = new()
        {
            Orientation = Orientation.Horizontal,
        };
        stackPanel.Children.Add(new Button()
        {
            Content = "—",
            Command = ReactiveCommand.Create(() =>
            {
                (VisualRoot as Window)!.WindowState = WindowState.Minimized;
            })
        });
        stackPanel.Children.Add(new Button()
        {
            Content = "0",
            Command = ReactiveCommand.Create(() =>
            {
                switch ((VisualRoot as Window)!.WindowState)
                {
                    case WindowState.Maximized:
                        (VisualRoot as Window)!.WindowState = WindowState.Normal;
                        break;
                    case WindowState.Normal:
                        (VisualRoot as Window)!.WindowState = WindowState.Maximized;
                        break;
                }
            })
        });
        stackPanel.Children.Add(new Button()
        {
            Content = "X",
            Command = ReactiveCommand.Create(() =>
            {
                if (Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
                {
                    desktopLifetime.Shutdown();
                }
            })
        });
        Grid.SetRow(stackPanel, 0);
        Grid.SetColumn(stackPanel, 2);
        grid.Children.Add(stackPanel);

        grid.PointerPressed += OnPressed;
        grid.PointerReleased += OnReleased;
        Content = grid;
    }

    private void OnReleased(object? sender, PointerReleasedEventArgs e)
    {
    }

    private void OnPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.Source is Control sourceControl && sourceControl is Grid)
        {
            if (e.Properties.IsLeftButtonPressed && e.ClickCount == 2)
            {
                switch ((VisualRoot as Window)!.WindowState)
                {
                    case WindowState.Maximized:
                        (VisualRoot as Window)!.WindowState = WindowState.Normal;
                        break;
                    case WindowState.Normal:
                        (VisualRoot as Window)!.WindowState = WindowState.Maximized;
                        break;
                }
            }
            else
            {
                (VisualRoot as Window)!.BeginMoveDrag(e);
            }
        }
        e.Handled = false;
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        var window = VisualRoot as Window;
        window.WhenAnyValue(window => window.Title)
            .Subscribe(title =>
            {
                Title.Content = title;
            });
        window.WhenAnyValue(window => window.WindowState)
        .Subscribe(windowState =>
        {
            switch ((VisualRoot as Window)!.WindowState)
            {
                case WindowState.Maximized:
                    Padding = new(6, 6, 0, 6);
                    break;
                case WindowState.Normal:
                    Padding = new(0, 0, 0, 0);
                    break;
            }
        });
    }

    public sealed override void Render(DrawingContext context)
    {
        base.Render(context);
    }

}
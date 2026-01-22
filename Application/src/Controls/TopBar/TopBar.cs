using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using ReactiveUI;

namespace CrystalCircuits.Application.Controls;

public class TopBar : UserControl
{
    public TopBar()
    {

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
            })
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
            Header = "Edit"
        };

        menu.Items.Add(File);
        menu.Items.Add(Edit);
        Content = menu;
    }

    public sealed override void Render(DrawingContext context)
    {
        base.Render(context);
        context.DrawRectangle(new SolidColorBrush(new Color(50, 0, 0, 0)), new Pen(0), new Rect(Bounds.Size), 0);

    }

}
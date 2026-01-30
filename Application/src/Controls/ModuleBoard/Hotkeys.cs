using Avalonia.Controls;
using Avalonia.Input;
using ReactiveUI;

namespace CrystalCircuits.Application.Controls.ModuleBoards;

static class Hotkeys
{
    public static void Add(ModuleBoard moduleBoard, List<Avalonia.Input.KeyBinding> keyBindings, BoardState boardState, Selection selection)
    {
        keyBindings.Add(new KeyBinding
        {
            Gesture = new KeyGesture(Key.Z, KeyModifiers.Control),
            Command = ReactiveCommand.Create(() =>
                {
                    if (!selection.Locked)
                    {
                        Service.Instance.GetService<CommandService>()!.Undo();
                    }
                })
        });
        keyBindings.Add(new KeyBinding
        {
            Gesture = new KeyGesture(Key.Z, KeyModifiers.Control | KeyModifiers.Shift),
            Command = ReactiveCommand.Create(() =>
                {
                    if (!selection.Locked)
                    {
                        Service.Instance.GetService<CommandService>()!.Redo();
                    }
                })
        });
        keyBindings.Add(new KeyBinding
        {
            Gesture = new KeyGesture(Key.A, KeyModifiers.Control),
            Command = ReactiveCommand.Create(() =>
                {
                    selection.SelectAll();
                })
        });

        keyBindings.Add(new KeyBinding
        {
            Gesture = new KeyGesture(Key.A, KeyModifiers.Control | KeyModifiers.Shift),
            Command = ReactiveCommand.Create(() =>
                {
                    selection.DeselectAll();
                })
        });
        keyBindings.Add(new KeyBinding
        {
            Gesture = new KeyGesture(Key.Delete),
            Command = ReactiveCommand.Create(() =>
                {
                    if (!selection.Locked)
                    {
                        Service.Instance.GetService<CommandService>()!.Do(new RemoveModuleCommand(boardState, selection.Selected, selection));
                    }
                })
        });

        keyBindings.Add(new KeyBinding
        {
            Gesture = new KeyGesture(Key.N, KeyModifiers.Control),
            Command = ReactiveCommand.Create(() =>
                {
                    if (!selection.Locked)
                    {
                        Service.Instance.GetService<ProjectService>()!.New();
                    }
                })
        });
        keyBindings.Add(new KeyBinding
        {
            Gesture = new KeyGesture(Key.O, KeyModifiers.Control),
            Command = ReactiveCommand.Create(async () =>
                {
                    if (!selection.Locked)
                    {
                        await Service.Instance.GetService<ProjectService>()!.LoadAsync(TopLevel.GetTopLevel(moduleBoard)!);
                    }
                })
        });
        keyBindings.Add(new KeyBinding
        {
            Gesture = new KeyGesture(Key.S, KeyModifiers.Control),
            Command = ReactiveCommand.Create(async () =>
                {
                    if (!selection.Locked)
                    {
                        await Service.Instance.GetService<ProjectService>()!.SaveAsync(TopLevel.GetTopLevel(moduleBoard)!);
                    }
                })
        });
        keyBindings.Add(new KeyBinding
        {
            Gesture = new KeyGesture(Key.S, KeyModifiers.Control | KeyModifiers.Shift),
            Command = ReactiveCommand.Create(async () =>
                {
                    if (!selection.Locked)
                    {
                        await Service.Instance.GetService<ProjectService>()!.SaveAsAsync(TopLevel.GetTopLevel(moduleBoard)!);
                    }
                })
        });

        keyBindings.Add(new KeyBinding
        {
            Gesture = new KeyGesture(Key.C, KeyModifiers.Control),
            Command = ReactiveCommand.Create(async () =>
                {
                    if (!selection.Locked)
                    {
                        Service.Instance.GetService<CommandService>()!.Do(new CopyModuleCommand(selection));
                    }
                })
        });
        keyBindings.Add(new KeyBinding
        {
            Gesture = new KeyGesture(Key.X, KeyModifiers.Control),
            Command = ReactiveCommand.Create(async () =>
                {
                    if (!selection.Locked)
                    {
                        Service.Instance.GetService<CommandService>()!.Do(new CutModuleCommand(boardState, selection));
                    }
                })
        });
        keyBindings.Add(new KeyBinding
        {
            Gesture = new KeyGesture(Key.V, KeyModifiers.Control),
            Command = ReactiveCommand.Create(async () =>
                {
                    if (!selection.Locked)
                    {
                        Service.Instance.GetService<CommandService>()!.Do(new PasteModuleCommand(boardState, selection));
                    }
                })
        });
    }

}
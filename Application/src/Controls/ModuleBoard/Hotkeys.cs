using Avalonia.Input;
using ReactiveUI;

namespace CrystalCircuits.Application.Controls.ModuleBoards;

static class Hotkeys
{
    public static void Add(List<Avalonia.Input.KeyBinding> keyBindings, BoardState boardState, Selection selection)
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
                        Service.Instance.GetService<CommandService>()!.Do(new DeleteModuleCommand(boardState, selection.Selected));
                        selection.DeselectAll();
                    }
                })
        });
    }

}
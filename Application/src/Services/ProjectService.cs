
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CrystalCircuits.Application.Controls.ModuleBoards;

namespace CrystalCircuits.Application.Services;

class ProjectService
{
    public IStorageFile? File { get; set; }
    public BoardState boardState { get; init; } = new();
    public void New() => boardState.New();
    public async Task SaveAsync(TopLevel topLevel)
    {
        if (File is null)
            await SaveAsAsync(topLevel);
        else
            boardState.Save(File);
    }

    public async Task SaveAsAsync(TopLevel topLevel)
    {
        File = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save File",
            FileTypeChoices = [new FilePickerFileType("Custom Data Files"){
                Patterns =  ["*.ccd"],
            }]
        });

        if (File is not null)
        {
            if (topLevel is Window window)
            {
                window.Title = $"Crystal Circuits {File.Name}"; // Change the window title
            }
            boardState.Save(File);
        }
    }

    public async Task LoadAsync(TopLevel topLevel)
    {
        var tempFile = (await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Project",
            AllowMultiple = false,
            FileTypeFilter = [new FilePickerFileType("Custom Data Files"){
                Patterns =  ["*.ccd"],
            }]
        }))[0];
        if (tempFile is not null)
        {
            File = tempFile;
            if (topLevel is Window window)
            {
                window.Title = $"Crystal Circuits {File.Name}"; // Change the window title
            }
            Service.Instance.GetService<CommandService>()!.Clear();
            boardState.Load(File);
        }
    }
}
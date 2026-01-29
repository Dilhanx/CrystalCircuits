
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CrystalCircuits.Application.Controls.ModuleBoards;
using MessagePack;


namespace CrystalCircuits.Application.Services;

[MessagePackObject(AllowPrivate = true)]
partial class SettingService
{
    [IgnoreMember]
    public ITheme Theme { get; private set; }

    [IgnoreMember]
    private string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\CrystalCircuits";
    [IgnoreMember]
    private string settingFilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\CrystalCircuits\\setting.config";
    public SettingService()
    {
        Theme = new TransparentTheme();
        New();
    }
    private void New()
    {
        Theme = new TransparentTheme();
    }
    public void Save()
    {
        Directory.CreateDirectory(folderPath);
        File.WriteAllBytes(settingFilePath, MessagePackSerializer.Typeless.Serialize(this));

    }
    public void Load()
    {
        byte[] bytes = File.ReadAllBytes(settingFilePath);
        SettingService tempState = (SettingService)MessagePackSerializer.Typeless.Deserialize(bytes)!;
        Theme = tempState.Theme;
    }
}
using GameBridge.Data.EngineData;
using GameBridge.Ui;
using GameBridge.Ui.Factory.UiFabrication.Attributes;

namespace GameBridge.Data;

public class UserData
{
	[Path(PathType.DirectoryPath)]
	public string GameBridgeSaveDirectory { get; set; }
	public UnityEngineSettings UnitySettings { get; set; } = new UnityEngineSettings();
	public UnrealEngineSettings UnrealSettings { get; set; } = new UnrealEngineSettings();
}
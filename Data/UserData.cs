using GameBridge.Data.EngineData;
using GameBridge.Ui.Attributes;

namespace GameBridge.Data;

public class UserData
{
	[InputField]
	public string Username { get; set; }
	public UnityEngineSettings UnitySettings { get; set; } = new UnityEngineSettings();
	public UnrealEngineSettings UnrealSettings { get; set; } = new UnrealEngineSettings();
}
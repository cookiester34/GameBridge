using GameBridge.Ui;
using GameBridge.Ui.Factory.UiFabrication.Attributes;

namespace GameBridge.Data.EngineData;

public class UnrealEngineProject : IEngineProject
{
	[InputField]
	public string ProjectName { get; set; }
	[Path(PathType.DirectoryPath)]
	public string ProjectDirectory { get; set; }
	[InputField]
	public string ProjectVersion { get; set; }

	public void LoadProject()
	{
		// TODO: Load Project
	}
}
using GameBridge.Ui;
using GameBridge.Ui.Factory.UiFabrication.Attributes;
using System.Collections.Generic;

namespace GameBridge.Data.EngineData;

public class UnrealEngineSettings : IEngineSettings<UnrealEngineProject>
{
	public List<EngineInstall> EngineInstallPaths { get; set; } = new List<EngineInstall>();
	[Path(PathType.DirectoryPath)]
	public List<string> ProjectDirectories { get; set; } = new List<string>();
	public List<UnrealEngineProject> Projects { get; set; } = new List<UnrealEngineProject>();
	
	public List<IEngineProject> GetProjects()
	{
		return new List<IEngineProject>();
	}
}
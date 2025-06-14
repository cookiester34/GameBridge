using GameBridge.Ui;
using GameBridge.Ui.Factory.UiFabrication.Attributes;
using GameBridge.Ui.Factory.UiFabrication.DataBinder;
using System.Collections.Generic;

namespace GameBridge.Data.EngineData;

public class UnrealEngineSettings : IEngineSettings<UnrealEngineProject>
{
	[Path(PathType.DirectoryPath)]
	public List<string> InstallScanPaths { get; set; } = new List<string>();
	public List<EngineInstall> IndividualEngineInstallPaths { get; set; } = new List<EngineInstall>();
	[Path(PathType.DirectoryPath)]
	public List<string> ProjectScanPaths { get; set; } = new List<string>();
	public List<UnrealEngineProject> IndividualProjectPaths { get; set; } = new List<UnrealEngineProject>();
	public bool IsEnabled { get; set; } = false;
	
	public List<IEngineProject> GetProjects()
	{
		return new List<IEngineProject>();
	}

	public List<EngineInstall> GetEngineInstallPaths()
	{
		return new List<EngineInstall>();
	}
}
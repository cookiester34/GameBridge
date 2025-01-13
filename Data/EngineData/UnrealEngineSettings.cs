using GameBridge.Ui;
using GameBridge.Ui.Attributes;
using System.Collections.Generic;

namespace GameBridge.Data.EngineData;

public class UnrealEngineSettings : IEngineSettings
{
	[Path(PathType.FilePath)]
	public List<string> EngineInstallPaths { get; set; } = new List<string>();
	[Path(PathType.DirectoryPath)]
	public List<IEngineProject> Projects { get; set; } = new List<IEngineProject>();
}
using GameBridge.Ui;
using GameBridge.Ui.Attributes;
using System.Collections.Generic;

namespace GameBridge.Data.EngineData;

public class UnityEngineSettings : IEngineSettings
{
	[Path(PathType.FilePath)]
	public List<string> EngineInstallPaths { get; set; } = new List<string>()
	{
		"Test",
		"Test2",
		"Test3",
		"Test4",

		"Test5",
		"Test6",
	};
	[Path(PathType.DirectoryPath)]
	public List<IEngineProject> Projects { get; set; } = new List<IEngineProject>();
}
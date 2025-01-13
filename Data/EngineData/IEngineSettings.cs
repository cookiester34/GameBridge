using System.Collections.Generic;

namespace GameBridge.Data.EngineData;

public interface IEngineSettings
{
	public List<string> EngineInstallPaths { get; set; }
	public List<IEngineProject> Projects { get; set; }
}
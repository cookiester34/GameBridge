using System.Collections.Generic;

namespace GameBridge.Data.EngineData;

public interface IEngineSettings<T> where T : IEngineProject
{
	public List<string> InstallScanPaths { get; set; }
	public List<EngineInstall> IndividualEngineInstallPaths { get; set; }
	public List<string> ProjectScanPaths { get; set; }
	public List<T> IndividualProjectPaths { get; set; }
	public bool IsEnabled { get; set; }
	public List<IEngineProject> GetProjects();
	public List<EngineInstall> GetEngineInstallPaths();
}
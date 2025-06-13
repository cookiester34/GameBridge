using System.Collections.Generic;

namespace GameBridge.Data.EngineData;

public interface IEngineSettings<T> where T : IEngineProject
{
	public List<string> EngineInstallDirectories { get; set; }
	public List<EngineInstall> EngineInstallPaths { get; set; }
	public List<string> ProjectDirectories { get; set; }
	public List<T> Projects { get; set; }
	public List<IEngineProject> GetProjects();
	public List<EngineInstall> GetEngineInstallPaths();
}
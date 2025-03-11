using GameBridge.Ui;
using GameBridge.Ui.Factory.UiFabrication.Attributes;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameBridge.Data.EngineData;

public class UnityEngineSettings : IEngineSettings<UnityEngineProject>
{
	private const string PROJECT_VERSION_TXT_NAME = "ProjectSettings/ProjectVersion.txt";
	
	public List<EngineInstall> EngineInstallPaths { get; set; } = new List<EngineInstall>();
	[Path(PathType.DirectoryPath)]
	public List<string> ProjectDirectories { get; set; } = new List<string>();
	public List<UnityEngineProject> Projects { get; set; } = new List<UnityEngineProject>();
	
	public List<IEngineProject> GetProjects()
	{
		Projects.Clear();
		
		foreach (var projectDirectory in ProjectDirectories)
		{
			if (!Directory.Exists(projectDirectory))
			{
				//TODO: flag as incorrect
				continue;
			}
			
			var directories = Directory.GetDirectories(projectDirectory);
			foreach (var directory in directories)
			{
				var projectVerionTxt = Path.Join(directory, PROJECT_VERSION_TXT_NAME);
				if (File.Exists(projectVerionTxt))
				{
					var projectName = directory.Split(Path.DirectorySeparatorChar)[^1];
					
					var projectExists = Projects.Any(project => project.ProjectName == projectName);
					if (projectExists) continue;

					var lines = File.ReadAllLines(projectVerionTxt);
					var version = "";
					if (lines.Length > 0)
					{
						version = lines[0].Split(":")[^1].Trim();
					}
					
					Projects.Add(new UnityEngineProject
					{
						ProjectName = projectName,
						ProjectDirectory = directory,
						ProjectVersion = version
					});
				}
			}
		}

		return [..Projects]; //fancy
	}
}
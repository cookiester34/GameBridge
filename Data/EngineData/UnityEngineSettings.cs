using GameBridge.Ui;
using GameBridge.Ui.Factory.UiFabrication.Attributes;
using GameBridge.Ui.Factory.UiFabrication.DataBinder;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameBridge.Data.EngineData;

public class UnityEngineSettings : IEngineSettings<UnityEngineProject>
{
	private const string PROJECT_VERSION_TXT_NAME = "ProjectSettings/ProjectVersion.txt";
	
	[Path(PathType.DirectoryPath)]
	//TODO: Get value changed working with collections [OnValueChanged(nameof(GetEngineInstallPaths))]
	public List<string> InstallScanPaths { get; set; } = new List<string>();
	public List<EngineInstall> IndividualEngineInstallPaths { get; set; } = new List<EngineInstall>();
	[Path(PathType.DirectoryPath)]
	//TODO: Get value changed working with collections [OnValueChanged(nameof(GetProjects))]
	public List<string> ProjectScanPaths { get; set; } = new List<string>();
	public List<UnityEngineProject> IndividualProjectPaths { get; set; } = new List<UnityEngineProject>();
	public bool IsEnabled { get; set; } = false;
	
	public List<IEngineProject> GetProjects()
	{
		IndividualProjectPaths.Clear();
		
		foreach (var projectDirectory in ProjectScanPaths)
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
					
					var projectExists = IndividualProjectPaths.Any(project => project.ProjectName == projectName);
					if (projectExists) continue;

					var lines = File.ReadAllLines(projectVerionTxt);
					var version = "";
					if (lines.Length > 0)
					{
						version = lines[0].Split(":")[^1].Trim();
					}
					
					IndividualProjectPaths.Add(new UnityEngineProject
					{
						ProjectName = projectName,
						ProjectDirectory = directory,
						ProjectVersion = version
					});
				}
			}
		}

		return [..IndividualProjectPaths];
	}

	public List<EngineInstall> GetEngineInstallPaths()
	{
		IndividualEngineInstallPaths.Clear();

		foreach (var rootDir in InstallScanPaths)
		{
			if (!Directory.Exists(rootDir))
				continue;

			var versionDirs = Directory.GetDirectories(rootDir);

			foreach (var versionDir in versionDirs)
			{
				var unityExePath = Path.Combine(versionDir, "Editor", "Unity.exe");

				if (!File.Exists(unityExePath)) continue;
				
				var version = Path.GetFileName(versionDir);

				var install = new EngineInstall
				{
					Version = version,
					InstallPath = unityExePath
				};

				IndividualEngineInstallPaths.Add(install);
			}
		}

		return [..IndividualEngineInstallPaths];
	}
}
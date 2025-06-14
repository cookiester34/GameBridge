using GameBridge.Ui;
using GameBridge.Ui.Factory.UiFabrication.Attributes;
using GameBridge.Ui.Factory.UiFabrication.DataBinder;
using System;
using System.Diagnostics;
using System.IO;

namespace GameBridge.Data.EngineData;

public class UnityEngineProject : IEngineProject
{
	[InputField]
	public string ProjectName { get; set; }
	
	[Path(PathType.DirectoryPath)]
	[OnValueChanged(nameof(HandleProjectPathSelected))]
	public string ProjectDirectory { get; set; }

	[InputField]
	public string ProjectVersion { get; set; }

	public void LoadProject()
	{
		if (string.IsNullOrWhiteSpace(ProjectDirectory) || !Directory.Exists(ProjectDirectory))
		{
			ShowMessage("Invalid project directory.");
			return;
		}

		string editorPath = "";

		foreach (var engineInstallPath in DataManager.UserData.UnitySettings.IndividualEngineInstallPaths)
		{
			if (engineInstallPath.Version != ProjectVersion) continue;
			ShowMessage($"Project version: {ProjectVersion} : engine install path: {engineInstallPath.Version}");
			editorPath = engineInstallPath.InstallPath;
			break;
		}

		if (!File.Exists(editorPath))
		{
			ShowMessage("Unity executable not found at: " + editorPath);
			return;
		}

		try
		{
			var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = editorPath,
					Arguments = $"-projectPath \"{ProjectDirectory}\"",
					UseShellExecute = false,
					CreateNoWindow = true
				}
			};

			process.Start();
			ShowMessage("Unity is launching the project...");
		}
		catch (Exception ex)
		{
			ShowMessage($"Error launching Unity: {ex.Message}");
		}
	}

	private void HandleProjectPathSelected(string newValue)
	{
		ProjectName = newValue;
	}
	
	private void ShowMessage(string message)
	{
		Console.WriteLine(message); // TEMP: for now just log to console
	}
}
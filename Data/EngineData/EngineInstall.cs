using GameBridge.Ui;
using GameBridge.Ui.Factory.UiFabrication.Attributes;
using GameBridge.Ui.Factory.UiFabrication.DataBinder;
using System.IO;

namespace GameBridge.Data.EngineData;

public class EngineInstall
{
	public string Version { get; set; }
	[Path(PathType.FilePath)]
	[OnValueChanged(nameof(HandleInstallPathChange))]
	public string InstallPath { get; set; }

	private void HandleInstallPathChange(string newPath)
	{
		if (string.IsNullOrWhiteSpace(newPath))
			return;

		try
		{
			// Get the folder that contains "Unity.exe"
			var editorFolder = Path.GetDirectoryName(newPath);

			// Go one level up (the folder that contains "Editor")
			var versionFolder = Path.GetDirectoryName(editorFolder);

			// Last folder name is the version string
			Version = Path.GetFileName(versionFolder);
		}
		catch
		{
			Version = "Unknown";
		}
	}
}
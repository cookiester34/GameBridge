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
		Version = newPath.Split(Path.DirectorySeparatorChar)[^1];
	}
}
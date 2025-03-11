using GameBridge.Ui;
using GameBridge.Ui.Factory.UiFabrication.Attributes;
using GameBridge.Ui.Factory.UiFabrication.DataBinder;
using GameBridge.Ui.Factory.UiFabrication.Decorators;

namespace GameBridge.Data.EngineData;

public class UnityEngineProject : IEngineProject
{
	[InputField]
	public string ProjectName { get; set; }
	
	[Path(PathType.DirectoryPath)]
	[OnValueChanged(nameof(HandleProjectPathSelected))]
	public string ProjectDirectory { get; set; }

	[InputField]
	[Button(nameof(LoadProject))]
	public string ProjectVersion { get; set; }

	public void LoadProject()
	{
		// TODO: Implement
	}

	private void HandleProjectPathSelected(string newValue)
	{
		ProjectName = newValue;
	}
}
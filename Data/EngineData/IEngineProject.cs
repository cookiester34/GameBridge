using GameBridge.Ui.Attributes;

namespace GameBridge.Data.EngineData;

public interface IEngineProject
{
	[InputField]
	public string ProjectName { get; set; }

	[InputField]
	public string ProjectPath { get; set; }

	[InputField]
	public string ProjectVersion { get; set; }

	public void LoadProject();
}
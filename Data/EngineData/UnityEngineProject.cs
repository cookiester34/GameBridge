namespace GameBridge.Data.EngineData;

public class UnityEngineProject : IEngineProject
{
	public string ProjectName { get; set; }
	public string ProjectPath { get; set; }
	public string ProjectVersion { get; set; }

	public void LoadProject()
	{
		// TODO: Implement
	}
}
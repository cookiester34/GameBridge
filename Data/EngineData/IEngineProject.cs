namespace GameBridge.Data.EngineData;

public interface IEngineProject
{
	public string ProjectName { get; set; }
	public string ProjectDirectory { get; set; }
	public string ProjectVersion { get; set; }

	public void LoadProject();
}
using System.IO;
using System.Text.Json;

namespace GameBridge.Data;

public static class DataManager
{
	private const string DataFileName = "data.json";
	private static UserData? userData;
	public static UserData UserData
	{
		get => userData ??= LoadData();
		private set => userData = value;
	}

	public static void SaveData()
	{
		var json = JsonSerializer.Serialize(UserData, new JsonSerializerOptions { WriteIndented = true });
		File.WriteAllText(DataFileName, json);
	}

	public static UserData LoadData()
	{
		if (!File.Exists(DataFileName)) return UserData = new UserData();
		var json = File.ReadAllText(DataFileName);
		return UserData = JsonSerializer.Deserialize<UserData>(json);

	}
}
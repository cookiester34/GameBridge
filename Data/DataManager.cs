using System;
using System.IO;
using System.Text.Json;

namespace GameBridge.Data;

public static class DataManager
{
	private const string DataFileName = "data.json";
	private static UserData? userData;

	private static string AppDir =>
		Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GameBridge");

	private static string DataPath => Path.Combine(AppDir, DataFileName);

	public static UserData UserData
	{
		get => userData ??= LoadData();
		private set => userData = value;
	}

	public static bool DoesSaveDataExist() => File.Exists(DataPath);

	public static void SaveData()
	{
		Directory.CreateDirectory(AppDir); // make sure folder exists
		var json = JsonSerializer.Serialize(UserData, new JsonSerializerOptions { WriteIndented = true });
		File.WriteAllText(DataPath, json);
	}

	public static UserData LoadData()
	{
		Directory.CreateDirectory(AppDir);

		if (!File.Exists(DataPath))
			return CreateNewUserData();

		try
		{
			var json = File.ReadAllText(DataPath);
			return UserData = JsonSerializer.Deserialize<UserData>(json) ?? CreateNewUserData();
		}
		catch
		{
			return CreateNewUserData();
		}
	}

	private static UserData CreateNewUserData()
	{
		UserData = new UserData();
		userData.GameBridgeSaveDirectory = AppDir;
		return userData;
	}

	public static string GetAppDirectory() => AppDir;
}
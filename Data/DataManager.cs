using System;
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

	//TODO: make this check that even if the file exists that there is data there
	public static bool DoesSaveDataExist() => File.Exists(DataFileName);

	public static void SaveData()
	{
		var json = JsonSerializer.Serialize(UserData, new JsonSerializerOptions { WriteIndented = true });
		File.WriteAllText(DataFileName, json);
	}

	public static UserData LoadData()
	{
		if (!File.Exists(DataFileName))
		{
			return CreateNewUserData();
		}

		try
		{
			var json = File.ReadAllText(DataFileName);
			return UserData = JsonSerializer.Deserialize<UserData>(json);
		}
		catch (Exception _)
		{
			return CreateNewUserData();
		}
	}

	private static UserData CreateNewUserData()
	{
		UserData = new UserData();
		userData.GameBridgeSaveDirectory = GetAppDirectory();
		return userData;
	}

	public static string GetAppDirectory()
	{
		var gameBridgeSaveDirectory = UserData.GameBridgeSaveDirectory;
		if (!string.IsNullOrWhiteSpace(gameBridgeSaveDirectory))
		{
			return Path.Combine(gameBridgeSaveDirectory, DataFileName);
		}
		return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DataFileName);
	}
}
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using GameBridge.Data;
using GameBridge.Ui;
using GameBridge.Ui.Factory;

namespace GameBridge.Pages;

public class SettingsPage : Page
{
	public SettingsPage()
	{
		var userData = DataManager.UserData;

		var title = new TextBlock
		{
			Text = "GameBridge Settings",
			FontWeight = FontWeight.Bold,
			FontSize = 18,
			Margin = new Thickness(5,0,0,0)
		};
		AddContent(title);

		AddContent(UiFactory.ProcessClass(userData));

		// var userInfoSection = new Section("User Info",
		// 	"Basic GameBridge settings");
		// AddContent(userInfoSection);
		//
		// var unitySettingsSection = new Section("Unity Engine Settings",
		// 	"Here are all the Unity Engine project settings");
		// foreach (var installPath in userData.UnitySettings.EngineInstallPaths)
		// {
		// 	unitySettingsSection.AddContent(new ExplorerField("Unity Installs Directory:", installPath));
		// }
		//
		// unitySettingsSection.AddContent(new ExplorerField("Unity Installs Directory:"));
		// AddContent(unitySettingsSection);
		//
		// var unrealSettingsSection = new Section("Unreal Engine Settings",
		// 	"Here are all the Unreal Engine project settings");
		// unrealSettingsSection.AddContent(new ExplorerField("Unreal Installs Directory:"));
		// AddContent(unrealSettingsSection);
	}
}
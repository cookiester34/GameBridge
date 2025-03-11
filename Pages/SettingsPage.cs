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
	}
}
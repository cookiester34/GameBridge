using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
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

		var innerPanel = new StackPanel
		{
			Orientation = Orientation.Vertical,
			Spacing = 10
		};

		innerPanel.Children.Add(new TextBlock
		{
			Text = "GameBridge Settings",
			FontWeight = FontWeight.Bold,
			FontSize = 18,
			Margin = new Thickness(5, 0, 0, 0)
		});

		var settingsUi = UiFactory.ProcessClass(userData);
		if (settingsUi != null)
			innerPanel.Children.Add(settingsUi);

		var paddedContainer = new Border
		{
			Padding = new Thickness(20), // ← Adds top/bottom/left/right padding
			Child = innerPanel
		};

		AddContent(paddedContainer);
	}
}
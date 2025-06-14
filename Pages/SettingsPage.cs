using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
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
			Margin = new Thickness(0, 0, 0, 0)
		});

		var settingsUi = UiFactory.ProcessClass(userData);
		if (settingsUi != null)
		{
			innerPanel.Children.Add(settingsUi);
		}

		var paddedContent = new Border
		{
			Padding = new Thickness(11, 0, 11, 2),
			Child = innerPanel
		};

		var scrollViewer = new ScrollViewer
		{
			Content = paddedContent,
			VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
			HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
			HorizontalAlignment = HorizontalAlignment.Stretch,
		};

		AddContent(scrollViewer);
	}
}
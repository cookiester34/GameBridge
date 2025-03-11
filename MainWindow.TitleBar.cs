using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using GameBridge.Data;

namespace GameBridge;

public partial class MainWindow
{
	private void BuildCustomTitleBar()
	{
		var titleBar = new DockPanel
		{
			Background = WindowColors.TitleBarColor,
			Height = 30
		};
		DockPanel.SetDock(titleBar, Dock.Top);
		AddContentToWindow(titleBar);

		titleBar.PointerPressed += (s, e) =>
		{
			// Start dragging
			if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
			{
				BeginMoveDrag(e);
			}
		};

		var titleText = new TextBlock
		{
			Text = "GameBridge",
			FontWeight = FontWeight.Bold,
			VerticalAlignment = VerticalAlignment.Center,
			Margin = new Thickness(10, 0)
		};
		DockPanel.SetDock(titleText, Dock.Left);
		titleBar.Children.Add(titleText);

		// Add minimize, maximize, and close buttons
		var buttonPanel = new StackPanel
		{
			Orientation = Orientation.Horizontal,
			HorizontalAlignment = HorizontalAlignment.Right
		};
		titleBar.Children.Add(buttonPanel);

		var minimizeButton = CreateTitleBarButton("—");
		minimizeButton.CornerRadius = new CornerRadius(0, 0, 0, 3);
		var maximizeButton = CreateTitleBarButton("☐");
		var closeButton = CreateTitleBarButton("X");

		buttonPanel.Children.Add(minimizeButton);
		buttonPanel.Children.Add(maximizeButton);
		buttonPanel.Children.Add(closeButton);

		minimizeButton.Click += (_, _) => WindowState = WindowState.Minimized;
		maximizeButton.Click += (_, _) =>
			WindowState = WindowState == WindowState.Maximized
				? WindowState.Normal
				: WindowState.Maximized;
		closeButton.Click += (_, _) =>
		{
			DataManager.SaveData();
			Close();
		};
	}

	private Button CreateTitleBarButton(string content)
	{
		var button = new Button
		{
			Content = new TextBlock
			{
				Text = content,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				FontSize = 16
			},
			Width = 30,
			Height = 30,
			CornerRadius = new CornerRadius(0),
			BorderThickness = new Thickness(0),
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center
		};

		button.PointerEntered += (s, e) =>
		{
			button.Background = WindowColors.UiHoverColor;
		};

		button.PointerExited += (s, e) =>
		{
			button.Background = WindowColors.UiColor;
		};

		return button;
	}
}
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform;
using GameBridge.Data;
using GameBridge.Data.EngineData;
using GameBridge.Pages;
using GameBridge.Ui;
using System;

namespace GameBridge;

public partial class MainWindow : ContentWindow
{
	public static event Action<Size> WindowSizeChanged;
	public MainWindow()
	{
		SetupMainWindow();

		BuildCustomTitleBar();

		CreateWindowContent();

		Initialize();

		this.LayoutUpdated += MainWindowLayoutUpdated;

		// Debug
		this.AttachDevTools();
	}

	private void MainWindowLayoutUpdated(object? sender, EventArgs e)
	{
		var size = Bounds.Size;
		WindowSizeChanged?.Invoke(size);
	}

	private void SetupMainWindow()
	{
		ExtendClientAreaToDecorationsHint = true;
		ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.NoChrome;

		SystemDecorations = SystemDecorations.BorderOnly;
		Background = WindowColors.BackgroundColor;

		CanResize = true;

		//TODO: Can save and load this
		Width = 700;
		Height = 500;
	}

	private void CreateWindowContent()
	{
		//TODO: Check if we have setup GameBridge, if so switch to main page

		if (DataManager.DoesSaveDataExist())
		{
			CreateMainUi();
		}
		else
		{
			CenterContent();
			CreateWelcomeUi();
		}
	}

	private void CreateMainUi()
	{
		var userData = DataManager.UserData;
		var pageNavigator = new PageNavigator();
		
		pageNavigator.AddPage("Unity", new EnginePage<UnityEngineProject>(userData.UnitySettings));
		pageNavigator.AddPage("Unreal", new EnginePage<UnrealEngineProject>(userData.UnrealSettings));
		
		AddContent(pageNavigator);
	}

	private void CreateWelcomeUi()
	{
		var welcomeContent = new StackPanel
		{
			Orientation = Orientation.Vertical,
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,
		};
		AddContent(welcomeContent);

		var welcomeButton = new Button
		{
			Content = "Welcome To GameBridge!",
			Background = Brushes.Transparent,
			FontSize = 20,
			HorizontalAlignment = HorizontalAlignment.Center
		};
		welcomeContent.AddChild(welcomeButton);

		var startSetupButton = new Button
		{
			Content = "Click Me To Start Setup!",
			Foreground = WindowColors.BackgroundTextColor,
			Background = Brushes.Transparent,
			HorizontalAlignment = HorizontalAlignment.Center
		};
		welcomeContent.AddChild(startSetupButton);

		var scrollView = new ScrollView()
		{
			IsVisible = false
		};
		scrollView.AddContent(new SettingsPage());

		var finishSetupbutton = new Button
		{
			Content = "Finish Setup"
		};
		finishSetupbutton.Click += (_, _) =>
		{
			RemoveContent(welcomeContent);
			RemoveContent(scrollView);
			CreateMainUi();
		};
		scrollView.AddContent(finishSetupbutton);
		AddContent(scrollView);

		welcomeButton.Click += (sender, e) =>
		{
			scrollView.Show();
			welcomeContent.Hide();
		};
		startSetupButton.Click += (sender, e) =>
		{
			scrollView.Show();
			welcomeContent.Hide();
		};
	}
}
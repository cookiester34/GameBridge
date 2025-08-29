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

		LayoutUpdated += MainWindowLayoutUpdated;

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
		Width = 1000;
		Height = 500;
		
		MinWidth = 800;
		MinHeight = 400;
	}

	private void CreateWindowContent()
	{
		//TODO: Check if we have set up GameBridge, if so switch to main page

		if (DataManager.DoesSaveDataExist())
		{
			CreateMainUi();
		}
		else
		{
			CreateWelcomeUi();
		}
	}

	private void CreateMainUi()
	{
		var userData = DataManager.UserData;
		var pageNavigator = new PageNavigator();
		
		pageNavigator.AddPage("Unity", new EnginePage<UnityEngineProject>(userData.UnitySettings));
		pageNavigator.AddPage("Unreal", new EnginePage<UnrealEngineProject>(userData.UnrealSettings));
		pageNavigator.AddPage("Settings", new SettingsPage(), false);
		
		//TODO: Save the last one and use that
		pageNavigator.SwitchPage("Unity");
		
		AddContentToWindow(pageNavigator);
	}

	private void CreateWelcomeUi()
	{
		var welcomeContent = new StackPanel
		{
			Orientation = Orientation.Vertical,
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,
		};

		var welcomeContainer = CenterContentInWindow(welcomeContent);

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

		welcomeButton.Click += (sender, e) =>
		{
			SwitchToSettingsPage();
		};
		startSetupButton.Click += (sender, e) =>
		{
			SwitchToSettingsPage();
		};

		void SwitchToSettingsPage()
		{
			RemoveContentToWindow(welcomeContainer);

			var introPage = new IntroPage();

			var layout = new Grid
			{
				RowDefinitions =
				{
					new RowDefinition(GridLength.Star),
					new RowDefinition(GridLength.Auto)
				},
				VerticalAlignment = VerticalAlignment.Stretch,
				HorizontalAlignment = HorizontalAlignment.Stretch
			};

			Grid.SetRow(introPage, 0);
			layout.Children.Add(introPage);

			var settingScrollViewContainer = CenterContentInWindow(layout, 0.1);
			
			introPage.OnSetupComplete += () =>
			{
				RemoveContentToWindow(settingScrollViewContainer);
				CreateMainUi();
			};
		}
	}
}
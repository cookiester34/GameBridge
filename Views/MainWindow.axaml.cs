using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using ProjectBridge.Code;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ProjectBridge.Views;

public partial class MainWindow : Window
{
	private readonly EngineDataPage engineDataPage;
	private readonly SettingsPage settingsPage;

	private bool sideMenuVisible;

	public MainWindow()
	{
		InitializeComponent();
		UpdateMainContentMargin(); // Set initial margin

		MainContent.Content = engineDataPage = new EngineDataPage(); // Set initial content
		MainContent.Margin = new Thickness(0, 40, 0, 0);

		SettingsContent.Content = settingsPage = new SettingsPage();
		SettingsContent.Margin = new Thickness(0, 40, 0, 0);

		ToggleActivePage(true);
	}

	private void OnPointerPressed(object sender, PointerPressedEventArgs e)
	{
		// Check if the left mouse button was pressed
		if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
		{
			// Start moving the window
			BeginMoveDrag(e);
		}
	}

	private void MinimizeWindow(object sender, RoutedEventArgs e)
	{
		WindowState = WindowState.Minimized; // Minimize the window
	}

	private void CloseWindow(object sender, RoutedEventArgs e)
	{
		Close(); // Close the window
	}

	private void SwitchEnginePage(object sender, RoutedEventArgs e)
	{
		if (sender is not Button button) return;

		var buttonContent = button.Content;
		switch (buttonContent)
		{
			case "Unity":
				UiDataManager.CurrentPage = CurrentPage.Unity;
				break;
			case "Unreal":
				UiDataManager.CurrentPage = CurrentPage.Unreal;
				break;
			case "Godot":
				UiDataManager.CurrentPage = CurrentPage.Godot;
				break;
		}

		engineDataPage.SetupEngineSpecificText();
		ToggleActivePage(true);
	}

	private void NavigateToSettings(object sender, RoutedEventArgs e)
	{
		ToggleActivePage(false);
	}

	private void ToggleActivePage(bool mainPageActive)
	{
		SettingsContent.IsVisible = !mainPageActive;
		MainContent.IsVisible = mainPageActive;
	}

	private void UpdateMainContentMargin()
	{
		// Update the margin based on the visibility of the side menu
		sideMenuVisible = SideMenu.IsVisible;
	}

	// Call this method to toggle visibility of the side menu
	private void ToggleSideMenuVisibility(object? sender, RoutedEventArgs routedEventArgs)
	{
		_ = AnimateMargin(); // Animate the margin change
	}

	private async Task AnimateMargin()
	{
		//TODO: This is currently tied to framerate, find a way to stop this
		sideMenuVisible = !sideMenuVisible;
		if (sideMenuVisible)
		{
			SideMenu.IsVisible = sideMenuVisible;
		}

		const int duration = 60; // Duration of the animation in milliseconds
		const int steps = 10; // Number of animation steps
		double startMargin = sideMenuVisible ? 0 : 110; // Starting margin
		double endMargin = sideMenuVisible ? 110 : 0; // Ending margin

		for (int i = 0; i <= steps; i++)
		{
			// Calculate the current margin value based on the progress
			double currentMargin = startMargin + (endMargin - startMargin) * (i / (double)steps);

			// Set the new margin for the MainContent
			MainContent.Margin = new Thickness(currentMargin, 40, 0, 0);
			SideMenu.Margin = new Thickness(currentMargin - 110, 46, 0, 0);

			// Delay to create smooth animation
			await Task.Delay(duration / steps);
		}

		// Toggle the visibility state after the animation
		SideMenu.IsVisible = sideMenuVisible;
	}
}
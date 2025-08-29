using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Themes.Simple;
using GameBridge.Data;
using GameBridge.CustomStyles;
using Avalonia.Themes.Fluent;
using Semi.Avalonia;
using System;
using Thickness = Avalonia.Thickness;

namespace GameBridge;

public class App : Application
{
	public override void Initialize()
	{
		// No XAML Loader, no global styles.
		Styles.AddRange(new Styles
		{
			// new SimpleTheme(),
			// new FluentTheme(),
			new SemiTheme()
		});


		// Styles.Add(new Style(x => x.OfType<Button>())
		// {
		// 	Setters =
		// 	{
		// 		new Setter(TemplatedControl.BackgroundProperty, WindowColors.UiColor),
		// 		new Setter(TemplatedControl.ForegroundProperty, WindowColors.TextColor),
		// 		new Setter(TemplatedControl.BorderBrushProperty, Brushes.Transparent),
		// 		new Setter(TemplatedControl.BorderThicknessProperty, new Thickness(0)),
		// 		new Setter(TemplatedControl.CornerRadiusProperty, new CornerRadius(3)),
		// 	}
		// });
		//
		// Styles.Add(new Style(x => x.OfType<TextBox>())
		// {
		// 	Setters =
		// 	{
		// 		new Setter(TemplatedControl.BackgroundProperty, WindowColors.BackgroundColor),
		// 		new Setter(TemplatedControl.ForegroundProperty, WindowColors.TextColor),
		// 		new Setter(TemplatedControl.BorderBrushProperty, Brushes.Transparent),
		// 		new Setter(TemplatedControl.BorderThicknessProperty, new Thickness(0)),
		// 		new Setter(TemplatedControl.CornerRadiusProperty, new CornerRadius(3)),
		// 	}
		// });
		//
		// Styles.Add(new Style(x => x.OfType<TextBlock>())
		// {
		// 	Setters =
		// 	{
		// 		new Setter(TemplatedControl.BackgroundProperty, Brushes.Transparent),
		// 		new Setter(TemplatedControl.ForegroundProperty, WindowColors.TextColor),
		// 		new Setter(TemplatedControl.BorderBrushProperty, Brushes.Transparent)
		// 	}
		// });
	}

	public override void OnFrameworkInitializationCompleted()
	{
		HoloTheme.Register(this);
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			desktop.MainWindow = new MainWindow(); // Define the main window programmatically.
		}
		base.OnFrameworkInitializationCompleted();
	}
}
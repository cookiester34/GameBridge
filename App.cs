using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Styling;
using GameBridge.Themes;

namespace GameBridge;

public class App : Application
{
    public override void Initialize()
    {
        // No XAML Loader, no global styles.
        Styles.AddRange(new Styles
        {
            new ModernTheme()
        });
    }

    public override void OnFrameworkInitializationCompleted()
	{
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			desktop.MainWindow = new MainWindow(); // Define the main window programmatically.
		}
		base.OnFrameworkInitializationCompleted();
	}
}
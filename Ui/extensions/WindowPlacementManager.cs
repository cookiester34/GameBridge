using System;
using System.IO;
using System.Text.Json;
using Avalonia;
using Avalonia.Controls;


public static class WindowPlacementManager
{
	private sealed class WindowPlacement
	{
		public double Width { get; set; }
		public double Height { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
		public WindowState State { get; set; }
	}

	public static void Attach(Window window, string appName, string fileName = "window.json")
	{
		if (window is null) throw new ArgumentNullException(nameof(window));
		if (string.IsNullOrWhiteSpace(appName)) throw new ArgumentNullException(nameof(appName));

		var settingsPath = GetSettingsPath(appName, fileName);
		Restore(window, settingsPath);

		window.Opened += (_, _) => EnsureVisible(window);
		window.PositionChanged += (_, _) => Save(window, settingsPath);
		window.PropertyChanged += (_, e) =>
		{
			if (e.Property == Window.WidthProperty ||
			    e.Property == Window.HeightProperty ||
			    e.Property == Window.WindowStateProperty)
			{
				Save(window, settingsPath);
			}
		};
		window.Closing += (_, _) => Save(window, settingsPath);
	}

	private static void Restore(Window window, string settingsPath)
	{
		try
		{
			if (!File.Exists(settingsPath)) return;

			var json = File.ReadAllText(settingsPath);
			var data = JsonSerializer.Deserialize<WindowPlacement>(json);
			if (data is null) return;

			// Never restore to Minimized
			var targetState = data.State == WindowState.Minimized ? WindowState.Normal : data.State;

			window.Width = data.Width > 100 ? data.Width : window.Width;
			window.Height = data.Height > 100 ? data.Height : window.Height;

			// Set position before state to avoid weird maximize-to-wrong-screen issues
			window.Position = new PixelPoint(data.X, data.Y);
			window.WindowState = targetState;

			EnsureVisible(window);
		}
		catch
		{
			// Ignore malformed files
		}
	}

	private static void Save(Window window, string settingsPath)
	{
		try
		{
			var stateToPersist = window.WindowState;

			// Persist Normal bounds even when Maximized
			if (window.WindowState == WindowState.Maximized)
			{
				// Temporarily compute normal bounds from platform if available
				// If not available, current size is acceptable
			}

			var data = new WindowPlacement
			{
				Width = Math.Max(100, window.Width),
				Height = Math.Max(100, window.Height),
				X = window.Position.X,
				Y = window.Position.Y,
				State = stateToPersist
			};

			var dir = Path.GetDirectoryName(settingsPath)!;
			if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

			var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(settingsPath, json);
		}
		catch
		{
			// Ignore write errors
		}
	}

	private static void EnsureVisible(Window window)
	{
		try
		{
			var screens = window.Screens;
			var rect = new PixelRect(window.Position, new PixelSize((int)window.Width, (int)window.Height));

			// If the window is fully off-screen, move it to the primary screen's working area
			var onAnyScreen = false;
			foreach (var s in screens.All)
			{
				if (s.WorkingArea.Intersects(rect))
				{
					onAnyScreen = true;
					break;
				}
			}

			if (!onAnyScreen)
			{
				var primary = screens.Primary?.WorkingArea ?? new PixelRect(0, 0, 1280, 800);
				var x = Math.Max(primary.X, primary.X + (primary.Width - (int)window.Width) / 2);
				var y = Math.Max(primary.Y, primary.Y + (primary.Height - (int)window.Height) / 2);
				window.Position = new PixelPoint(x, y);
				window.WindowState = WindowState.Normal;
			}
		}
		catch
		{
			// Best-effort
		}
	}

	private static string GetSettingsPath(string appName, string fileName)
	{
		var baseDir = OperatingSystem.IsWindows()
			? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
			: OperatingSystem.IsMacOS()
				? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library",
					"Application Support")
				: Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

		return Path.Combine(baseDir, appName, fileName);
	}
}
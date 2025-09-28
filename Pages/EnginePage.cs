using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using GameBridge.Data.EngineData;
using GameBridge.Ui;
using GameBridge.Ui.Factory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameBridge.Pages;

public class EnginePage<T> : Page where T : IEngineProject
{
	private readonly StackPanel list = new()
	{
		Orientation = Orientation.Vertical,
		Spacing = 8,
		Margin = new Thickness(10, 0, 10, 10),
		HorizontalAlignment = HorizontalAlignment.Stretch,
		VerticalAlignment = VerticalAlignment.Top
	};

	private readonly ScrollViewer scrollView;
	private readonly List<Border> sectionWrappers = new();

	private readonly IEngineSettings<T> engineSettings;

	public EnginePage(IEngineSettings<T> engineSettings)
	{
		this.engineSettings = engineSettings;

		scrollView = new ScrollViewer
		{
			VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
			HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
			Content = list,
			Margin = new Thickness(0, 5, 0, 0)
		};

		AddContent(scrollView);
		BuildList();
	}

	private void BuildList()
	{
		list.Children.Clear();
		sectionWrappers.Clear();

		var projects = engineSettings.GetProjects();
		var installs = engineSettings.GetEngineInstallPaths();

		for (var i = 0; i < projects.Count; i++)
		{
			var project = projects[i];

			BuildProjectUi(project, installs);
		}
	}

	private void BuildProjectUi(IEngineProject project, List<EngineInstall> installs)
	{
		// at top of method
		var mutedBrush =
			(Application.Current?.Resources.TryGetResource("Brush.Muted", null, out var r) == true && r is IBrush b)
				? b
				: new SolidColorBrush(Color.Parse("#AAB2C0"));

		// container card
		var card = new Border
		{
			Classes = { "card" },
			Margin = new Thickness(2, 0, 2, 2)
		};

		var root = new Grid
		{
			RowDefinitions =
			{
				new RowDefinition(GridLength.Auto), // header
				new RowDefinition(GridLength.Auto) // meta
			},
			ColumnDefinitions =
			{
				new ColumnDefinition(new GridLength(200)),
				new ColumnDefinition(GridLength.Star),
				new ColumnDefinition(GridLength.Auto)
			}
		};

		// --- Header: Name | Version ▼ | X ---
		var nameBlock = new TextBlock
		{
			Text = project.ProjectName,
			FontWeight = FontWeight.Medium,
			FontSize = 16,
			MaxWidth = 200,
			TextTrimming = TextTrimming.LeadingCharacterEllipsis
		};
		root.Children.Add(nameBlock);

		// Version button with chevron
		var versionBtn = new Button
		{
			Classes = { "nav-btn" },
			Padding = new Thickness(0, 0),
			Margin = new Thickness(0, 2, 0, 0),
			HorizontalAlignment = HorizontalAlignment.Left,
			Content = new StackPanel
			{
				Orientation = Orientation.Horizontal,
				Spacing = 6,
				VerticalAlignment =  VerticalAlignment.Center,
				Children =
				{
					new TextBlock
					{
						Text = project.ProjectVersion,
						Padding = new Thickness(0, 5, 0, 0),
					},
					new StackPanel
					{
						Orientation = Orientation.Vertical,
						Spacing = -2, // slight overlap so they look like one icon
						Children =
						{
							new PathIcon
							{
								Data = Geometry.Parse("M 0 6 L 4 2 L 8 6 Z"), // up triangle
								Width = 8,
								Height = 6,
								Foreground = Brushes.White,
								Margin = new Thickness(0, 5, 0, 0)
							},
							new PathIcon
							{
								Data = Geometry.Parse("M 0 2 L 4 6 L 8 2 Z"), // down triangle
								Width = 8,
								Height = 6,
								Foreground = Brushes.White,
								Margin = new Thickness(0, 3, 0, 0)
							}
						}
					}
				}
			}
		};
		versionBtn.Click += async (_, _) =>
		{
			// TODO: open your version select dialog
			// await new VersionPickerDialog(...).ShowDialog(this.GetWindow());
		};
		Grid.SetColumn(versionBtn, 1);
		root.Children.Add(versionBtn);

		// Remove button
		var removeBtn = new Button
		{
			Content = "✕",
			Padding = new Thickness(10, 6),
			Foreground = Brushes.IndianRed,
			Background = Brushes.Transparent,
			BorderBrush = Brushes.Transparent,
			HorizontalAlignment = HorizontalAlignment.Right
		};
		removeBtn.Click += (_, _) =>
		{
			// TODO: remove project from settings
		};
		Grid.SetColumn(removeBtn, 2);
		root.Children.Add(removeBtn);
		
		var launchBtn = new Button
		{
			Content = "Launch Project"
		};
		launchBtn.Click += (_, _) => project.LoadProject();
		Grid.SetColumn(launchBtn, 2);
		Grid.SetRow(launchBtn, 1);
		root.Children.Add(launchBtn);

		// --- Meta row: Project Path | Last Modified ---
		var metaGrid = new Grid
		{
			ColumnDefinitions =
			{
				new ColumnDefinition(new GridLength(200)),
				new ColumnDefinition(GridLength.Auto)
			},
			Margin = new Thickness(0, 6, 0, 0)
		};

		var pathBlock = new TextBlock
		{
			Text = project.ProjectDirectory,
			Foreground = mutedBrush,
			MaxWidth = 200,
			TextTrimming = TextTrimming.LeadingCharacterEllipsis
		};

		pathBlock.Cursor = new Cursor(StandardCursorType.Hand);
		pathBlock.PointerPressed += (_, e) =>
		{
			if (e.GetCurrentPoint(pathBlock).Properties.IsLeftButtonPressed)
			{
				try
				{
					OpenFolder(project.ProjectDirectory);
				}
				catch
				{
					/* ignore */
				}
			}
		};
		metaGrid.Children.Add(pathBlock);
		
		var lastAccess = Directory.Exists(project.ProjectDirectory)
			? Directory.GetLastAccessTime(project.ProjectDirectory)
			: DateTime.MinValue;

		var lastAccessBlock = new TextBlock
		{
			Text = ToRelativeAgo(lastAccess),
			Foreground = mutedBrush,
			FontSize = 12
		};
		Grid.SetColumn(lastAccessBlock, 1);
		metaGrid.Children.Add(lastAccessBlock);

		Grid.SetRow(metaGrid, 1);
		Grid.SetColumnSpan(metaGrid, 2);
		root.Children.Add(metaGrid);

		card.Child = root;

		// wrap into outer container used by page
		var sectionWrapper = new Border { Margin = new Thickness(2, 0, 2, 2), Child = card };
		sectionWrappers.Add(sectionWrapper);
		list.Children.Add(sectionWrapper);
	}
	
	private static string ToRelativeAgo(DateTime when)
	{
		if (when == DateTime.MinValue) return "unknown";
		var span = DateTime.Now - when;
		if (span.TotalSeconds < 90) return $"{(int)Math.Max(1, span.TotalSeconds)}s ago";
		if (span.TotalMinutes < 90) return $"{(int)Math.Round(span.TotalMinutes)} mins ago";
		if (span.TotalHours   < 36) return $"{(int)Math.Round(span.TotalHours)} hrs ago";
		if (span.TotalDays    < 14) return $"{(int)Math.Round(span.TotalDays)} days ago";
		if (span.TotalDays    < 70) return $"{(int)Math.Round(span.TotalDays / 7)} wks ago";
		if (span.TotalDays    < 365) return $"{(int)Math.Round(span.TotalDays / 30)} mos ago";
		return $"{(int)Math.Round(span.TotalDays / 365)} yrs ago";
	}

	private static void OpenFolder(string path)
	{
		if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path)) return;

		if (OperatingSystem.IsWindows())
			System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("explorer.exe", $"\"{path}\"") { UseShellExecute = true });
		else if (OperatingSystem.IsMacOS())
			System.Diagnostics.Process.Start("open", $"\"{path}\"");
		else
			System.Diagnostics.Process.Start("xdg-open", $"\"{path}\"");
	}

	private bool HasMatchingInstall(IEngineProject project, List<EngineInstall> installs)
	{
		return installs.Any(install =>
			install.Version.Equals(project.ProjectVersion, StringComparison.OrdinalIgnoreCase));
	}
}
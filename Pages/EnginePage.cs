using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using GameBridge.Data.EngineData;
using GameBridge.Ui;
using GameBridge.Ui.Factory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameBridge.Pages;

public class EnginePage<T> : Page where T : IEngineProject
{
	private readonly Grid grid = new()
	{
		Margin = new Thickness(10, 0, 10, 10),
		HorizontalAlignment = HorizontalAlignment.Stretch,
		VerticalAlignment = VerticalAlignment.Top
	};

	private readonly ScrollViewer scrollView;
	private readonly List<Border> sectionWrappers = new();
	private readonly bool AutoGridWidth = true;
	private int gridWidth = 3;

	private readonly IEngineSettings<T> engineSettings;

	public EnginePage(IEngineSettings<T> engineSettings)
	{
		this.engineSettings = engineSettings;

		scrollView = new ScrollViewer
		{
			VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
			HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
			Content = grid
		};

		AddContent(scrollView);
		BuildInitialGrid();

		LayoutUpdated += (_, _) =>
		{
			if (AutoGridWidth && Bounds.Width > 0)
			{
				var newWidth = Math.Max(1, (int)(Bounds.Width / 300));
				if (newWidth != gridWidth)
				{
					gridWidth = newWidth;
					RepositionGrid();
				}
			}
		};
	}

	private void BuildInitialGrid()
	{
		grid.ColumnDefinitions.Clear();
		for (var i = 0; i < gridWidth; i++)
		{
			grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
		}

		var projects = engineSettings.GetProjects();
		var installs = engineSettings.GetEngineInstallPaths();

		for (var i = 0; i < projects.Count; i++)
		{
			var project = projects[i];

			Control settingsContent = null;
			
			switch (project)
			{
				case UnityEngineProject unity:
					settingsContent = UiFactory.ProcessClass(unity) ??
					                  new TextBlock { Text = "Failed to draw project GUI" };
					break;
				case UnrealEngineProject unreal:
					settingsContent = UiFactory.ProcessClass(unreal) ??
					                  new TextBlock { Text = "Failed to draw project GUI" };
					break;
			}
			
			var settingsExpander = new Expander
			{
				Header = "Engine Settings",
				Content = settingsContent,
				IsExpanded = false,
				Margin = new Thickness(0, 4, 0, 4)
			};

			var stack = new StackPanel
			{
				Spacing = 6
			};

			var header = new Grid
			{
				ColumnDefinitions =
				{
					new ColumnDefinition(GridLength.Star),
					new ColumnDefinition(GridLength.Auto)
				}
			};

			header.Children.Add(new TextBlock
			{
				Text = project.ProjectName,
				FontWeight = FontWeight.Bold,
				FontSize = 16
			});

			var removeBtn = new Button
			{
				Content = "✕",
				Padding = new Thickness(6, 2, 6, 2),
				Background = Brushes.Transparent,
				Foreground = Brushes.Red,
				BorderBrush = null
			};
			removeBtn.Click += (_, _) =>
			{
				// REMOVE PROJECT - method not yet implemented
			};
			Grid.SetColumn(removeBtn, 1);
			header.Children.Add(removeBtn);

			stack.Children.Add(header);

			var launchRow = new StackPanel
			{
				Orientation = Orientation.Horizontal,
				Spacing = 6
			};

			var launchBtn = new Button
			{
				Content = "Launch Project"
			};
			launchBtn.Click += (_, _) => project.LoadProject();

			launchRow.Children.Add(launchBtn);

			if (!HasMatchingInstall(project, installs))
			{
				var downloadBtn = new Button
				{
					Content = $"Download {project.ProjectVersion}",
					Background = Brushes.Orange,
					Foreground = Brushes.Black
				};
				downloadBtn.Click += (_, _) =>
				{
					// DOWNLOAD VERSION - method not yet implemented
				};
				launchRow.Children.Add(downloadBtn);
			}

			stack.Children.Add(launchRow);
			stack.Children.Add(settingsExpander);

			var section = new Section
			{
				ContentContainer = { Children = { stack } }
			};

			var sectionWrapper = new Border
			{
				Margin = new Thickness(2, 0, 2, 2),
				Child = section
			};

			sectionWrappers.Add(sectionWrapper);
			grid.Children.Add(sectionWrapper);
		}

		RepositionGrid();
	}
	
	private bool HasMatchingInstall(IEngineProject project, List<EngineInstall> installs)
	{
		return installs.Any(install =>
			install.Version.Equals(project.ProjectVersion, StringComparison.OrdinalIgnoreCase));
	}

	private void RepositionGrid()
	{
		grid.ColumnDefinitions.Clear();
		for (var i = 0; i < gridWidth; i++)
		{
			grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
		}

		var rows = (int)Math.Ceiling((double)sectionWrappers.Count / gridWidth);
		grid.RowDefinitions.Clear();
		for (var i = 0; i < rows; i++)
		{
			grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
		}

		for (var i = 0; i < sectionWrappers.Count; i++)
		{
			var control = sectionWrappers[i];
			Grid.SetRow(control, i / gridWidth);
			Grid.SetColumn(control, i % gridWidth);
		}
	}
}
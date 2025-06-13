using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using GameBridge.Data.EngineData;
using GameBridge.Ui;
using GameBridge.Ui.Factory;
using System;

namespace GameBridge.Pages;

public class EnginePage<T> : Page where T : IEngineProject
{
	private int gridWidth = 2;

	public EnginePage(IEngineSettings<T> engineSettings)
	{
		var scrollView = new ScrollViewer
		{
			VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
			HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Stretch
		};

		var grid = new Grid
		{
			Margin = new Thickness(10),
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Top
		};

		// Define columns
		for (int i = 0; i < gridWidth; i++)
		{
			grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
		}

		var projects = engineSettings.GetProjects();

		// Optional: auto-calculate grid height
		int rows = (int)Math.Ceiling((double)projects.Count / gridWidth);

		for (int i = 0; i < rows; i++)
		{
			grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
		}

		// Add project sections to grid
		for (int i = 0; i < projects.Count; i++)
		{
			var project = projects[i];
			var section = new Section(project.ProjectName)
			{
				HorizontalAlignment = HorizontalAlignment.Stretch
			};

			switch (project)
			{
				case UnityEngineProject unityEngineProject:
					section.AddContent(UiFactory.ProcessClass(unityEngineProject) ?? new TextBlock{Text = "Failed to draw project Gui"});
					break;
				case UnrealEngineProject unrealEngineProject:
					section.AddContent(UiFactory.ProcessClass(unrealEngineProject) ?? new TextBlock{Text = "Failed to draw project Gui"});
					break;
			}

			int row = i / gridWidth;
			int column = i % gridWidth;

			// ✅ Wrap section in a Border to add spacing
			var sectionWrapper = new Border
			{
				Margin = new Thickness(3), // adjust as needed
				Child = section
			};

			Grid.SetRow(sectionWrapper, row);
			Grid.SetColumn(sectionWrapper, column);
			grid.Children.Add(sectionWrapper);
		}

		scrollView.Content = grid;
		AddContent(scrollView);
	}
}
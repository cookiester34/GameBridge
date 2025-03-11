using Avalonia;
using GameBridge.Data.EngineData;
using GameBridge.Ui;
using GameBridge.Ui.Factory;
using Avalonia.Controls;
using Avalonia.Layout;

namespace GameBridge.Pages;

public class EnginePage<T> : Page where T : IEngineProject
{
	public EnginePage(IEngineSettings<T> engineSettings)
	{
		var scrollView = new ScrollView();
		AddContent(scrollView);

		// Create grid with 2 columns
		var grid = new Grid
		{
			ColumnDefinitions = 
			{
				new ColumnDefinition(GridLength.Star),
				new ColumnDefinition(GridLength.Star)
			},
			Margin = new Thickness(10),
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Top
		};

		scrollView.AddContent(grid);

		var projects = engineSettings.GetProjects();
        
		for (int i = 0; i < projects.Count; i++)
		{
			var project = projects[i];
			var section = new Section(project.ProjectName);
			if (project is UnityEngineProject unityEngineProject)
			{
				section.AddContent(UiFactory.ProcessClass(unityEngineProject));
			}
			else if (project is UnrealEngineProject unrealEngineProject)
			{
				section.AddContent(UiFactory.ProcessClass(unrealEngineProject));
			}

			// Calculate grid position
			int row = i / 2;
			int column = i % 2;

			// Add rows as needed
			while (grid.RowDefinitions.Count <= row)
			{
				grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
			}

			// Add to grid
			Grid.SetRow(section, row);
			Grid.SetColumn(section, column);
			grid.AddChild(section);
		}
	}
}
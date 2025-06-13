using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
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
		Margin = new Thickness(10),
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
				int newWidth = Math.Max(1, (int)(Bounds.Width / 300));
				if (newWidth != gridWidth)
				{
					gridWidth = newWidth;
					AnimateGridReposition();
				}
			}
		};
	}

	private void BuildInitialGrid()
	{
		grid.ColumnDefinitions.Clear();
		for (int i = 0; i < gridWidth; i++)
			grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

		var projects = engineSettings.GetProjects();
		engineSettings.GetEngineInstallPaths();

		for (int i = 0; i < projects.Count; i++)
		{
			var project = projects[i];
			var section = new Section(project.ProjectName)
			{
				HorizontalAlignment = HorizontalAlignment.Stretch
			};

			switch (project)
			{
				case UnityEngineProject unity:
					section.AddContent(UiFactory.ProcessClass(unity) ?? new TextBlock { Text = "Failed to draw project GUI" });
					break;
				case UnrealEngineProject unreal:
					section.AddContent(UiFactory.ProcessClass(unreal) ?? new TextBlock { Text = "Failed to draw project GUI" });
					break;
			}

			var sectionWrapper = new Border
			{
				Margin = new Thickness(3),
				Child = section
			};

			sectionWrappers.Add(sectionWrapper);
			grid.Children.Add(sectionWrapper);
		}

		AnimateGridReposition();
	}

	private void AnimateGridReposition()
	{
		var previousPositions = new Dictionary<Control, Point>();

		// 1. Record the current positions before layout change
		foreach (var control in sectionWrappers)
		{
			var oldPoint = control.TranslatePoint(new Point(0, 0), this);
			if (oldPoint != null)
			{
				previousPositions[control] = oldPoint.Value;
			}
		}

		// 2. Apply new grid layout
		grid.ColumnDefinitions.Clear();
		for (int i = 0; i < gridWidth; i++)
			grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

		int rows = (int)Math.Ceiling((double)sectionWrappers.Count / gridWidth);
		grid.RowDefinitions.Clear();
		for (int i = 0; i < rows; i++)
			grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

		for (int i = 0; i < sectionWrappers.Count; i++)
		{
			var control = sectionWrappers[i];
			Grid.SetRow(control, i / gridWidth);
			Grid.SetColumn(control, i % gridWidth);
		}

		// 3. After layout, animate them to new positions
		Dispatcher.UIThread.Post(() =>
		{
			foreach (var control in sectionWrappers)
			{
				var oldPos = previousPositions.TryGetValue(control, out var val) ? val : default;
				var newPos = control.TranslatePoint(new Point(0, 0), this) ?? default;

				double dx = oldPos.X - newPos.X;
				double dy = oldPos.Y - newPos.Y;

				// Start from old offset
				control.RenderTransform = new TranslateTransform { X = dx, Y = dy };

				// Add transition
				control.Transitions = new Transitions
				{
					new TransformOperationsTransition
					{
						Property = Control.RenderTransformProperty,
						Duration = TimeSpan.FromMilliseconds(300),
						Easing = new Avalonia.Animation.Easings.CubicEaseOut()
					}
				};

				// Animate to new position (0,0)
				(control.RenderTransform as TranslateTransform)!.X = 0;
				(control.RenderTransform as TranslateTransform)!.Y = 0;
			}
		}, DispatcherPriority.Render);
	}
}
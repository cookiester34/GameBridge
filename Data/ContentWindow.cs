using Avalonia.Controls;
using GameBridge.Ui;
using Avalonia.Layout;
using System;

namespace GameBridge.Data;

public abstract class ContentWindow : Window
{
	public WindowContext Ctx { get; private set; }

	public ContentWindow()
	{
		Ctx = new WindowContext();
		Ctx.DockPanel.LastChildFill = true; // Important for layout to fill space
		Content = Ctx.DockPanel;
	}

	public void AddContentToWindow(Control control)
	{
		control.HorizontalAlignment = HorizontalAlignment.Stretch;
		control.VerticalAlignment = VerticalAlignment.Stretch;
		control.Width = double.NaN;
		control.Height = double.NaN;

		Ctx.DockPanel.Children.Add(control);
	}

	public void RemoveContentToWindow(Control control)
	{
		Ctx.DockPanel.Children.Remove(control);
	}

	// Optional alignment helpers — apply directly to controls
	public void AlignContent(Control control, HorizontalAlignment horizontal, VerticalAlignment vertical)
	{
		control.HorizontalAlignment = horizontal;
		control.VerticalAlignment = vertical;
	}
	
	public Control CenterContentInWindow(Control control, double horizontalPaddingPercent = 0.2)
	{
		// Clamp percent between 0 and 0.5 (max 50% padding on each side)
		horizontalPaddingPercent = Math.Clamp(horizontalPaddingPercent, 0, 0.5);

		control.HorizontalAlignment = HorizontalAlignment.Stretch;
		control.VerticalAlignment = VerticalAlignment.Center;
		control.Width = double.NaN;

		var container = new Border
		{
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Center,
			Child = control
		};

		var grid = new Grid
		{
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Stretch
		};

		grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));

		// Left - Center - Right columns
		grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(horizontalPaddingPercent, GridUnitType.Star)));
		grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1 - 2 * horizontalPaddingPercent, GridUnitType.Star)));
		grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(horizontalPaddingPercent, GridUnitType.Star)));

		Grid.SetRow(container, 0);
		Grid.SetColumn(container, 1); // Place in center column
		grid.Children.Add(container);

		AddContentToWindow(grid);
		return grid;
	}
}
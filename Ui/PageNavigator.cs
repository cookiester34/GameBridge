using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using GameBridge.Data;
using System;
using System.Collections.Generic;

namespace GameBridge.Ui;

public class PageNavigator : Panel
{
	private Dictionary<string, PageArg> pages = new();

	// private ScrollView resizableScrollView;
	private StackPanel stackPanel;
	private Grid grid;
	private StackPanel pageButtonPanel;

	public PageNavigator()
	{
		// VerticalAlignment = VerticalAlignment.Stretch;
		// Height = double.NaN;
		
		stackPanel = new StackPanel
		{
			// VerticalAlignment = VerticalAlignment.Stretch
		};
		Children.Add(stackPanel);
		
		grid = new Grid
		{
			HorizontalAlignment = HorizontalAlignment.Stretch,
			ColumnDefinitions =
			{
				new ColumnDefinition(new GridLength(150)),
				new ColumnDefinition(GridLength.Star)
			}
		};
		stackPanel.AddChild(grid);
		
		pageButtonPanel = new StackPanel
		{
			Background = WindowColors.SecondaryBackgroundColor,
		};
		Grid.SetColumn(pageButtonPanel, 0);
		grid.AddChild(pageButtonPanel);
	}
	
	public void AddPage(string name, Page page)
	{
		PageArg pageArg;
		if (!pages.TryAdd(name, pageArg = new PageArg
		    {
			    Page = page
		    }))
		{
			throw new Exception("Page already exists in page navigator");
		}

		pageArg.NavigationButton = new Button
		{
			Content = name,
			HorizontalAlignment = HorizontalAlignment.Stretch,
			Margin = new Thickness(3)
		};
		pageArg.NavigationButton.Click += (_, _) =>
		{
			SwitchPage(name);
		};
		pageButtonPanel.AddChild(pageArg.NavigationButton);
	}

	public void RemovePage(string name)
	{
		if (pages.TryGetValue(name, out PageArg pageArg))
		{
			pageButtonPanel.Children.Remove(pageArg.NavigationButton);
			grid.Children.Remove(pageArg.Page);
			
			pages.Remove(name);
		}
	}

	public void SwitchPage(string name)
	{
		if (pages.TryGetValue(name, out var pageArg))
		{
			if (grid.Children.Count > 1)
			{
				grid.RemoveChildAt(1);
			}
			var pageArgPage = pageArg.Page;
			Grid.SetColumn(pageArgPage, 1);
			grid.AddChild(pageArgPage);
		}
	}
	
	private struct PageArg
	{
		public Page Page { get; set; }
		public Button NavigationButton { get; set; }
	}
}
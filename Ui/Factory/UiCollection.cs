using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using GameBridge.Data;
using GameBridge.Ui.Factory.UiFabrication;
using GameBridge.Ui.Factory.UiFabrication.DataBinder;
using GameBridge.Ui.Factory.UiFabrication.Decorators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GameBridge.Ui.Factory;

public class UiCollection : Panel
{
	private readonly Type templateType;
	private readonly MemberInfo[] members;
	private readonly bool isPrimitiveType;
	private readonly object[] attributes;
	private readonly IDataBinder[] dataBinders;
	private readonly IDecorator[] decorators;
	
	private StackPanel StackPanel { get; set; }
	private StackPanel topPanelPanel { get; set; }
	private StackPanel elementPanel { get; set; }
	private StackPanel bottomPanel { get; set; }

	private List<IUiFabricator> fabricators = new();

	private IList modifiableCollection;

	public UiCollection(string name, Type collectionType, object[] attributes)
	{
		Name = "Collection";
		HorizontalAlignment = HorizontalAlignment.Stretch;
		VerticalAlignment = VerticalAlignment.Stretch;
		Width = Double.NaN;
		
		this.attributes = attributes;
		dataBinders = attributes.OfType<IDataBinder>().ToArray();
		decorators = attributes.OfType<IDecorator>().ToArray();
		templateType = collectionType.IsArray ? collectionType.GetElementType() : collectionType.GetGenericArguments()[0];
		members = templateType.GetMembers(BindingFlags.Instance | BindingFlags.Public);
		isPrimitiveType = collectionType.IsPrimitive;

		var stackPanel = new StackPanel();
		Children.Add(stackPanel);
        
		var gridContainer = new Grid
		{
			ColumnDefinitions =
			{
				new ColumnDefinition(new GridLength(60)),
				new ColumnDefinition(GridLength.Star)
			},
			Margin = new Thickness(0, 0, 0, 0)
		};
		stackPanel.AddChild(gridContainer);
		
		var collapseButton = new Button
		{
			Content = "Show",
			Height = 25,
			VerticalAlignment = VerticalAlignment.Top
		};
		Grid.SetColumn(collapseButton, 0);
		gridContainer.Children.Add(collapseButton);

		var collectionName = new TextBlock
		{
			Text = name,
			Padding = new Thickness(4, 0, 0, 0),
			VerticalAlignment = VerticalAlignment.Center
		};
		Grid.SetColumn(collectionName, 1);
		gridContainer.Children.Add(collectionName);

		StackPanel = new StackPanel()
		{
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Stretch,
			Width = Double.NaN
		};
		
		var border = new Border
		{
			Background = WindowColors.ThirdBackgroundColor,
			BorderThickness = new Thickness(1),
			CornerRadius = new CornerRadius(3),
			Padding = new Thickness(2),
			Margin = new Thickness(0, 6, 0, 2),
			Child = StackPanel,
			IsVisible = false
		};
		stackPanel.AddChild(border);
		
		collapseButton.Click += (_, _) =>
		{
			border.IsVisible = !border.IsVisible;
			collapseButton.Content = border.IsVisible ? "Collapse" : "Show";
		};
		
		topPanelPanel = new StackPanel()
		{
			Orientation = Orientation.Horizontal,
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Top,
			Width = Double.NaN
		};
		StackPanel.AddChild(topPanelPanel);
		
		elementPanel = new StackPanel()
		{
			Orientation = Orientation.Vertical,
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Stretch,
			Width = Double.NaN
		};
		StackPanel.AddChild(elementPanel);
		
		bottomPanel = new StackPanel()
		{
			Orientation = Orientation.Horizontal,
			HorizontalAlignment = HorizontalAlignment.Right,
			VerticalAlignment = VerticalAlignment.Top,
			Width = Double.NaN
		};
		StackPanel.AddChild(bottomPanel);

		var removeButton = new Button
		{
			Content = "-",
			Width = 30,
			Height = 24.5,
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,
			Margin = new Thickness(0,2,6,0)
		};
		removeButton.Click += (_, _) =>
		{
			if (modifiableCollection != null)
			{
				RemoveAtIndex(modifiableCollection.Count - 1);
			}
		};
		bottomPanel.AddChild(removeButton);

		var addButton = new Button
		{
			Content = "+",
			Width = 30,
			Height = 24.5,
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,
			Margin = new Thickness(0,2,2,0)
		};
		addButton.Click += (_, _) =>
		{
			if (modifiableCollection != null)
			{
				if (templateType == typeof(string))
				{
					AddAtIndex("", modifiableCollection.Count);
				}
				else
				{
					AddAtIndex(Activator.CreateInstance(templateType), modifiableCollection.Count);
				}
			}
		};
		bottomPanel.AddChild(addButton);
	}

	public void InitializeUi(IEnumerable collection)
	{
		if (collection is not IList modifiableList) return;
		modifiableCollection = modifiableList;

		var index = -1;
		foreach (var element in collection)
		{
			index++;
			AddAtIndex(element, index, false);
		}
	}

	public void RemoveAtIndex(int index)
	{
		if (index >= 0 && index < elementPanel.Children.Count)
		{
			elementPanel.Children.RemoveAt(index);
		}

		if (index >= 0 && index < modifiableCollection.Count)
		{
			modifiableCollection.RemoveAt(index);
		}

		var fabricatorsCount = fabricators.Count;
		if (fabricatorsCount <= 0) return;
		for (int i = index + 1; i < fabricatorsCount; i++)
		{
			fabricators[i].UpdateBoundCollectionIndex(i - 1);
		}

		fabricators[index].DataWatcher.Dispose();
		fabricators.RemoveAt(index);
		
		DataManager.SaveData();
	}

	public void RemoveContent(Control? control)
	{
		if (control == null) return;

		var index = elementPanel.Children.IndexOf(control);
		if (index == -1) return;

		RemoveAtIndex(index);
	}

	public void AddAtIndex(object value, int index, bool modifySource = true)
	{
		if (modifySource)
		{
			if (index >= 0 && index < modifiableCollection.Count)
			{
				var existingData = modifiableCollection[index];
				modifiableCollection[index] = value;
				modifiableCollection.Add(existingData);
			}
			else
			{
				modifiableCollection.Add(value);
			}
		}

		var gridContainer = new Grid
		{
			ColumnDefinitions =
			{
				new ColumnDefinition(GridLength.Star),
				new ColumnDefinition(GridLength.Auto)
			},
			Margin = new Thickness(0, 0, 0, 0)
		};
		
		var border = new Border
		{
			Background = WindowColors.ThirdBackgroundColor,
			BorderThickness = new Thickness(1),
			CornerRadius = new CornerRadius(3),
			Padding = new Thickness(2),
			Margin = new Thickness(0, 0, 0, 2),
			Child = gridContainer
		};
		elementPanel.Children.Add(border);

		// Content Panel
		var contentPanel = new StackPanel
		{
			Orientation = Orientation.Vertical,
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Center,
			Width = Double.NaN
		};
		Grid.SetColumn(contentPanel, 0);
		gridContainer.Children.Add(contentPanel);

		// Options Panel
		var optionsPanel = new StackPanel
		{
			Orientation = Orientation.Vertical,
			HorizontalAlignment = HorizontalAlignment.Right,
			VerticalAlignment = VerticalAlignment.Center,
			Width = 70
		};
		Grid.SetColumn(optionsPanel, 1);
		gridContainer.Children.Add(optionsPanel);

		// Remove Button
		var removeButton = new Button
		{
			Content = "Remove",
			Width = 65,
			Height = 24.5,
			HorizontalAlignment = HorizontalAlignment.Right,
			VerticalAlignment = VerticalAlignment.Center
		};
		removeButton.Click += (_, _) =>
		{
			RemoveContent(border);
		};
		optionsPanel.Children.Add(removeButton);

		if (isPrimitiveType || templateType == typeof(string))
		{
			var fabricator = FactoryHelpers.GetFabricatorForType(templateType);
			if (fabricator != null)
			{
				fabricator.CreateField("", value, attributes);
				fabricator.BindCollectionElement(modifiableCollection, index, dataBinders);
				
				if (decorators.Length > 0)
				{
					var topLevel = decorators.Where(decorator => decorator.IsTopDecorator);
					var bottomLevel = decorators.Where(decorator => !decorator.IsTopDecorator);
			
					var container = new StackPanel();

					var target = fabricator.ModifiableCollection[fabricator.BoundCollectionIndex];
			
					// top decorators
					foreach (var decorator in topLevel)
					{
						container.AddChild(decorator.CreateDecorator(target));
					}
			
					//Field
					container.AddChild(fabricator.Field);
			
					//bottom decorators
					foreach (var decorator in bottomLevel)
					{
						container.AddChild(decorator.CreateDecorator(target));
					}
					
					contentPanel.AddChild(container);
				}
				else
				{
					contentPanel.AddChild(fabricator.Field);
				}
				
				fabricators.Add(fabricator);
			}
		}
		else
		{
			foreach (var elementMember in members)
			{
				if (elementMember.MemberType != MemberTypes.Property &&
				    elementMember.MemberType != MemberTypes.Field) continue;
				
				contentPanel.AddChild(elementMember.CreateAndBindUi(value));
			}
		}
		
		DataManager.SaveData();
	}
}
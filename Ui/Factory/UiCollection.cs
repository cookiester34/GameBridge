using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using GameBridge.Data;
using GameBridge.Ui.Factory;
using GameBridge.Ui.Factory.UiFabrication;
using GameBridge.Ui.Factory.UiFabrication.DataBinder;
using GameBridge.Ui.Factory.UiFabrication.Decorators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class UiCollection : UserControl
{
	private readonly Type templateType;
	private readonly MemberInfo[] members;
	private readonly bool isPrimitiveType;
	private readonly object[] attributes;
	private readonly IDataBinder[] dataBinders;
	private readonly IDecorator[] decorators;

	private readonly StackPanel elementPanel;
	private readonly List<IUiFabricator> fabricators = new();
	private IList modifiableCollection;

	public UiCollection(string name, Type collectionType, object[] attributes)
	{
		this.attributes = attributes;
		dataBinders = attributes.OfType<IDataBinder>().ToArray();
		decorators = attributes.OfType<IDecorator>().ToArray();

		templateType = collectionType.IsArray
			? collectionType.GetElementType()
			: collectionType.GetGenericArguments()[0];

		members = templateType.GetMembers(BindingFlags.Instance | BindingFlags.Public);
		isPrimitiveType = collectionType.IsPrimitive;

		// Top-level layout container
		var root = new StackPanel
		{
			Orientation = Orientation.Vertical,
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Stretch,
			Spacing = 8
		};

		// Header row
		var headerGrid = new Grid
		{
			ColumnDefinitions =
			{
				new ColumnDefinition(GridLength.Star),
				new ColumnDefinition(GridLength.Auto)
			}
		};
		root.Children.Add(headerGrid);

		headerGrid.Children.Add(new TextBlock
		{
			Text = name,
			FontWeight = FontWeight.Bold,
			VerticalAlignment = VerticalAlignment.Center,
			Margin = new Thickness(2)
		});

		var toggleButton = new Button
		{
			Content = "Show",
			Margin = new Thickness(2),
			MinWidth = 60
		};
		Grid.SetColumn(toggleButton, 1);
		headerGrid.Children.Add(toggleButton);

		// Collapsible container
		var collapsibleBorder = new Border
		{
			BorderThickness = new Thickness(1),
			CornerRadius = new CornerRadius(4),
			Padding = new Thickness(6),
			Margin = new Thickness(0, 4, 0, 0),
			Background = WindowColors.ThirdBackgroundColor,
			IsVisible = false
		};
		root.Children.Add(collapsibleBorder);

		var collectionContent = new StackPanel
		{
			Orientation = Orientation.Vertical,
			HorizontalAlignment = HorizontalAlignment.Stretch
		};
		collapsibleBorder.Child = collectionContent;

		elementPanel = new StackPanel
		{
			Orientation = Orientation.Vertical,
			HorizontalAlignment = HorizontalAlignment.Stretch,
			Spacing = 6
		};
		collectionContent.Children.Add(elementPanel);

		var buttonRow = new StackPanel
		{
			Orientation = Orientation.Horizontal,
			HorizontalAlignment = HorizontalAlignment.Right,
			Spacing = 4,
			Margin = new Thickness(2),
		};
		collectionContent.Children.Add(buttonRow);

		var addButton = new Button
		{
			Content = new TextBlock
			{
				Text = "+",
				FontSize = 13,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center
			},
			Width = 30,
			Height = 30,
			Padding = new Thickness(5)
		};

		var removeButton = new Button
		{
			Content = new TextBlock
			{
				Text = "–",
				FontSize = 13,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center
			},
			Width = 30,
			Height = 30,
			Padding = new Thickness(5)
		};

		addButton.Click += (_, _) =>
		{
			if (modifiableCollection == null) return;
			var value = templateType == typeof(string)
				? ""
				: Activator.CreateInstance(templateType);
			AddAtIndex(value, modifiableCollection.Count);
		};

		removeButton.Click += (_, _) =>
		{
			if (modifiableCollection?.Count > 0)
				RemoveAtIndex(modifiableCollection.Count - 1);
		};

		buttonRow.Children.Add(removeButton);
		buttonRow.Children.Add(addButton);

		toggleButton.Click += (_, _) =>
		{
			collapsibleBorder.IsVisible = !collapsibleBorder.IsVisible;
			toggleButton.Content = collapsibleBorder.IsVisible ? "Collapse" : "Show";
		};

		Content = root;
	}

	public void InitializeUi(IEnumerable collection)
	{
		if (collection is not IList modifiableList) return;
		modifiableCollection = modifiableList;

		for (int i = 0; i < modifiableList.Count; i++)
		{
			AddAtIndex(modifiableList[i], i, false);
		}
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

		var content = new Grid
		{
			ColumnDefinitions =
			{
				new ColumnDefinition(GridLength.Star),
				new ColumnDefinition(GridLength.Auto)
			},
			Margin = new Thickness(0, 0, 0, 2)
		};

		var border = new Border
		{
			Background = WindowColors.ThirdBackgroundColor,
			CornerRadius = new CornerRadius(4),
			Padding = new Thickness(6),
			Child = content
		};
		elementPanel.Children.Add(border);

		var fieldContainer = new StackPanel
		{
			Orientation = Orientation.Vertical,
			HorizontalAlignment = HorizontalAlignment.Stretch
		};
		Grid.SetColumn(fieldContainer, 0);
		content.Children.Add(fieldContainer);

		var removeButton = new Button
		{
			Content = "Remove",
			Margin = new Thickness(4),
			HorizontalAlignment = HorizontalAlignment.Right
		};
		removeButton.Click += (_, _) => RemoveContent(border);
		Grid.SetColumn(removeButton, 1);
		content.Children.Add(removeButton);

		if (isPrimitiveType || templateType == typeof(string))
		{
			var fabricator = FactoryHelpers.GetFabricatorForType(templateType);
			if (fabricator != null)
			{
				fabricator.CreateField("", value, attributes);
				fabricator.BindCollectionElement(modifiableCollection, index, dataBinders);

				fabricator.Field.HorizontalAlignment = HorizontalAlignment.Stretch;
				fabricator.Field.Width = double.NaN;

				fieldContainer.Children.Add(fabricator.Field);
				fabricators.Add(fabricator);
			}
		}
		else
		{
			foreach (var member in members)
			{
				if (member.MemberType is not MemberTypes.Property and not MemberTypes.Field)
					continue;

				var ui = member.CreateAndBindUi(value);
				if (ui != null)
				{
					ui.HorizontalAlignment = HorizontalAlignment.Stretch;
					ui.Width = double.NaN;
					fieldContainer.Children.Add(ui);
				}
			}
		}

		DataManager.SaveData();
	}

	public void RemoveAtIndex(int index)
	{
		if (index >= 0 && index < elementPanel.Children.Count)
			elementPanel.Children.RemoveAt(index);

		if (index >= 0 && index < modifiableCollection.Count)
			modifiableCollection.RemoveAt(index);

		if (index < fabricators.Count)
		{
			fabricators[index].DataWatcher.Dispose();
			fabricators.RemoveAt(index);
		}

		for (int i = index; i < fabricators.Count; i++)
		{
			fabricators[i].UpdateBoundCollectionIndex(i);
		}

		DataManager.SaveData();
	}

	public void RemoveContent(Control control)
	{
		var index = elementPanel.Children.IndexOf(control);
		if (index != -1)
			RemoveAtIndex(index);
	}
}
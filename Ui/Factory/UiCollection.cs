using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using GameBridge.Ui.Attributes;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace GameBridge.Ui.Factory;

public class UiCollection : Panel
{
	private StackPanel StackPanel { get; set; }

	private IList modifiableCollection;
	private Type templateType;
	private MemberInfo[] members;
	private bool isPrimitiveType;
	private object[] attributes;
	private bool hasAttributes;

	public UiCollection(Type collectionType, object[] attributes)
	{
		this.attributes = attributes;
		hasAttributes = attributes is { Length: > 0 };
		templateType = collectionType.IsArray ? collectionType.GetElementType() : collectionType.GetGenericArguments()[0];
		members = templateType.GetMembers(BindingFlags.Instance | BindingFlags.Public);
		isPrimitiveType = collectionType.IsPrimitive;

		StackPanel = new StackPanel()
		{
			HorizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment = VerticalAlignment.Top
		};

		Children.Add(StackPanel);
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
		if (index >= 0 && index < StackPanel.Children.Count)
		{
			StackPanel.Children.RemoveAt(index);
		}

		if (index >= 0 && index < modifiableCollection.Count)
		{
			modifiableCollection.RemoveAt(index);
		}
	}

	public void RemoveContent(Control? control)
	{
		if (control == null) return;

		var index = StackPanel.Children.IndexOf(control);
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
				new ColumnDefinition(GridLength.Star),  // Main Content (expands)
				new ColumnDefinition(GridLength.Auto)   // Options like Remove button
			}
		};
		StackPanel.Children.Add(gridContainer);

		// Content Panel
		var contentPanel = new StackPanel
		{
			Orientation = Orientation.Vertical,
			HorizontalAlignment = HorizontalAlignment.Center
		};
		Grid.SetColumn(contentPanel, 0);
		gridContainer.Children.Add(contentPanel);

		// Options Panel
		var optionsPanel = new StackPanel
		{
			Orientation = Orientation.Vertical,
			HorizontalAlignment = HorizontalAlignment.Right,
			VerticalAlignment = VerticalAlignment.Top,
			Width = 80
		};
		Grid.SetColumn(optionsPanel, 1);
		gridContainer.Children.Add(optionsPanel);

		// Remove Button
		var removeButton = new Button
		{
			Content = "Remove",
			Width = 60,
			Height = 24.5,
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,
		};
		removeButton.Click += (_, _) =>
		{
			RemoveContent(gridContainer);
		};
		optionsPanel.Children.Add(removeButton);

		if (isPrimitiveType || templateType == typeof(string))
		{
			var name = $"Index: {index}";
			var uiElementMember = UiFactory.CreateUiFieldForType(templateType, attributes, name, value);
			if (uiElementMember == null) return;

			if (templateType == typeof(string))
			{
				if (hasAttributes)
				{
					if (attributes.Any(o => o.GetType() == typeof(PathAttribute)))
					{
						var explorerField = (ExplorerField)uiElementMember;
						explorerField.TextChanged += (_, _) =>
						{
							var newValue = explorerField.Text;
							modifiableCollection[index] = newValue;
						};
						contentPanel.AddChild(explorerField);
						return;
					}

					if (attributes.Any(o => o.GetType() == typeof(InputFieldAttribute)))
					{
						var textBox = (TextBox)uiElementMember;
						textBox.TextChanged += (_, _) =>
						{
							var newValue = textBox.Text;
							modifiableCollection[index] = newValue;
						};
						contentPanel.AddChild(FactoryHelpers.CreateNameField(name, textBox));
						return;
					}
				}
				else
				{
					contentPanel.AddChild(FactoryHelpers.CreateNameField(name, uiElementMember));
					return;
				}
			}

			if (templateType == typeof(int))
			{
				if (attributes.Any(o => o.GetType() == typeof(InputFieldAttribute)))
				{
					var textBox = (TextBox)uiElementMember;
					textBox.Watermark = "Int...";
					textBox.TextChanged += FactoryHelpers.ValidateIntegerInputHandler;
					textBox.TextChanged += (_, _) =>
					{
						var newValue = int.Parse(textBox.Text);
						modifiableCollection[index] = newValue;
					};
					contentPanel.AddChild(FactoryHelpers.CreateNameField(name, textBox));
					return;
				}
			}

			if (templateType == typeof(float))
			{
				if (attributes.Any(o => o.GetType() == typeof(InputFieldAttribute)))
				{
					var textBox = (TextBox)uiElementMember;
					textBox.Watermark = "Float...";
					textBox.TextChanged += FactoryHelpers.ValidateFloatInputHandler;
					textBox.TextChanged += (_, _) =>
					{
						var newValue = float.Parse(textBox.Text);
						modifiableCollection[index] = newValue;
					};
					contentPanel.AddChild(FactoryHelpers.CreateNameField(name, textBox));
					return;
				}
			}

			if (templateType == typeof(bool))
			{
				var toggle = (ToggleSwitch)uiElementMember;
				toggle.IsCheckedChanged += (_, _) =>
				{
					var newValue = (bool)toggle.IsChecked;
					modifiableCollection[index] = newValue;
					;
				};
				contentPanel.AddChild(FactoryHelpers.CreateNameField(name, toggle));
				return;
			}

			if (templateType is { IsClass: true } && !typeof(IEnumerable).IsAssignableFrom(templateType))
			{
				contentPanel.AddChild(UiFactory.ProcessClass(value, templateType));
				return;
			}

			contentPanel.AddChild(FactoryHelpers.CreateNameField(name, uiElementMember));
			return;
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
	}
}
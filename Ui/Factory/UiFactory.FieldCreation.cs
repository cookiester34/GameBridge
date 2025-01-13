using Avalonia.Controls;
using GameBridge.Ui.Attributes;
using System;
using System.Collections;
using System.Linq;

namespace GameBridge.Ui.Factory;

public static partial class UiFactory
{
	public static Control? CreateUiFieldForType(Type type, object[] attributes, string name, object value)
	{
		var hasAttributes = attributes.Length > 0;

		if (type == typeof(string))
		{
			if (hasAttributes)
			{
				if (attributes.Any(o => o.GetType() == typeof(PathAttribute)))
				{
					var pathAttribute = (PathAttribute)attributes.First(o => o.GetType() == typeof(PathAttribute));
					var explorerField = new ExplorerField(name, (string?)value, pathAttribute.PathType);
					return explorerField;
				}

				if (attributes.Any(o => o.GetType() == typeof(InputFieldAttribute)))
				{
					var textBox = new TextBox
					{
						Text = (string?)value
					};
					return textBox;
				}
			}

			var textBlock = new TextBlock
			{
				Text = (string?)value
			};
			return textBlock;
		}

		if (type == typeof(int))
		{
			if (attributes.Any(o => o.GetType() == typeof(InputFieldAttribute)))
			{
				var textBox = new TextBox
				{
					Watermark = "Int..."
				};
				return textBox;
			}

			var textBlock = new TextBlock
			{
				Text = value.ToString()
			};
			return textBlock;
		}

		if (type == typeof(float))
		{
			if (attributes.Any(o => o.GetType() == typeof(InputFieldAttribute)))
			{
				var textBox = new TextBox
				{
					Watermark = "Float.."
				};
				return textBox;
			}

			var textBlock = new TextBlock
			{
				Text = value.ToString()
			};
			return textBlock;
		}

		if (type == typeof(bool))
		{
			var toggle = new ToggleSwitch();
			toggle.IsChecked = (bool)(value ?? throw new InvalidOperationException());
			return toggle;
		}

		if (type is { IsClass: true } && !typeof(IEnumerable).IsAssignableFrom(type))
		{
			return ProcessClass(value, type);
		}

		return null;
	}
}
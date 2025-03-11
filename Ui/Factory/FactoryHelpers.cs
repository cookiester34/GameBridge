using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using GameBridge.Ui.Factory.UiFabrication;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace GameBridge.Ui.Factory;

public static partial class FactoryHelpers
{
	[GeneratedRegex(@"^\d*$")]
	private static partial Regex IntCheckRegex();

	[GeneratedRegex(@"^\d*\.?\d*$")]
	private static partial Regex FloatCheckRegex();

	private static void ValidateFloatInput(TextBox textBox)
	{
		// Unsubscribe from the TextChanged event
		textBox.TextChanged -= ValidateFloatInputHandler;

		// Remove invalid characters (non-digits)
		var textBoxText = textBox.Text;
		if (textBoxText != null && !FloatCheckRegex().IsMatch(textBoxText))
		{
			textBox.Text = Regex.Replace(textBoxText, @"[^0-9.]", "");
			textBox.CaretIndex = textBoxText.Length;
		}

		textBoxText = textBox.Text;
		if (textBoxText != null && textBoxText.Split('.').Length > 2)
		{
			textBox.Text = textBoxText.Remove(textBoxText.LastIndexOf('.'));
			textBox.CaretIndex = textBox.Text.Length;
		}

		// Re-subscribe to the TextChanged event
		textBox.TextChanged += ValidateFloatInputHandler;
	}

	public static void ValidateFloatInputHandler(object? sender, EventArgs e)
	{
		if (sender is TextBox textBox)
		{
			ValidateFloatInput(textBox);
		}
	}

	private static void ValidateIntegerInput(TextBox textBox)
	{
		// Unsubscribe from the TextChanged event
		textBox.TextChanged -= ValidateIntegerInputHandler;

		// Remove invalid characters (non-digits)
		var textBoxText = textBox.Text;
		if (textBoxText != null && !IntCheckRegex().IsMatch(textBoxText))
		{
			textBox.Text = Regex.Replace(textBoxText, @"\D", "");
			textBox.CaretIndex = textBoxText.Length; // Move caret to the end
		}

		// Re-subscribe to the TextChanged event
		textBox.TextChanged += ValidateIntegerInputHandler;
	}

	public static void ValidateIntegerInputHandler(object? sender, EventArgs e)
	{
		if (sender is TextBox textBox)
		{
			ValidateIntegerInput(textBox);
		}
	}

	public static Control? CreateNameField(string name, Control? content, bool vertical = false)
	{
		var grid = vertical ?
			new Grid
			{
				RowDefinitions =
				[
					new RowDefinition(GridLength.Auto),
					new RowDefinition(GridLength.Star)
				],
				HorizontalAlignment = HorizontalAlignment.Stretch,
				VerticalAlignment = VerticalAlignment.Stretch,
				Margin = new Thickness(0, 2, 0, 2)
			}
			: new Grid
			{
				ColumnDefinitions =
				[
					new ColumnDefinition(GridLength.Auto),
					new ColumnDefinition(GridLength.Star)
				],
				HorizontalAlignment = HorizontalAlignment.Stretch,
				Margin = new Thickness(0, 2, 0, 2)
			};

		if (!string.IsNullOrWhiteSpace(name))
		{
			var nameBox = new TextBlock
			{
				Text = name,
				VerticalAlignment = VerticalAlignment.Center,
				Margin = new Thickness(0, 0, 6, 0),
				MinWidth = 100
			};
			if (vertical)
			{
				Grid.SetRow(nameBox, 0);
			}
			else
			{
				Grid.SetColumn(nameBox, 0);
			}

			grid.Children.Add(nameBox);
		}

		if (content != null)
		{
			// Make content stretch in its column
			content.HorizontalAlignment = HorizontalAlignment.Stretch;
			content.VerticalAlignment = VerticalAlignment.Center;
			
			if (vertical)
			{
				Grid.SetRow(content, 1);
			}
			else
			{
				Grid.SetColumn(content, 1);
			}
			grid.Children.Add(content);
		}

		return grid;
	}

	public static IUiFabricator? GetFabricatorForType(Type type)
	{
		if (type == typeof(string)) return new StringFabricator();
		if (type == typeof(int)) return new IntFabricator();
		if (type == typeof(float)) return new FloatFabricator();
		if (type == typeof(bool)) return new BoolFabricator();
		return null;
	}

	public static string NiceString(string input)
	{
		if (string.IsNullOrEmpty(input))
			return input;

		StringBuilder result = new StringBuilder();
		result.Append(input[0]);

		for (int i = 1; i < input.Length; i++)
		{
			char current = input[i];
			char previous = input[i - 1];

			if (char.IsUpper(current))
			{
				bool previousIsSpace = previous == ' ';
				bool previousIsLower = char.IsLower(previous);
				bool nextIsLower = i + 1 < input.Length && char.IsLower(input[i + 1]);

				if (!previousIsSpace && (previousIsLower || nextIsLower))
				{
					result.Append(' ');
				}
			}

			result.Append(current);
		}

		return result.ToString();
	}
}
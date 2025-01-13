using Avalonia.Controls;
using Avalonia.Layout;
using System;
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

	public static Control? CreateNameField(string name, Control? content)
	{
		var stackPanel = new StackPanel
		{
			Orientation = Orientation.Horizontal,
			Spacing = 10
		};

		var nameBox = new TextBlock
		{
			Text = name
		};
		stackPanel.AddChild(nameBox);

		stackPanel.AddChild(content);

		return stackPanel;
	}
}
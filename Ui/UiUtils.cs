using Avalonia.Controls;

namespace GameBridge.Ui;

public static class UiUtils
{
	/// <summary>
	/// Adds content as a child of this Panel
	/// </summary>
	/// <param name="control">The Panel To add to</param>
	/// <param name="content">The content to add</param>
	/// <typeparam name="T"></typeparam>
	public static void AddChild<T>(this T control, Control? content) where T : Panel
	{
		if (content != null)
		{
			control.Children.Add(content);
		}
	}
}
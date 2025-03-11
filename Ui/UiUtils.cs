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
	
	/// <summary>
	/// Removes content as a child of this Panel
	/// </summary>
	/// <param name="control">The Panel To add to</param>
	/// <param name="content">The content to add</param>
	/// <typeparam name="T"></typeparam>
	public static void RemoveChild<T>(this T control, Control? content) where T : Panel
	{
		if (content != null)
		{
			control.Children.Remove(content);
		}
	}
	
	/// <summary>
	/// Removes content as a child of this Panel
	/// </summary>
	/// <param name="control">The Panel To add to</param>
	/// <param name="content">The content to add</param>
	/// <typeparam name="T"></typeparam>
	public static void RemoveChildAt<T>(this T control, int index) where T : Panel
	{
		if (control.Children.Count > index)
		{
			control.Children.RemoveAt(index);
		}
	}

	public static void Hide(this Control control) => control.IsVisible = false;

	public static void Show(this Control control) => control.IsVisible = true;
}
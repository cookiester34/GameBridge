using Avalonia.Controls;
using GameBridge.Ui;
using Avalonia.Layout;

namespace GameBridge.Data;

public abstract class ContentWindow : Window
{
	public WindowContext Ctx { get; private set; }
	public StackPanel StackPanel { get; private set; }

	public ContentWindow()
	{
		Ctx = new WindowContext();
		var ctxStackPanel = Ctx.DockPanel;
		Content = ctxStackPanel;

		StackPanel = new StackPanel();
	}

	public void Initialize()
	{
		AddContentToWindow(StackPanel);
	}

	public void AddContentToWindow(Control control)
	{
		Ctx.DockPanel.Children.Add(control);
	}

	public void AddContent(Control control)
	{
		StackPanel.Children.Add(control);
	}

	public void RemoveContent(Control control)
	{
		StackPanel.Children.Remove(control);
	}

	public void CenterContent()
	{
		StackPanel.HorizontalAlignment = HorizontalAlignment.Center;
		StackPanel.VerticalAlignment = VerticalAlignment.Center;
	}

	public void LeftAlignContent()
	{
		StackPanel.HorizontalAlignment = HorizontalAlignment.Left;
		StackPanel.VerticalAlignment = VerticalAlignment.Top;
	}

	public void RightAlignContent()
	{
		StackPanel.HorizontalAlignment = HorizontalAlignment.Right;
		StackPanel.VerticalAlignment = VerticalAlignment.Top;
	}
}
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;

namespace GameBridge.Ui;

public class ScrollView : ContentControl
{
	public StackPanel StackPanel { get; private set; }

	public ScrollViewer ScrollViewer { get; private set; }
	
	public Controls Children => StackPanel.Children;

	public ScrollView()
	{
		VerticalAlignment = VerticalAlignment.Top;
		
		StackPanel = new StackPanel()
		{
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Top
		};

		ScrollViewer = new ScrollViewer
		{
			Content = StackPanel,
			VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
			HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Stretch
		};

		Content = ScrollViewer;
	}

	public void AddContent(Control control)
	{
		StackPanel.AddChild(control);
	}

	public void RemoveContent(Control control)
	{
		StackPanel.RemoveChild(control);
	}
	
	public void RemoveContentAt(int index)
	{
		StackPanel.RemoveChildAt(index);
	}
}
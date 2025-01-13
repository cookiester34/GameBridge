using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using GameBridge.Ui.Extensions;

namespace GameBridge.Ui;

public class ScrollView : ResizablePanel
{
	public DockPanel DockPanel { get; private set; }

	public ScrollViewer? ScrollViewer { get; private set; }

	public ScrollView(int percentageHeight, int percentageWidth) : base(percentageHeight, percentageWidth)
	{
		DockPanel = new DockPanel()
		{
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center
		};

		ScrollViewer = new ScrollViewer
		{
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Top,
			Content = DockPanel,
			IsVisible = true,
			VerticalScrollBarVisibility = ScrollBarVisibility.Auto
		};

		Children.Add(ScrollViewer);

		SetupControl(ScrollViewer);
	}

	public void AddContent(Control? control)
	{
		DockPanel.AddChild(control);
	}
}
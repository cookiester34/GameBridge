using Avalonia.Controls;
using Avalonia.Layout;

namespace GameBridge.Ui;

public class Page : UserControl
{
	public StackPanel StackPanel { get; private set; }

	public Page(Orientation orientation = Orientation.Vertical)
	{
		StackPanel = new StackPanel
		{
			Name = "Page",
			Orientation = orientation,
			Spacing = 10
		};

		Content = StackPanel;
	}

	public void AddContent(Control? control)
	{
		StackPanel.Children.Add(control);
	}
}
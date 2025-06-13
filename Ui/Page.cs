using Avalonia.Controls;
using Avalonia.Layout;

namespace GameBridge.Ui;

public class Page : UserControl
{
	public Grid RootGrid { get; private set; }

	public Page()
	{
		RootGrid = new Grid
		{
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Stretch,
		};

		Content = RootGrid;
	}

	public void AddContent(Control? control)
	{
		if (control != null)
			RootGrid.Children.Add(control);
	}
}
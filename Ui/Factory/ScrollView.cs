using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;

namespace GameBridge.Ui
{
	public class ScrollView : ContentControl
	{
		public StackPanel StackPanel { get; private set; }

		public ScrollViewer ScrollViewer { get; private set; }

		public Controls Children => StackPanel.Children;

		public ScrollView()
		{
			HorizontalAlignment = HorizontalAlignment.Stretch;
			VerticalAlignment = VerticalAlignment.Stretch;

			// Use a Grid wrapper to constrain the height properly
			var grid = new Grid
			{
				HorizontalAlignment = HorizontalAlignment.Stretch,
				VerticalAlignment = VerticalAlignment.Stretch,
				RowDefinitions =
				{
					new RowDefinition(GridLength.Star)
				}
			};

			StackPanel = new StackPanel
			{
				Orientation = Orientation.Vertical,
				HorizontalAlignment = HorizontalAlignment.Stretch
			};

			ScrollViewer = new ScrollViewer
			{
				Content = StackPanel,
				VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
				HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
				HorizontalAlignment = HorizontalAlignment.Stretch,
				VerticalAlignment = VerticalAlignment.Stretch
			};

			Grid.SetRow(ScrollViewer, 0);
			grid.Children.Add(ScrollViewer);
			Content = grid;
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
}
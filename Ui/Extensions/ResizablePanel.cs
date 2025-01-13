using Avalonia;
using Avalonia.Controls;

namespace GameBridge.Ui.Extensions;

public abstract class ResizablePanel : Panel
{
	public int PercentageHeight { get; set; }
	public int PercentageWidth { get; set; }
	private Control? control;

	public ResizablePanel(int percentageHeight, int percentageWidth)
	{
		PercentageHeight = percentageHeight;
		PercentageWidth = percentageWidth;

		MainWindow.WindowSizeChanged += HandleWindowSizeChanged;
	}

	protected void SetupControl(Control? newControl)
	{
		control = newControl;
	}

	protected void HandleWindowSizeChanged(Size size)
	{
		if (control == null) return;
		control.MaxHeight = control.Height = size.Height * PercentageHeight / 100;
		control.MaxWidth = control.Width = size.Width * PercentageWidth / 100;
	}
}
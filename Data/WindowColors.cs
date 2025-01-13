using Avalonia.Media;

namespace GameBridge.Data;

public static class WindowColors
{
	public static IBrush TitleBarColor { get; set; } = new SolidColorBrush(Color.FromRgb(23, 23, 26));
	public static IBrush BackgroundColor { get; set; } = new SolidColorBrush(Color.FromRgb(23, 23, 26));
	public static IBrush SecondaryBackgroundColor { get; set; } = new SolidColorBrush(Color.FromRgb(35, 35, 41));
	public static IBrush TextColor { get; set; } = new SolidColorBrush(Color.FromRgb(228, 231, 236));
	public static IBrush BackgroundTextColor { get; set; } = new SolidColorBrush(Color.FromRgb(164, 166, 170));
	public static IBrush UiColor { get; set; } = new SolidColorBrush(Color.FromRgb(49, 105, 221));
	public static IBrush UiHoverColor { get; set; } = new SolidColorBrush(Color.FromRgb(34, 80, 157));
	public static IBrush SecondaryUiColor { get; set; } = new SolidColorBrush(Color.FromRgb(252, 117, 74));
	public static IBrush SecondaryUiHoverColor { get; set; } = new SolidColorBrush(Color.FromRgb(172, 77, 49));
	public static IBrush WarningUiColor { get; set; } = new SolidColorBrush(Color.FromRgb(160, 30, 54));
	public static IBrush WarningUiHoverColor { get; set; } = new SolidColorBrush(Color.FromRgb(56, 36, 44));
}
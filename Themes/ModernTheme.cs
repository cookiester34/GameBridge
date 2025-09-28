using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Themes.Fluent;
using Avalonia.Media;
using Avalonia.Styling;

namespace GameBridge.Themes;

public class ModernTheme : Styles
{
	public ModernTheme()
	{
		// Base theme (required so templates exist)
		Add(new FluentTheme());

		// ---- Palette ----
		var colBg = Color.Parse("#0E1116");
		var colPanel = Color.Parse("#161A22");
		var colUi = Color.Parse("#2A2F3A");
		var colUiHov = Color.Parse("#343A46");
		var colStroke = Color.Parse("#3E4452");
		var colText = Colors.White;
		var colMuted = Color.Parse("#AAB2C0");
		var colAccent = Color.Parse("#6EA8FE");
		var colSelected = Color.Parse("#48a7fe");
		
		// TextBox (Fluent keys)
		Resources["TextControlBackground"]               = new SolidColorBrush(colPanel);
		Resources["TextControlBackgroundPointerOver"]    = new SolidColorBrush(colUi);
		Resources["TextControlBackgroundFocused"]        = new SolidColorBrush(colUi);
		Resources["TextControlBorderBrush"]              = new SolidColorBrush(colStroke);
		Resources["TextControlBorderBrushPointerOver"]   = new SolidColorBrush(colStroke);
		Resources["TextControlBorderBrushFocused"]       = new SolidColorBrush(colAccent);
		Resources["TextControlForeground"]               = new SolidColorBrush(colText);

		// Expander header (Fluent keys)
		Resources["ExpanderHeaderBackground"]            = new SolidColorBrush(colUi);
		Resources["ExpanderHeaderBackgroundPointerOver"] = new SolidColorBrush(colUiHov);
		Resources["ExpanderHeaderBackgroundPressed"]     = new SolidColorBrush(colUiHov);

		// Expose a few brushes if you want to reuse
		Resources["Brush.Panel"] = new SolidColorBrush(colPanel);
		Resources["Brush.Stroke"] = new SolidColorBrush(colStroke);
		Resources["Brush.Accent"] = new SolidColorBrush(colAccent);
		Resources["Brush.Muted"] = new SolidColorBrush(colMuted);

		// App background
		Add(new Style(x => x.OfType<Window>())
		{
			Setters =
			{
				new Setter(TemplatedControl.BackgroundProperty, new SolidColorBrush(colBg))
			}
		});

		// ---- Core controls ----
		Add(new Style(x => x.OfType<TextBlock>())
		{
			Setters =
			{
				new Setter(TextBlock.ForegroundProperty, new SolidColorBrush(colText)),
				new Setter(TextBlock.HeightProperty, 20.0),
				new Setter(TextBlock.MinHeightProperty, 20.0),
				new Setter(TextBlock.FontSizeProperty, 12.0)
			}
		});

		// TextBox hover
		Add(new Style(x => x.OfType<TextBox>().Class(":pointerover"))
		{
			Setters =
			{
				new Setter(TextBox.BackgroundProperty, new SolidColorBrush(colUi)),
				new Setter(TextBox.BorderBrushProperty, new SolidColorBrush(colStroke))
			}
		});

		// TextBox focus
		Add(new Style(x => x.OfType<TextBox>().Class(":focus"))
		{
			Setters =
			{
				new Setter(TextBox.BorderBrushProperty, new SolidColorBrush(colAccent)),
				new Setter(TextBox.BackgroundProperty, new SolidColorBrush(colUi))
			}
		});

		Add(new Style(x => x.OfType<Button>())
		{
			Setters =
			{
				new Setter(Button.BackgroundProperty, new SolidColorBrush(colUi)),
				new Setter(Button.ForegroundProperty, Brushes.White),
				new Setter(Button.BorderBrushProperty, Brushes.Transparent),
				new Setter(Button.BorderThicknessProperty, new Thickness(0)),
				new Setter(Button.CornerRadiusProperty, new CornerRadius(3)),
				new Setter(Button.PaddingProperty, new Thickness(10, 6)),
				new Setter(Button.FontSizeProperty, 12.0),
				new Setter(Button.HorizontalAlignmentProperty, HorizontalAlignment.Center),
				new Setter(Button.VerticalAlignmentProperty, VerticalAlignment.Center),
				new Setter(Button.HorizontalContentAlignmentProperty, HorizontalAlignment.Center),
				new Setter(Button.VerticalContentAlignmentProperty, VerticalAlignment.Center)
			}
		});
		Add(new Style(x => x.OfType<Button>().Class(":pointerover"))
		{
			Setters = { new Setter(Button.BackgroundProperty, new SolidColorBrush(colUiHov)) }
		});

		Add(new Style(x => x.OfType<TextBox>())
		{
			Setters =
			{
				new Setter(TextBox.BackgroundProperty, new SolidColorBrush(colPanel)),
				new Setter(TextBox.ForegroundProperty, Brushes.White),
				new Setter(TextBox.BorderBrushProperty, new SolidColorBrush(colStroke)),
				new Setter(TextBox.BorderThicknessProperty, new Thickness(1)),
				new Setter(TextBox.CornerRadiusProperty, new CornerRadius(3)),
				new Setter(TextBox.PaddingProperty, new Thickness(8, 5)),
				new Setter(TextBox.FontSizeProperty, 12.0),
				new Setter(TextBox.MinHeightProperty, 26.0),
				new Setter(TextBox.HeightProperty, 26.0),
				new Setter(TextBox.TextWrappingProperty, TextWrapping.NoWrap)
			}
		});

		Add(new Style(x => x.OfType<ToggleSwitch>())
		{
			Setters =
			{
				new Setter(ToggleSwitch.ForegroundProperty, Brushes.White),
				new Setter(ToggleSwitch.HorizontalAlignmentProperty, HorizontalAlignment.Stretch)
			}
		});

		// ---- “Card” panels ----
		Add(new Style(x => x.OfType<Border>().Class("card"))
		{
			Setters =
			{
				new Setter(Border.BackgroundProperty, new SolidColorBrush(colPanel) { Opacity = 0.95 }),
				new Setter(Border.CornerRadiusProperty, new CornerRadius(3)),
				new Setter(Border.BorderBrushProperty, new SolidColorBrush(colStroke)),
				new Setter(Border.BorderThicknessProperty, new Thickness(1)),
				new Setter(Border.PaddingProperty, new Thickness(6, 4, 4, 4))
			}
		});

		// Expander container
		Add(new Style(x => x.OfType<Expander>())
		{
			Setters =
			{
				new Setter(Expander.BackgroundProperty, new SolidColorBrush(Color.Parse("#1E2028"))),
				new Setter(Expander.BorderBrushProperty, new SolidColorBrush(Color.Parse("#3E4452"))),
				new Setter(Expander.BorderThicknessProperty, new Thickness(1)),
				new Setter(Expander.CornerRadiusProperty, new CornerRadius(8)),
				new Setter(Expander.PaddingProperty, new Thickness(8))
			}
		});

		// Expander header
		Add(new Style(x => x.OfType<Expander>().Template().OfType<ToggleButton>())
		{
			Setters =
			{
				new Setter(ToggleButton.BackgroundProperty, new SolidColorBrush(colUiHov)),
				new Setter(ToggleButton.PaddingProperty, new Thickness(10, 6)),
				new Setter(ToggleButton.CornerRadiusProperty, new CornerRadius(6)),
				new Setter(ToggleButton.HorizontalAlignmentProperty, HorizontalAlignment.Stretch)
			}
		});

		// Expander header hover state
		Add(new Style(x => x
			.OfType<Expander>()
			.Template()
			.OfType<ToggleButton>()
			.Class(":pointerover"))
		{
			Setters =
			{
				new Setter(ToggleButton.BackgroundProperty, new SolidColorBrush(Color.Parse("#3A3D49")))
			}
		});

		// ---- Sidebar / nav buttons ----
		Add(new Style(x => x.OfType<Button>().Class("nav-btn"))
		{
			Setters =
			{
				new Setter(Button.MarginProperty, new Thickness( 3, 2, 2, 2)),
				new Setter(Button.PaddingProperty, new Thickness(8, 4, 4, 4)),
				new Setter(Button.HorizontalContentAlignmentProperty, HorizontalAlignment.Left),
				new Setter(Button.BackgroundProperty, new SolidColorBrush(Color.FromArgb(0,0,0,0))),
			}
		});
		
		Add(new Style(x => x.OfType<Button>().Class("nav-btn").Class(":pointerover"))
		{
			Setters =
			{
				new Setter(Button.BackgroundProperty, new SolidColorBrush(colUiHov))
			}
		});
		
		Add(new Style(x => x.OfType<Button>().Class("nav-btn-active"))
		{
			Setters =
			{
				new Setter(Button.BackgroundProperty, new SolidColorBrush(colUiHov))
			}
		});

		// ---- Pill buttons for CTAs ----
		Add(new Style(x => x.OfType<Button>().Class("pill"))
		{
			Setters =
			{
				new Setter(Button.CornerRadiusProperty, new CornerRadius(999)),
				new Setter(Button.PaddingProperty, new Thickness(12, 6)),
				new Setter(Button.FontSizeProperty, 12.0)
			}
		});
		Add(new Style(x => x.OfType<Button>().Class("pill").Class("primary"))
		{
			Setters =
			{
				new Setter(Button.BackgroundProperty, new SolidColorBrush(colAccent)),
				new Setter(Button.ForegroundProperty, Brushes.White)
			}
		});
		Add(new Style(x => x.OfType<Button>().Class("pill").Class(":pointerover"))
		{
			Setters = { new Setter(Button.OpacityProperty, 0.92) }
		});

		// ---- Lists / selection ----
		Add(new Style(x => x.OfType<ListBoxItem>().Class(":selected"))
		{
			Setters =
			{
				new Setter(TemplatedControl.BackgroundProperty, new SolidColorBrush(colUiHov)),
				new Setter(TemplatedControl.ForegroundProperty, Brushes.White)
			}
		});

		// ---- Slim scrollbars ----
		Add(new Style(x => x.OfType<ScrollBar>()))
			;
		Add(new Style(x => x.OfType<ScrollBar>().Class(":horizontal"))
		{
			Setters =
			{
				new Setter(RangeBase.MaximumProperty, 1d),
				new Setter(ScrollBar.HeightProperty, 6d),
			}
		});
		Add(new Style(x => x.OfType<ScrollBar>().Class(":vertical"))
		{
			Setters =
			{
				new Setter(RangeBase.MaximumProperty, 1d),
				new Setter(ScrollBar.WidthProperty, 6d),
			}
		});
		Add(new Style(x => x.OfType<Thumb>()))
			;
		Add(new Style(x => x.OfType<Thumb>().Class(":pointerover"))
		{
			Setters = { new Setter(TemplatedControl.BackgroundProperty, new SolidColorBrush(colUiHov)) }
		});
	}
}
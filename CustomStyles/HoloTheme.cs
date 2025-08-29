using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Platform;
using Avalonia.Rendering.Composition;

namespace GameBridge.CustomStyles
{
    public static class HoloTheme
    {
        public static void Register(Application application)
        {
            var resources = application.Resources;

            // Palette
            resources["Col.Surface"] = Color.Parse("#0E1116");
            resources["Col.Card"]    = Color.Parse("#141821");
            resources["Col.Accent"]  = Color.Parse("#7C3AED");
            resources["Col.Accent2"] = Color.Parse("#16A4FF");

            // Canvas gradient
            var canvasGradient = new LinearGradientBrush
            {
                StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
                EndPoint   = new RelativePoint(1, 1, RelativeUnit.Relative),
                GradientStops =
                {
                    new GradientStop(Color.Parse("#0E1116"), 0),
                    new GradientStop(Color.Parse("#151B26"), 0.4),
                    new GradientStop(Color.Parse("#1B1230"), 1)
                }
            };
            resources["Brush.CanvasGrad"] = canvasGradient;

            // “Glass” background
            var glass = new SolidColorBrush(Color.Parse("#0A0F17")) { Opacity = 0.6 };
            resources["Brush.Glass"] = glass;

            // ===== Styles =====
            var styles = application.Styles;

            // Card
            var card = new Style(x => x.OfType<Border>().Class("holo-card"));
            card.Setters.Add(new Setter(Border.BackgroundProperty, resources["Brush.Glass"]));
            card.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(12)));
            card.Setters.Add(new Setter(Border.PaddingProperty, new Thickness(12)));
            styles.Add(card);

            // Headings
            styles.Add(TextStyle(".h1", 22, FontWeight.SemiBold, 0.97));
            styles.Add(TextStyle(".h2", 14, FontWeight.Medium,   0.9));
            styles.Add(TextStyle(".micro", 11, FontWeight.Regular, 0.7));

            // Pill buttons
            var pill = new Style(x => x.OfType<Button>().Class("pill"));
            pill.Setters.Add(new Setter(Button.CornerRadiusProperty, new CornerRadius(999)));
            pill.Setters.Add(new Setter(Button.PaddingProperty, new Thickness(10, 6)));
            pill.Setters.Add(new Setter(TemplatedControl.FontSizeProperty, 12.0));
            styles.Add(pill);

            var primary = new Style(x => x.OfType<Button>().Class("pill").Class("primary"));
            primary.Setters.Add(new Setter(TemplatedControl.ForegroundProperty, Brushes.White));
            primary.Setters.Add(new Setter(TemplatedControl.BackgroundProperty, Gradient(resources)));
            styles.Add(primary);

            var ghost = new Style(x => x.OfType<Button>().Class("pill").Class("ghost"));
            ghost.Setters.Add(new Setter(TemplatedControl.ForegroundProperty, Brushes.White));
            ghost.Setters.Add(new Setter(TemplatedControl.BackgroundProperty, new SolidColorBrush(Colors.White){ Opacity = 0.1 }));
            ghost.Setters.Add(new Setter(Button.BorderBrushProperty, new SolidColorBrush(Colors.White){ Opacity = 0.2 }));
            ghost.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(1)));
            styles.Add(ghost);

            // Sidebar link
            var sidebar = new Style(x => x.OfType<Button>().Class("sidebar-link"));
            sidebar.Setters.Add(new Setter(Button.HorizontalAlignmentProperty, Avalonia.Layout.HorizontalAlignment.Left));
            sidebar.Setters.Add(new Setter(Button.BackgroundProperty, Brushes.Transparent));
            sidebar.Setters.Add(new Setter(TemplatedControl.ForegroundProperty, new SolidColorBrush(Color.Parse("#CCE0FF"))));
            sidebar.Setters.Add(new Setter(TemplatedControl.PaddingProperty, new Thickness(8,4)));
            sidebar.Setters.Add(new Setter(TemplatedControl.FontSizeProperty, 12.0));
            styles.Add(sidebar);

            var sidebarHover = new Style(x => x.OfType<Button>().Class("sidebar-link").Property(Interactive.IsPointerOverProperty, true));
            sidebarHover.Setters.Add(new Setter(TemplatedControl.ForegroundProperty, Brushes.White));
            styles.Add(sidebarHover);

            // Tabs
            var tab = new Style(x => x.OfType<ToggleButton>().Class("holo-tab"));
            tab.Setters.Add(new Setter(ToggleButton.BackgroundProperty, Brushes.Transparent));
            tab.Setters.Add(new Setter(TemplatedControl.ForegroundProperty, new SolidColorBrush(Color.Parse("#BBD2FF"))));
            tab.Setters.Add(new Setter(TemplatedControl.PaddingProperty, new Thickness(10,6)));
            tab.Setters.Add(new Setter(TemplatedControl.FontSizeProperty, 12.0));
            tab.Setters.Add(new Setter(ToggleButton.CornerRadiusProperty, new CornerRadius(999)));
            tab.Setters.Add(new Setter(ToggleButton.BorderBrushProperty, Brushes.Transparent));
            styles.Add(tab);

            var tabChecked = new Style(x => x.OfType<ToggleButton>().Class("holo-tab").Property(ToggleButton.IsCheckedProperty, true));
            tabChecked.Setters.Add(new Setter(ToggleButton.BackgroundProperty, new SolidColorBrush(Colors.White){ Opacity = 0.13 }));
            tabChecked.Setters.Add(new Setter(TemplatedControl.ForegroundProperty, Brushes.White));
            tabChecked.Setters.Add(new Setter(ToggleButton.BorderBrushProperty, new SolidColorBrush(Colors.White){ Opacity = 0.27 }));
            tabChecked.Setters.Add(new Setter(ToggleButton.BorderThicknessProperty, new Thickness(1)));
            styles.Add(tabChecked);

            // Icon button
            var icon = new Style(x => x.OfType<Button>().Class("icon"));
            icon.Setters.Add(new Setter(Button.BackgroundProperty, Brushes.Transparent));
            icon.Setters.Add(new Setter(TemplatedControl.PaddingProperty, new Thickness(4)));
            icon.Setters.Add(new Setter(Button.BorderBrushProperty, Brushes.Transparent));
            icon.Setters.Add(new Setter(TemplatedControl.ForegroundProperty, new SolidColorBrush(Color.Parse("#9BB0C8"))));
            styles.Add(icon);

            var iconHover = new Style(x => x.OfType<Button>().Class("icon").Property(Interactive.IsPointerOverProperty, true));
            iconHover.Setters.Add(new Setter(TemplatedControl.ForegroundProperty, Brushes.White));
            styles.Add(iconHover);

            // Compact TextBox
            var text = new Style(x => x.OfType<TextBox>().Class("holo"));
            text.Setters.Add(new Setter(TemplatedControl.FontSizeProperty, 12.0));
            text.Setters.Add(new Setter(TemplatedControl.PaddingProperty, new Thickness(8,5)));
            text.Setters.Add(new Setter(TextBox.CornerRadiusProperty, new CornerRadius(8)));
            text.Setters.Add(new Setter(TextBox.BackgroundProperty, new SolidColorBrush(Color.Parse("#171C24"))));
            text.Setters.Add(new Setter(TextBox.BorderBrushProperty, new SolidColorBrush(Color.Parse("#262C37"))));
            text.Setters.Add(new Setter(TextBox.BorderThicknessProperty, new Thickness(1)));
            styles.Add(text);
        }

        static IBrush Gradient(ResourceDictionary r)
        {
            var brush = new LinearGradientBrush
            {
                StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
                EndPoint   = new RelativePoint(1, 0, RelativeUnit.Relative),
                GradientStops =
                {
                    new GradientStop((Color)r["Col.Accent2"], 0),
                    new GradientStop((Color)r["Col.Accent"], 1)
                }
            };
            return brush;
        }

        static Style TextStyle(string cls, double size, FontWeight weight, double opacity)
        {
            var style = new Style(x => x.OfType<TextBlock>().Class(cls.TrimStart('.')));
            style.Setters.Add(new Setter(TextBlock.FontSizeProperty, size));
            style.Setters.Add(new Setter(TextBlock.FontWeightProperty, weight));
            style.Setters.Add(new Setter(Visual.OpacityProperty, opacity));
            return style;
        }
    }
}
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using GameBridge.Data;
using GameBridge.Ui.Extensions;

namespace GameBridge.Ui;

public class Section : ResizablePanel
{
	public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<Section, string>(nameof(Title));

    public static readonly StyledProperty<string> DescriptionProperty =
        AvaloniaProperty.Register<Section, string>(nameof(Description));

    public StackPanel ContentContainer { get; private set; }

    // Property to get/set Title
    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    // Property to get/set Description
    public string Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public Section(int percentageHeight, int percentageWidth, string? title = null, string? description = null) : base(percentageHeight, percentageWidth)
    {
        var border = new Border
        {
            Background = WindowColors.SecondaryBackgroundColor,
            // BorderBrush = ,
            BorderThickness = new Thickness(2),
            CornerRadius = new CornerRadius(10),
            Padding = new Thickness(10),
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        var stackPanel = new StackPanel
        {
            Orientation = Orientation.Vertical,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Spacing = 8
        };

        //TODO: instead of not adding it, just hide it depending on value
        if (title != null)
        {
            var titleTextBlock = new TextBlock
            {
                Text = "Title",
                FontWeight = FontWeight.Bold,
                FontSize = 16,
                Foreground = WindowColors.TextColor
            };
            Title = title;
            titleTextBlock.Bind(TextBlock.TextProperty, new Binding(nameof(Title)) { Source = this });
            stackPanel.Children.Add(titleTextBlock);
        }

        //TODO: instead of not adding it, just hide it depending on value
        if (description != null)
        {
            var descriptionTextBlock = new TextBlock
            {
                Text = "Optional Description",
                FontSize = 14,
                Foreground = WindowColors.BackgroundTextColor
            };
            descriptionTextBlock.Bind(TextBlock.TextProperty, new Binding(nameof(Description)) { Source = this });
            Description = description;
            stackPanel.Children.Add(descriptionTextBlock);
        }

        // Add a content container to allow adding more UI elements like buttons and text
        ContentContainer = new StackPanel
        {
            Orientation = Orientation.Vertical,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Spacing = 8
        };

        stackPanel.Children.Add(ContentContainer);

        border.Child = stackPanel;
        Children.Add(border);
    }

    public void AddContent(Control control)
    {
        ContentContainer.Children.Add(control);
    }
}
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using GameBridge.Data;

namespace GameBridge.Ui;

public class Section : Panel
{
	public static readonly StyledProperty<string> TitleProperty =
		AvaloniaProperty.Register<Section, string>(nameof(Title));

	public static readonly StyledProperty<string> DescriptionProperty =
		AvaloniaProperty.Register<Section, string>(nameof(Description));

	public StackPanel ContentContainer { get; private set; }

	public string Title
	{
		get => GetValue(TitleProperty);
		set => SetValue(TitleProperty, value);
	}

	public string Description
	{
		get => GetValue(DescriptionProperty);
		set => SetValue(DescriptionProperty, value);
	}

	public Section(string title = null, string? description = null)
	{
		Width = double.NaN;
		Height = double.NaN;
		HorizontalAlignment = HorizontalAlignment.Stretch;
		VerticalAlignment = VerticalAlignment.Stretch;

		var border = new Border
		{
			Background = WindowColors.SecondaryBackgroundColor,
			BorderThickness = new Thickness(2),
			CornerRadius = new CornerRadius(10),
			Padding = new Thickness(10),
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Top,
			Width = double.NaN
		};

		var stackPanel = new StackPanel
		{
			Orientation = Orientation.Vertical,
			HorizontalAlignment = HorizontalAlignment.Stretch,
			Spacing = 8,
			Width = double.NaN
		};

		if (title != null)
		{
			var titleTextBlock = new TextBlock
			{
				Text = "Title",
				FontWeight = FontWeight.Bold,
				FontSize = 16,
				Foreground = WindowColors.TextColor,
				HorizontalAlignment = HorizontalAlignment.Stretch
			};
			Title = title;
			titleTextBlock.Bind(TextBlock.TextProperty, new Binding(nameof(Title)) { Source = this });
			stackPanel.Children.Add(titleTextBlock);
		}

		if (description != null)
		{
			var descriptionTextBlock = new TextBlock
			{
				Text = "Optional Description",
				FontSize = 14,
				Foreground = WindowColors.BackgroundTextColor,
				HorizontalAlignment = HorizontalAlignment.Stretch
			};
			descriptionTextBlock.Bind(TextBlock.TextProperty, new Binding(nameof(Description)) { Source = this });
			Description = description;
			stackPanel.Children.Add(descriptionTextBlock);
		}

		ContentContainer = new StackPanel
		{
			Orientation = Orientation.Vertical,
			HorizontalAlignment = HorizontalAlignment.Stretch,
			Spacing = 8,
			Width = double.NaN
		};

		stackPanel.Children.Add(ContentContainer);
		border.Child = stackPanel;
		Children.Add(border);
	}

	public void AddContent(Control control)
	{
		control.HorizontalAlignment = HorizontalAlignment.Stretch;
		control.Width = double.NaN;
		ContentContainer.Children.Add(control);
	}
}
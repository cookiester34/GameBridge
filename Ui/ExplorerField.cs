using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Platform.Storage;
using GameBridge.Data;
using System;

namespace GameBridge.Ui
{
    public class ExplorerField : UserControl
    {
        public string? Text
        {
            get => filePathTextBox.Text;
            set => filePathTextBox.Text = value;
        }

        private readonly TextBox filePathTextBox;

        public event EventHandler<TextChangedEventArgs>? TextChanged;

        public ExplorerField(string title, string? path = null, PathType pathAttributePathType = PathType.FilePath)
        {
            // File path input
            filePathTextBox = new TextBox
            {
                Watermark = "Path...",
                Height = 25,
                Text = path ?? string.Empty,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            filePathTextBox.TextChanged += (o, args) => TextChanged?.Invoke(o, args);

            // Browse button
            var browseButton = new Button
            {
                Content = "Browse",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = WindowColors.TextColor,
                Background = WindowColors.UiColor,
                CornerRadius = new CornerRadius(3),
                Padding = new Thickness(5),
                Margin = new Thickness(5,0,0,0),
                Width = 60
            };

            browseButton.Click += async (sender, e) =>
            {
                var storageProvider = TopLevel.GetTopLevel(this)?.StorageProvider;
                if (storageProvider == null) return;

                // Open file dialog
                var fileResult = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    AllowMultiple = false,
                    Title = "Select a File"
                });

                if (fileResult.Count > 0)
                {
                    filePathTextBox.Text = fileResult[0].Path.LocalPath;
                }
            };

            // Combine TextBox and Button in a horizontal layout using Grid
            var filePathInput = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                ColumnDefinitions =
                {
                    new ColumnDefinition(new GridLength(80)),          // Title
                    new ColumnDefinition(GridLength.Star),          // TextBox (expands to fill available space)
                    new ColumnDefinition(new GridLength(60))           // Button
                }
            };

            // Title TextBlock
            var titleBlock = new TextBlock
            {
                Text = title,
                Foreground = WindowColors.TextColor,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 80
            };
            Grid.SetColumn(titleBlock, 0);
            filePathInput.Children.Add(titleBlock);

            // File path TextBox
            Grid.SetColumn(filePathTextBox, 1);
            filePathInput.Children.Add(filePathTextBox);

            // Browse Button
            Grid.SetColumn(browseButton, 2);
            filePathInput.Children.Add(browseButton);

            // Set the content to the Grid
            Content = filePathInput;
        }
    }
}

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
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Center;
            
            filePathTextBox = new TextBox
            {
                Watermark = "Path...",
                Height = 25,
                Text = path ?? string.Empty,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                Width = double.NaN
            };

            filePathTextBox.TextChanged += (o, args) => TextChanged?.Invoke(o, args);

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

                if (pathAttributePathType == PathType.FilePath)
                {
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
                }
                else
                {
                    var directoryResult = await storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
                    {
                        AllowMultiple = false,
                        Title = "Select a Directory"
                    });

                    if (directoryResult.Count > 0)
                    {
                        filePathTextBox.Text = directoryResult[0].Path.LocalPath;
                    }
                }
            };

            var filePathInput = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                ColumnDefinitions = 
                {
                    new ColumnDefinition(GridLength.Auto),
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Auto)
                },
                Margin = new Thickness(0, 0, 0, 0)
            };

            if (!string.IsNullOrWhiteSpace(title))
            {
                var titleBlock = new TextBlock
                {
                    Text = title,
                    Foreground = WindowColors.TextColor,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 6, 0),
                    MinWidth = 100
                };
                Grid.SetColumn(titleBlock, 0);
                filePathInput.AddChild(titleBlock);
            }

            filePathTextBox.Margin = new Thickness(0, 0, 10, 0);
            Grid.SetColumn(filePathTextBox, 1);
            filePathInput.AddChild(filePathTextBox);

            Grid.SetColumn(browseButton, 2);
            filePathInput.AddChild(browseButton);

            Content = filePathInput;
        }
    }
}

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using GameBridge.Data;
using System;
using System.Collections.Generic;

namespace GameBridge.Ui
{
    public class PageNavigator : Grid
    {
        private readonly Dictionary<string, PageArg> pages = new();
        private readonly StackPanel topButtonsPanel;
        private readonly StackPanel bottomButtonsPanel;
        
        private Dictionary<string, Button> buttons = new();
        private Page? activePage;
        private Button? activeButton;

        public PageNavigator()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;

            ColumnDefinitions.Add(new ColumnDefinition(new GridLength(150)));
            ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

            var sidebarGrid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(GridLength.Star),
                    new RowDefinition(GridLength.Auto)
                },
                Background = WindowColors.SecondaryBackgroundColor
            };

            var sidebarBorder = new Border
            {
                CornerRadius = new CornerRadius(0, 6, 0, 0),
                Margin = new Thickness(0, 2, 0, 0),
                Background = WindowColors.SecondaryBackgroundColor,
                Child = sidebarGrid,
                ClipToBounds = true,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            SetColumn(sidebarBorder, 0);
            Children.Add(sidebarBorder);

            topButtonsPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
            };
            SetRow(topButtonsPanel, 0);
            sidebarGrid.Children.Add(topButtonsPanel);

            var spacer = new Border();
            SetRow(spacer, 1);
            sidebarGrid.Children.Add(spacer);

            bottomButtonsPanel = new StackPanel
            {
                Orientation = Orientation.Vertical
            };
            SetRow(bottomButtonsPanel, 2);
            sidebarGrid.Children.Add(bottomButtonsPanel);
        }
        
        public void AddTitle(string title, bool alignTop = true)
        {
            var button = new TextBlock()
            {
                Text = title,
                FontWeight = FontWeight.Medium,
                FontSize = 14,
                Padding = new Thickness(9, 0, 4, 4),
                Margin = new Thickness(1),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Foreground = new SolidColorBrush(Color.Parse("#8c8c8c"))
            };

            var targetPanel = alignTop ? topButtonsPanel : bottomButtonsPanel;
            targetPanel.Children.Add(button);
        }

        public void AddPage(string name, Page page, bool alignTop = true)
        {
            if (!pages.TryAdd(name, new PageArg { Page = page }))
                throw new Exception("Page already exists in page navigator");

            var button = new Button
            {
                Content = name,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Classes = { "nav-btn" }
            };
            buttons[name] = button;
            button.Click += (_, _) => SwitchPage(name);

            pages[name] = new PageArg
            {
                Page = page,
                NavigationButton = button,
                IsTopAligned = alignTop
            };

            var targetPanel = alignTop ? topButtonsPanel : bottomButtonsPanel;
            targetPanel.Children.Add(button);

            if (activePage == null)
                SwitchPage(name);
        }

        public void RemovePage(string name)
        {
            if (!pages.TryGetValue(name, out var pageArg)) return;
            
            var panel = pageArg.IsTopAligned ? topButtonsPanel : bottomButtonsPanel;
            panel.Children.Remove(pageArg.NavigationButton);

            if (pageArg.Page == activePage)
            {
                Children.Remove(activePage);
                activePage = null;
            }

            pages.Remove(name);
        }

        public void SwitchPage(string name)
        {
            if (!pages.TryGetValue(name, out var pageArg)) return;

            if (activePage != null)
                Children.Remove(activePage);

            activePage = pageArg.Page;
            activePage.HorizontalAlignment = HorizontalAlignment.Stretch;
            activePage.VerticalAlignment = VerticalAlignment.Stretch;

            SetColumn(activePage, 1);
            Children.Add(activePage);

            activeButton?.Classes.Remove("nav-btn-active");
            activeButton = buttons[name];
            activeButton.Classes.Add("nav-btn-active");
        }

        private struct PageArg
        {
            public Page Page { get; set; }
            public Button NavigationButton { get; set; }
            public bool IsTopAligned { get; set; }
        }
    }
}
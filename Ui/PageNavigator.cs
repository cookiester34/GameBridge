using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
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
        private Page? activePage;

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
                CornerRadius = new CornerRadius(0, 10, 0, 0),
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

        public void AddPage(string name, Page page, bool alignTop = true)
        {
            if (!pages.TryAdd(name, new PageArg { Page = page }))
                throw new Exception("Page already exists in page navigator");

            var button = new Button
            {
                Content = name,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0),
                Padding = new Thickness(6, 4),
                Background = WindowColors.SecondaryBackgroundColor,
                BorderThickness = new Thickness(0),
                CornerRadius = new CornerRadius(0)
            };

            button.Click += (_, _) => SwitchPage(name);

            pages[name] = new PageArg
            {
                Page = page,
                NavigationButton = button,
                IsTopAligned = alignTop
            };

            if (alignTop)
            {
                if (topButtonsPanel.Children.Count == 0)
                {
                    button.CornerRadius = new CornerRadius(0, 10, 0, 0);
                }
                topButtonsPanel.Children.Add(button);
            }
            else
            {
                bottomButtonsPanel.Children.Add(button);
            }
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
        }

        private struct PageArg
        {
            public Page Page { get; set; }
            public Button NavigationButton { get; set; }
            public bool IsTopAligned { get; set; }
        }
    }
}
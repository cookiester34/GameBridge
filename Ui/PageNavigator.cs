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
        private readonly StackPanel pageButtonPanel;
        private Page? activePage;

        public PageNavigator()
        {
            // Setup the overall layout
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
            Width = double.NaN;
            Height = double.NaN;

            // Define columns: fixed-width left (150px), stretch right
            ColumnDefinitions.Add(new ColumnDefinition(new GridLength(150)));  // Tab area
            ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));      // Page content area

            // Create the vertical tab button panel
            pageButtonPanel = new StackPanel
            {
                Background = WindowColors.SecondaryBackgroundColor,
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            Grid.SetColumn(pageButtonPanel, 0);
            Children.Add(pageButtonPanel);
        }

        public void AddPage(string name, Page page)
        {
            if (!pages.TryAdd(name, new PageArg { Page = page }))
                throw new Exception("Page already exists in page navigator");

            var button = new Button
            {
                Content = name,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(3)
            };

            button.Click += (_, _) => SwitchPage(name);

            pages[name] = new PageArg
            {
                Page = page,
                NavigationButton = button
            };

            pageButtonPanel.Children.Add(button);
        }

        public void RemovePage(string name)
        {
            if (pages.TryGetValue(name, out var pageArg))
            {
                pageButtonPanel.Children.Remove(pageArg.NavigationButton);
                if (pageArg.Page == activePage)
                {
                    Children.Remove(activePage);
                    activePage = null;
                }
                pages.Remove(name);
            }
        }

        public void SwitchPage(string name)
        {
            if (!pages.TryGetValue(name, out var pageArg))
                return;

            if (activePage != null)
                Children.Remove(activePage);

            activePage = pageArg.Page;
            activePage.HorizontalAlignment = HorizontalAlignment.Stretch;
            activePage.VerticalAlignment = VerticalAlignment.Stretch;

            Grid.SetColumn(activePage, 1);
            Children.Add(activePage);
        }

        private struct PageArg
        {
            public Page Page { get; set; }
            public Button NavigationButton { get; set; }
        }
    }
}
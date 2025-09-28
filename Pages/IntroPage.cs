using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using GameBridge.Data;
using GameBridge.Data.EngineData;
using GameBridge.Ui;
using GameBridge.Ui.Factory;
using System;

namespace GameBridge.Pages;

public class IntroPage : Page
{
    private int currentStep = 0;
    private bool isGoingBack = false;

    private readonly StackPanel contentPanel;
    private readonly Button nextButton;
    private readonly Button backButton;
    private readonly UserData userData;
    private readonly Button finishButton;

    public event Action OnSetupComplete;

    public IntroPage()
    {
        userData = DataManager.UserData;

        var layout = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition(GridLength.Star),
                new RowDefinition(GridLength.Auto)
            },
            Margin = new Thickness(20)
        };

        contentPanel = new StackPanel
        {
            Spacing = 12,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Top
        };

        var scrollViewer = new ScrollViewer
        {
            Content = contentPanel,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled
        };

        Grid.SetRow(scrollViewer, 0);
        layout.Children.Add(scrollViewer);

        backButton = new Button
        {
            Content = "Back",
            HorizontalAlignment = HorizontalAlignment.Right,
            IsEnabled = false
        };
        backButton.Click += (_, _) => PreviousStep();

        nextButton = new Button
        {
            Content = "Next",
            HorizontalAlignment = HorizontalAlignment.Right
        };
        nextButton.Click += (_, _) => NextStep();

        finishButton = new Button
        {
            Content = "Finish Setup",
            HorizontalAlignment = HorizontalAlignment.Right,
            IsVisible = false
        };
        finishButton.Click += (_, _) => FinishSetup();

        var buttonPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Right,
            Spacing = 10,
            Margin = new Thickness(0, 10, 0, 0)
        };
        buttonPanel.Children.Add(backButton);
        buttonPanel.Children.Add(nextButton);
        buttonPanel.Children.Add(finishButton);

        Grid.SetRow(buttonPanel, 1);
        layout.Children.Add(buttonPanel);

        AddContent(layout);
        ShowStep(0);
    }

    private void NextStep()
    {
        isGoingBack = false;
        currentStep++;
        ShowStep(currentStep);
    }

    private void PreviousStep()
    {
        isGoingBack = true;
        currentStep = Math.Max(0, currentStep - 1);
        ShowStep(currentStep);
    }

    private void SkipStep()
    {
        currentStep += isGoingBack ? -1 : 1;
        ShowStep(currentStep);
    }

    private void ShowStep(int step)
    {
        contentPanel.Children.Clear();
        backButton.IsEnabled = step > 0;
        nextButton.IsVisible = step < 6;
        finishButton.IsVisible = step == 6;

        switch (step)
        {
            case 0:
                ShowSaveDirectoryStep();
                break;
            case 1:
                ShowEngineSelection();
                break;
            case 2:
                if (userData.UnitySettings.IsEnabled) ShowUnityInstallStep(); else SkipStep();
                break;
            case 3:
                if (userData.UnitySettings.IsEnabled) ShowUnityProjectStep(); else SkipStep();
                break;
            case 4:
                if (userData.UnrealSettings.IsEnabled) ShowUnrealInstallStep(); else SkipStep();
                break;
            case 5:
                if (userData.UnrealSettings.IsEnabled) ShowUnrealProjectStep(); else SkipStep();
                break;
            case 6:
                ShowFinishedStep();
                break;
            default:
                ShowStep(0);
                break;
        }
    }

    private void ShowSaveDirectoryStep()
    {
        AddHeader("Save Directory", "Choose where GameBridge stores its data");

        AddField(userData, nameof(UserData.GameBridgeSaveDirectory));
    }

    private void ShowEngineSelection()
    {
        AddHeader("Engine Selection", "Which engines do you use?");

        var unityCheck = new CheckBox { Content = "Unity", IsChecked = userData.UnitySettings.IsEnabled };
        unityCheck.Checked += (_, _) => userData.UnitySettings.IsEnabled = true;
        unityCheck.Unchecked += (_, _) => userData.UnitySettings.IsEnabled = false;

        var unrealCheck = new CheckBox { Content = "Unreal", IsChecked = userData.UnrealSettings.IsEnabled };
        unrealCheck.Checked += (_, _) => userData.UnrealSettings.IsEnabled = true;
        unrealCheck.Unchecked += (_, _) => userData.UnrealSettings.IsEnabled = false;

        contentPanel.Children.Add(unityCheck);
        contentPanel.Children.Add(unrealCheck);
    }

    private void ShowUnityInstallStep()
    {
        AddHeader("Unity Engine Install Directories", "Select scan folders and optionally add installs manually");

        var detectDefaultButton = new Button
        {
            Content = "Add Unity Hub default install path",
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(0, 0, 0, 6)
        };

        detectDefaultButton.Click += (_, _) =>
        {
            var defaultPath = @"C:\Program Files\Unity\Hub\Editor";
            if (HasValidUnityInstalls(defaultPath) &&
                !userData.UnitySettings.InstallScanPaths.Contains(defaultPath))
            {
                userData.UnitySettings.InstallScanPaths.Add(defaultPath);
                DataManager.SaveData();
                ShowStep(currentStep);
            }
            else
            {
                Console.WriteLine("Failed to install Unity Hub default install path");
            }
        };

        contentPanel.Children.Add(detectDefaultButton);

        AddField(userData.UnitySettings, nameof(UnityEngineSettings.InstallScanPaths));
        contentPanel.Children.Add(new ToggleOptionalField(userData.UnitySettings, nameof(UnityEngineSettings.IndividualEngineInstallPaths), "Add custom install paths"));
    }

    private void ShowUnityProjectStep()
    {
        AddHeader("Unity Project Directories", "Select scan folders and optionally add projects manually");

        AddField(userData.UnitySettings, nameof(UnityEngineSettings.ProjectScanPaths));
        contentPanel.Children.Add(new ToggleOptionalField(userData.UnitySettings, nameof(UnityEngineSettings.IndividualProjectPaths), "Add individual Unity projects"));
    }

    private void ShowUnrealInstallStep()
    {
        AddHeader("Unreal Engine Install Directories", "Select scan folders and optionally add installs manually");

        var detectDefaultButton = new Button
        {
            Content = "Add Epic Games default install path",
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(0, 0, 0, 6)
        };

        detectDefaultButton.Click += (_, _) =>
        {
            var defaultPath = @"C:\Program Files\Epic Games";
            if (HasValidUnrealInstalls(defaultPath) &&
                !userData.UnrealSettings.InstallScanPaths.Contains(defaultPath))
            {
                userData.UnrealSettings.InstallScanPaths.Add(defaultPath);
                DataManager.SaveData();
                ShowStep(currentStep);
            }
        };

        contentPanel.Children.Add(detectDefaultButton);

        AddField(userData.UnrealSettings, nameof(UnrealEngineSettings.InstallScanPaths));
        contentPanel.Children.Add(new ToggleOptionalField(userData.UnrealSettings, nameof(UnrealEngineSettings.IndividualEngineInstallPaths), "Add custom install paths"));
    }

    private void ShowUnrealProjectStep()
    {
        AddHeader("Unreal Project Directories", "Select scan folders and optionally add projects manually");

        AddField(userData.UnrealSettings, nameof(UnrealEngineSettings.ProjectScanPaths));
        contentPanel.Children.Add(new ToggleOptionalField(userData.UnrealSettings, nameof(UnrealEngineSettings.IndividualProjectPaths), "Add individual Unreal projects"));
    }

    private void ShowFinishedStep()
    {
        AddHeader("Setup Complete!", "You're ready to use GameBridge.");

        contentPanel.Children.Add(new TextBlock
        {
            Text = "You can now switch to the engine pages or settings at any time from the sidebar."
        });
    }

    private void AddHeader(string title, string subtitle)
    {
        var headerPanel = new StackPanel
        {
            Spacing = 2,
            Margin = new Thickness(0, 0, 0, 6)
        };

        headerPanel.Children.Add(new TextBlock
        {
            Text = title,
            FontSize = 20,
            FontWeight = FontWeight.Bold
        });

        headerPanel.Children.Add(new TextBlock
        {
            Text = subtitle,
            FontSize = 14,
            Foreground = Brushes.Gray
        });

        contentPanel.Children.Add(headerPanel);
    }

    private void AddField(object target, string propertyName)
    {
        var prop = target.GetType().GetProperty(propertyName);
        if (prop == null) return;

        var ui = prop.CreateAndBindUi(target);
        if (ui != null)
        {
            contentPanel.Children.Add(ui);
        }
    }

    private void FinishSetup()
    {
        DataManager.SaveData();
        OnSetupComplete?.Invoke();
    }

    private bool HasValidUnityInstalls(string root)
    {
        if (!System.IO.Directory.Exists(root))
            return false;

        foreach (var versionDir in System.IO.Directory.GetDirectories(root))
        {
            var exe = System.IO.Path.Combine(versionDir, "Editor", "Unity.exe");
            if (System.IO.File.Exists(exe)) return true;
        }

        return false;
    }

    private bool HasValidUnrealInstalls(string root)
    {
        if (!System.IO.Directory.Exists(root))
            return false;

        foreach (var versionDir in System.IO.Directory.GetDirectories(root))
        {
            var win64Dir = System.IO.Path.Combine(versionDir, "Engine", "Binaries", "Win64");
            if (System.IO.File.Exists(System.IO.Path.Combine(win64Dir, "UE4Editor.exe")) ||
                System.IO.File.Exists(System.IO.Path.Combine(win64Dir, "UnrealEditor.exe")))
                return true;
        }

        return false;
    }

    private class ToggleOptionalField : StackPanel
    {
        private readonly CheckBox toggle;
        private readonly Control? fieldControl;

        public ToggleOptionalField(object target, string propertyName, string label)
        {
            Orientation = Orientation.Vertical;
            Spacing = 4;

            toggle = new CheckBox
            {
                Content = label,
                IsChecked = false
            };

            var prop = target.GetType().GetProperty(propertyName);
            fieldControl = prop?.CreateAndBindUi(target);
            if (fieldControl != null)
                fieldControl.IsVisible = false;

            toggle.Checked += (_, _) => { if (fieldControl != null) fieldControl.IsVisible = true; };
            toggle.Unchecked += (_, _) => { if (fieldControl != null) fieldControl.IsVisible = false; };

            Children.Add(toggle);
            if (fieldControl != null) Children.Add(fieldControl);
        }
    }
}
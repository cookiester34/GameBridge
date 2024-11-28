using Avalonia.Controls;
using ProjectBridge.Code;

namespace ProjectBridge.Views;

public partial class EngineDataPage : UserControl
{
	public EngineDataPage()
	{
		InitializeComponent();
		SetupEngineSpecificText();
	}
	
	private void SetupEngineSpecificText()
	{
		var currentPage = UiDataManager.CurrentPage;
		SetupEngineInstallsButton.Content = $"Setup {currentPage} Engine Installs Path!";
		SetupEngineProjectsButton.Content = $"Setup {currentPage} Engine Projects Path!";
	}
}
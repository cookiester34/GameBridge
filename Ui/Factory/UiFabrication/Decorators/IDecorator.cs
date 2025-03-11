using Avalonia.Controls;

namespace GameBridge.Ui.Factory.UiFabrication.Decorators;

public interface IDecorator
{
	public bool IsTopDecorator { get; set; }
	public Control CreateDecorator(object? targetInstance);
}
using Avalonia.Controls;
using System;
using System.Reflection;

namespace GameBridge.Ui.Factory.UiFabrication.Decorators;

public class ButtonAttribute : Attribute,  IDecorator
{
	public string MethodName { get; }
	
	public ButtonAttribute(string methodName)
	{
		MethodName = methodName;
	}

	public bool IsTopDecorator { get; set; } = false;

	public Control CreateDecorator(object? targetInstance)
	{
		// Get type from the instance containing the attributed member
		var targetType = targetInstance?.GetType() 
		               ?? throw new ArgumentNullException(nameof(targetInstance), 
			               "Target instance required for type inference");

		var method = targetType.GetMethod(MethodName,
			BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
			BindingFlags.Static | BindingFlags.FlattenHierarchy);

		ValidateMethod(method, targetType);

		var button = new Button
		{
			Content = FactoryHelpers.NiceString(MethodName),
		};

		button.Click += (_, _) =>
		{
			// For static methods, pass null as target instance
			method?.Invoke(method.IsStatic ? null : targetInstance, Array.Empty<object?>());
		};

		return button;
	}
	
	private void ValidateMethod(MethodInfo? method, Type targetType)
	{
		if (method == null)
			throw new MissingMethodException($"Method {MethodName} not found in {targetType.Name}");
	}
}
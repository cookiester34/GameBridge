using System;
using System.Reflection;

namespace GameBridge.Ui.Factory.UiFabrication.DataBinder;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class OnValueChangedAttribute : Attribute, IDataBinder
{
	public string MethodName { get; }
	
	private Type? targetType;
	private MethodInfo? method;

	public OnValueChangedAttribute(string methodName)
	{
		MethodName = methodName;
	}

	public void OnValueChange(object data, object? targetInstance)
	{
		// Get type from the instance containing the attributed member
		targetType ??= targetInstance?.GetType() 
		             ?? throw new ArgumentNullException(nameof(targetInstance), 
			             "Target instance required for type inference");

		if (method == null)
		{
			method = targetType.GetMethod(MethodName,
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
				BindingFlags.Static | BindingFlags.FlattenHierarchy);

			ValidateMethod(method, targetType, data.GetType());
		}
        
		// For static methods, pass null as target instance
		method?.Invoke(method.IsStatic ? null : targetInstance, new[] { data });
	}

	private void ValidateMethod(MethodInfo? method, Type targetType, Type dataType)
	{
		if (method == null)
			throw new MissingMethodException($"Method {MethodName} not found in {targetType.Name}");

		var parameters = method.GetParameters();
		if (parameters.Length != 1 || !parameters[0].ParameterType.IsAssignableFrom(dataType))
			throw new ArgumentException(
				$"Method {MethodName} must have a single parameter of type {dataType.Name}");
	}
}
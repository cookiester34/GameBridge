using Avalonia.Controls;
using System;
using System.Reflection;

namespace GameBridge.Ui.Factory;

public static partial class UiFactory
{
	public static Control? ProcessClass<T>(T input, Type? givenType = null)
	{
		var control = new Section();

		if (input == null) return control;

		var type = givenType ?? typeof(T);
		var isClass = type.IsClass;

		if (!isClass) return control;

		var members = type.GetMembers(BindingFlags.Instance | BindingFlags.Public);

		foreach (var memberInfo in members)
		{
			if (memberInfo.MemberType != MemberTypes.Property &&
			    memberInfo.MemberType != MemberTypes.Field) continue;

			var ui = memberInfo.CreateAndBindUi(input);
			if (ui == null) continue;
			control.AddContent(ui);
		}

		return control;
	}
}
using Avalonia.Controls;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using GameBridge.Ui.Factory.UiFabrication.DataBinder;

namespace GameBridge.Ui.Factory;

public static partial class UiFactory
{
	public static Control? CreateAndBindUiField(Type? type, object[] attributes, string name, object value, MemberInfo memberInfo, object target)
	{
		var fabricator = FactoryHelpers.GetFabricatorForType(type);
		var dataBinders = attributes.OfType<IDataBinder>().ToArray();
		if (fabricator != null)
		{
			fabricator.CreateField(name, value, attributes);
			fabricator.BindField(memberInfo, target, dataBinders);
			return fabricator.Field;
		}

		if (type is { IsClass: true } && !typeof(IEnumerable).IsAssignableFrom(type))
		{
			return ProcessClass(value, type);
		}

		if (value != null)
		{
			var textBlock = new TextBlock
			{
				Text = value.ToString()
			};
			return textBlock;
		}

		return null;
	}
}
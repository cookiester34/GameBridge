using Avalonia.Controls;
using Avalonia.Layout;
using GameBridge.Data;
using GameBridge.Ui.Factory.UiFabrication.Decorators;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace GameBridge.Ui.Factory;

public static partial class UiFactory
{
	public static Control? CreateAndBindUi(this MemberInfo memberInfo, object target)
	{
		var memberType = memberInfo.GetUnderlyingType();
		var attributes = memberInfo.GetCustomAttributesWithInheritance();
		var value = memberInfo.GetValue(target);
		var name = FactoryHelpers.NiceString(memberInfo.Name);
		var decorators = attributes.OfType<IDecorator>().ToArray();

		Control field = null;
		
		if (memberType.IsArray || (typeof(IEnumerable).IsAssignableFrom(memberType) && memberType != typeof(string)))
		{
			if (value is not IEnumerable collection) return null;

			field = new UiCollection(name, memberType, attributes);

			((UiCollection)field).InitializeUi(collection);
		}

		field ??= CreateAndBindUiField(memberType, attributes, name, value, memberInfo, target);
		if (field == null) return null;

		Control? finishedField;
		if (field is not ExplorerField and not UiCollection)
		{
			finishedField = FactoryHelpers.CreateNameField(name, field,
				memberType is { IsClass: true } && !typeof(IEnumerable).IsAssignableFrom(memberType));
		}
		else
		{
			finishedField = field;
		}

		if (finishedField == null) return null;
		
		finishedField.Name = name;

		if (decorators.Length > 0)
		{
			var topLevel = decorators.Where(decorator => decorator.IsTopDecorator);
			var bottomLevel = decorators.Where(decorator => !decorator.IsTopDecorator);

			var container = new StackPanel
			{
				Orientation = Orientation.Vertical,
				HorizontalAlignment = HorizontalAlignment.Stretch,
				Spacing = 6
			};

			// top decorators
			foreach (var decorator in topLevel)
			{
				container.AddChild(decorator.CreateDecorator(target));
			}

			//Field
			container.AddChild(finishedField);

			//bottom decorators
			foreach (var decorator in bottomLevel)
			{
				container.AddChild(decorator.CreateDecorator(target));
			}

			return container;
		}

		return finishedField;
	}
}
using Avalonia.Controls;
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
		
		if (memberType.IsArray || (typeof(IEnumerable).IsAssignableFrom(memberType) && memberType != typeof(string)))
		{
			if (value is not IEnumerable collection) return null;

			var collectionUi = new UiCollection(name, memberType, attributes);

			collectionUi.InitializeUi(collection);

			return collectionUi;
		}

		var field = CreateAndBindUiField(memberType, attributes, name, value, memberInfo, target);
		if (field == null) return null;

		if (field is ExplorerField)
		{
			return field;
		}
		
		var finishedField = FactoryHelpers.CreateNameField(name, field,
			memberType is { IsClass: true } && !typeof(IEnumerable).IsAssignableFrom(memberType));

		if (decorators.Length > 0)
		{
			var topLevel = decorators.Where(decorator => decorator.IsTopDecorator);
			var bottomLevel = decorators.Where(decorator => !decorator.IsTopDecorator);
			
			var container = new StackPanel();
			
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
		else
		{
			return finishedField;
		}
	}
}
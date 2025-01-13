using Avalonia.Controls;
using GameBridge.Data;
using GameBridge.Ui.Attributes;
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
		var hasAttributes = attributes.Length > 0;
		var name = memberInfo.Name;

		if (memberType.IsArray || (typeof(IEnumerable).IsAssignableFrom(memberType) && memberType != typeof(string)))
		{
			var collection = value as IEnumerable;

			if (collection == null) return null;

			var collectionUi = new UiCollection(memberType, attributes);

			collectionUi.InitializeUi(collection);

			return collectionUi;
		}

		var uiElement = CreateUiFieldForType(memberType, attributes, name, value);
		if (uiElement == null) return null;

		if (memberType == typeof(string))
		{
			if (hasAttributes)
			{
				if (attributes.Any(o => o.GetType() == typeof(PathAttribute)))
				{
					var explorerField = (ExplorerField)uiElement;
					explorerField.TextChanged += (_, _) =>
					{
						var newValue = explorerField.Text;
						if (newValue != null) memberInfo.SetValue(target, newValue);
					};
					return explorerField;
				}

				if (attributes.Any(o => o.GetType() == typeof(InputFieldAttribute)))
				{
					var textBox = (TextBox)uiElement;
					textBox.TextChanged += (_, _) =>
					{
						var newValue = textBox.Text;
						if (newValue != null) memberInfo.SetValue(target, newValue);
					};
					return FactoryHelpers.CreateNameField(name, textBox);
				}
			}
		}

		if (memberType == typeof(int))
		{
			if (attributes.Any(o => o.GetType() == typeof(InputFieldAttribute)))
			{
				var textBox = (TextBox)uiElement;
				textBox.Watermark = "Int...";
				textBox.TextChanged += FactoryHelpers.ValidateIntegerInputHandler;
				textBox.TextChanged += (_, _) =>
				{
					var newValue = textBox.Text;
					if (newValue != null) memberInfo.SetValue(target, int.Parse(newValue));
				};
				return FactoryHelpers.CreateNameField(name, textBox);
			}
		}

		if (memberType == typeof(float))
		{
			if (attributes.Any(o => o.GetType() == typeof(InputFieldAttribute)))
			{
				var textBox = (TextBox)uiElement;
				textBox.Watermark = "Float...";
				textBox.TextChanged += FactoryHelpers.ValidateFloatInputHandler;
				textBox.TextChanged += (_, _) =>
				{
					var newValue = textBox.Text;
					if (newValue != null) memberInfo.SetValue(target, float.Parse(newValue));
				};
				return FactoryHelpers.CreateNameField(name, textBox);
			}
		}

		if (memberType == typeof(bool))
		{
			var toggle = (ToggleSwitch)uiElement;
			toggle.IsCheckedChanged += (_, _) =>
			{
				var newValue = (bool)toggle.IsChecked;
				memberInfo.SetValue(target, newValue);
			};
		}

		if (memberType is { IsClass: true } && !typeof(IEnumerable).IsAssignableFrom(memberType))
		{
			return ProcessClass(value, memberType);
		}

		return FactoryHelpers.CreateNameField(name, uiElement);
	}
}
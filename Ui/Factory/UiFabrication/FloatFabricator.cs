using Avalonia.Controls;
using GameBridge.Data;
using GameBridge.Ui.Factory.UiFabrication.Attributes;
using GameBridge.Ui.Factory.UiFabrication.DataBinder;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace GameBridge.Ui.Factory.UiFabrication;

public class FloatFabricator : IUiFabricator
{
	public string Name { get; set; }
	public Control Field { get; set; }
	public DataWatcher DataWatcher { get; set; }
	public IList ModifiableCollection { get; set; }
	public int BoundCollectionIndex { get; set; }

	private bool isInputField;
	
	public Control CreateField(string name, object value, params object[] attributes)
	{
		Name = name;
		isInputField = attributes.Any(o => o.GetType() == typeof(InputFieldAttribute));
		
		if (isInputField)
		{
			Field = new TextBox
			{
				Text = (string?)value,
				Watermark = "Int...",
				HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
				Width = double.NaN
			};
			var textBox = ((TextBox)Field);
			textBox.TextChanged += FactoryHelpers.ValidateFloatInputHandler;
		}
		else
		{
			Field = new TextBlock
			{
				Text = (string?)value
			};
		}

		return Field;
	}

	public void BindField(MemberInfo memberInfo, object target, IDataBinder[] dataBinders)
	{
		DataWatcher = new DataWatcher(memberInfo, target);
		
		if (isInputField)
		{
			var textBox = ((TextBox)Field);
			textBox.TextChanged += TextBoxOnTextChanged;
			DataWatcher.dataChanged += (oldValue, newValue) =>
			{
				textBox.TextChanged -= TextBoxOnTextChanged;
				textBox.Text = (string?)newValue;
				textBox.TextChanged += TextBoxOnTextChanged;
			};
			
			void TextBoxOnTextChanged(object? o, TextChangedEventArgs textChangedEventArgs)
			{
				var newValue = textBox.Text;
				if (newValue != null) memberInfo.SetValue(target, newValue);

				foreach (var dataBinder in dataBinders)
				{
					dataBinder.OnValueChange(newValue, target);
				}
			}
		}
		else
		{
			var textBlock = (TextBlock)Field;
			DataWatcher.dataChanged += (oldValue, newValue) =>
			{
				textBlock.Text = (string?)newValue;
			};
		}
	}

	public void BindCollectionElement(IList modifiableCollection, int index, IDataBinder[] dataBinders)
	{
		ModifiableCollection = modifiableCollection;
		DataWatcher = new DataWatcher(ModifiableCollection, index);
		BoundCollectionIndex = index;
		
		if (isInputField)
		{
			var textBox = ((TextBox)Field);
			textBox.TextChanged += TextBoxOnTextChanged;
			DataWatcher.dataChanged += (oldValue, newValue) =>
			{
				textBox.TextChanged -= TextBoxOnTextChanged;
				textBox.Text = (string?)newValue;
				textBox.TextChanged += TextBoxOnTextChanged;
			};
			
			void TextBoxOnTextChanged(object? o, TextChangedEventArgs textChangedEventArgs)
			{
				var newValue = textBox.Text;
				if (newValue != null) ModifiableCollection[BoundCollectionIndex] = newValue;

				foreach (var dataBinder in dataBinders)
				{
					dataBinder.OnValueChange(newValue, ModifiableCollection[BoundCollectionIndex]);
				}
			}
		}
		else
		{
			var textBlock = (TextBlock)Field;
			DataWatcher.dataChanged += (oldValue, newValue) =>
			{
				textBlock.Text = (string?)newValue;
			};
		}
	}
}
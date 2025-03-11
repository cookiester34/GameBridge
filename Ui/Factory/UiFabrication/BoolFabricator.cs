using System;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Interactivity;
using GameBridge.Data;
using GameBridge.Ui.Factory.UiFabrication.DataBinder;
using System.Collections;

namespace GameBridge.Ui.Factory.UiFabrication;

public class BoolFabricator : IUiFabricator
{
	public string Name { get; set; }
	public Control Field { get; set; }
	public DataWatcher DataWatcher { get; set; }
	public IList ModifiableCollection { get; set; }
	public int BoundCollectionIndex { get; set; }

	public Control CreateField(string name, object value, params object[] attributes)
	{
		Field = new ToggleSwitch
		{
			IsChecked = (bool)(value ?? throw new InvalidOperationException())
		};
		return Field;
	}

	public void BindField(MemberInfo memberInfo, object target, IDataBinder[] dataBinders)
	{
		DataWatcher = new DataWatcher(memberInfo, target);
		
		var toggleSwitch = (ToggleSwitch)Field;
		toggleSwitch.IsCheckedChanged += ToggleSwitchOnIsCheckedChanged;
		DataWatcher.dataChanged += (oldValue, newValue) =>
		{
			toggleSwitch.IsCheckedChanged -= ToggleSwitchOnIsCheckedChanged;
			toggleSwitch.IsChecked = (bool?)newValue;
			toggleSwitch.IsCheckedChanged += ToggleSwitchOnIsCheckedChanged;
		};
		
		void ToggleSwitchOnIsCheckedChanged(object? o, RoutedEventArgs routedEventArgs)
		{
			var newValue = (bool)toggleSwitch.IsChecked;
			memberInfo.SetValue(target, newValue);

			foreach (var dataBinder in dataBinders)
			{
				dataBinder.OnValueChange(newValue, target);
			}
		}
	}

	public void BindCollectionElement(IList modifiableCollection, int index, IDataBinder[] dataBinders)
	{
		ModifiableCollection = modifiableCollection;
		DataWatcher = new DataWatcher(ModifiableCollection, index);
		BoundCollectionIndex = index;
		
		var toggleSwitch = (ToggleSwitch)Field;
		toggleSwitch.IsCheckedChanged += ToggleSwitchOnIsCheckedChanged;
		
		DataWatcher.dataChanged += (oldValue, newValue) =>
		{
			toggleSwitch.IsCheckedChanged -= ToggleSwitchOnIsCheckedChanged;
			toggleSwitch.IsChecked = (bool?)newValue;
			toggleSwitch.IsCheckedChanged += ToggleSwitchOnIsCheckedChanged;
		};
		
		void ToggleSwitchOnIsCheckedChanged(object? o, RoutedEventArgs routedEventArgs)
		{
			var newValue = ((ToggleSwitch)Field).IsChecked;
			if (newValue != null) ModifiableCollection[BoundCollectionIndex] = newValue;

			foreach (var dataBinder in dataBinders)
			{
				dataBinder.OnValueChange(newValue, ModifiableCollection[BoundCollectionIndex]);
			}
		}
	}
}
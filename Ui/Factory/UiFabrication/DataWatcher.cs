using Avalonia.Threading;
using GameBridge.Data;
using System;
using System.Collections;
using System.Reflection;

namespace GameBridge.Ui.Factory.UiFabrication;

public class DataWatcher
{
	private object? lastValue;

	private MemberInfo? memberInfo;
	private object target;

	private IList? modifiableCollection;
	private int index;

	public event Action<object?, object?> dataChanged; 
	
	public DataWatcher(MemberInfo memberInfo, object target)
	{
		this.memberInfo = memberInfo;
		this.target = target;
		lastValue = memberInfo.GetValue(target);
		DataWatcherManager.RegisterDataWatcher(this);
	}

	public DataWatcher(IList modifiableCollection, int index)
	{
		this.modifiableCollection = modifiableCollection;
		this.index = index;
		lastValue = modifiableCollection[index];
		DataWatcherManager.RegisterDataWatcher(this);
	}

	public void UpdateIndex(int newIndex)
	{
		index = newIndex;
	}

	public void CheckDataForChanges()
	{
		// Can only change Ui on the main thread and this will trigger UI changes.
		// Also getting the current value here because index won't be updated in time if done on another thread.
		Dispatcher.UIThread.Post(() =>
		{
			// Update UI controls here
			var currentValue = GetCurrentValue();
		
			if (Equals(currentValue, lastValue)) return;
			
			dataChanged?.Invoke(lastValue, currentValue);
			
			lastValue = currentValue;
			
			DataManager.SaveData();
		});
	}

	private object? GetCurrentValue()
	{
		if (memberInfo != null)
		{
			return memberInfo.GetValue(target);
		}

		if (modifiableCollection != null)
		{
			return modifiableCollection[index];
		}
		
		DataWatcherManager.UnregisterDataWatcher(this);
		return null;
	}
	
	public void Dispose()
	{
		DataWatcherManager.UnregisterDataWatcher(this);
	}

	~DataWatcher()
	{
		DataWatcherManager.UnregisterDataWatcher(this);
	}
}
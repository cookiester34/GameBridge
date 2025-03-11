using System.Collections.Generic;
using System.Threading;

namespace GameBridge.Ui.Factory.UiFabrication;

public static class DataWatcherManager
{
	private const int TIMER_CHECK_INTERVAL = 250;
	
	private static Timer timer;
	private static HashSet<DataWatcher> dataWatchers = new();

	static DataWatcherManager()
	{
		timer = new Timer(CheckForChanges, null, TIMER_CHECK_INTERVAL, TIMER_CHECK_INTERVAL);
	}

	private static void CheckForChanges(object? _)
	{
		foreach (var dataWatcher in dataWatchers)
		{
			dataWatcher.CheckDataForChanges();
		}
	}

	public static void RegisterDataWatcher(DataWatcher dataWatcher)
	{
		dataWatchers.Add(dataWatcher);
	}

	public static void UnregisterDataWatcher(DataWatcher dataWatcher)
	{
		dataWatchers.Remove(dataWatcher);
	}
}
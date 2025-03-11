namespace GameBridge.Ui.Factory.UiFabrication.DataBinder;

public interface IDataBinder
{
	public void OnValueChange(object data, object? targetInstance = null);
}
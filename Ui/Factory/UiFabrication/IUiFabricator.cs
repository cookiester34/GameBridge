using System.Reflection;
using Avalonia.Controls;
using GameBridge.Ui.Factory.UiFabrication.DataBinder;
using System.Collections;

namespace GameBridge.Ui.Factory.UiFabrication;

public interface IUiFabricator
{
    public string Name { get; set; }
    public Control Field { get; set; }
    public DataWatcher DataWatcher { get; set; }
    public IList ModifiableCollection { get; set; }
    public int BoundCollectionIndex { get; set; }
    public Control CreateField(string name, object value, params object[] attributes);
    public void BindField(MemberInfo memberInfo, object target, IDataBinder[] dataBinders);
    public void BindCollectionElement(IList modifiableCollection, int index, IDataBinder[] dataBinders);
    
    public void UpdateBoundCollectionIndex(int newIndex)
    {
        BoundCollectionIndex = newIndex;
        DataWatcher.UpdateIndex(newIndex);
    }
}
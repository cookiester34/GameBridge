using System.Reflection;
using Avalonia.Controls;
using GameBridge.Data;
using GameBridge.Ui.Factory.UiFabrication.Attributes;
using GameBridge.Ui.Factory.UiFabrication.DataBinder;
using System.Collections;
using System.Linq;

namespace GameBridge.Ui.Factory.UiFabrication;

public class StringFabricator : IUiFabricator
{
    public string Name { get; set; }
    public Control Field { get; set; }
    public DataWatcher DataWatcher { get; set; }
    public IList ModifiableCollection { get; set; }
    public int BoundCollectionIndex { get; set; }
    private bool isInputField;
    private bool isPathField;
    
    
    public Control CreateField(string name, object value, params object[] attributes)
    {
        Name = name;
        isInputField = attributes.Any(o => o.GetType() == typeof(InputFieldAttribute));
        isPathField = attributes.Any(o => o.GetType() == typeof(PathAttribute));

        if (isInputField)
        {
            Field = new TextBox
            {
                Text = (string?)value,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                Width = double.NaN
            };
        }
        else if (isPathField)
        {
            var pathAttribute = (PathAttribute)attributes.First(o => o.GetType() == typeof(PathAttribute));
            Field = new ExplorerField(name, (string?)value, pathAttribute.PathType);
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
            var textBox = (TextBox)Field;
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
        else if (isPathField)
        {
            var explorerField = (ExplorerField)Field;
            explorerField.TextChanged += ExplorerFieldOnTextChanged;
            DataWatcher.dataChanged += (oldValue, newValue) =>
            {
                explorerField.TextChanged -= ExplorerFieldOnTextChanged;
                explorerField.Text = (string?)newValue;
                explorerField.TextChanged += ExplorerFieldOnTextChanged;
            };
            
            void ExplorerFieldOnTextChanged(object? o, TextChangedEventArgs textChangedEventArgs)
            {
                var newValue = explorerField.Text;
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
            var textBox = (TextBox)Field;

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
        else if (isPathField)
        {
            var explorerField = (ExplorerField)Field;
            explorerField.TextChanged += OnTextChanged;
            
            DataWatcher.dataChanged += (oldValue, newValue) =>
            {
                explorerField.TextChanged -= OnTextChanged;
                explorerField.Text = (string?)newValue;
                explorerField.TextChanged += OnTextChanged;
            };
            
            void OnTextChanged(object? o, TextChangedEventArgs textChangedEventArgs)
            {
                var newValue = explorerField.Text;
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
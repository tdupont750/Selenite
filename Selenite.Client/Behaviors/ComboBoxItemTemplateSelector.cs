using System.Windows;
using System.Windows.Controls;
using Selenite.Client.Extensions;

namespace Selenite.Client.Behaviors
{
    public class ComboBoxItemTemplateSelector : DataTemplateSelector
    {
        #region SelectedTemplate

        public static DependencyProperty SelectedTemplateProperty =
            DependencyProperty.RegisterAttached("SelectedTemplate",
                                                typeof(DataTemplate),
                                                typeof(ComboBoxItemTemplateSelector),
                                                new UIPropertyMetadata(null));
        [AttachedPropertyBrowsableForType(typeof(ComboBox))]
        public static DataTemplate GetSelectedTemplate(ComboBox obj)
        {
            return (DataTemplate)obj.GetValue(SelectedTemplateProperty);
        }
        public static void SetSelectedTemplate(ComboBox obj, DataTemplate value)
        {
            obj.SetValue(SelectedTemplateProperty, value);
        }

        #endregion // SelectedTemplate
        #region DropDownTemplate

        public static DependencyProperty DropDownTemplateProperty =
            DependencyProperty.RegisterAttached("DropDownTemplate",
                                                typeof(DataTemplate),
                                                typeof(ComboBoxItemTemplateSelector),
                                                new UIPropertyMetadata(null));
        [AttachedPropertyBrowsableForType(typeof(ComboBox))]
        public static DataTemplate GetDropDownTemplate(ComboBox obj)
        {
            return (DataTemplate)obj.GetValue(DropDownTemplateProperty);
        }
        public static void SetDropDownTemplate(ComboBox obj, DataTemplate value)
        {
            obj.SetValue(DropDownTemplateProperty, value);
        }

        #endregion // DropDownTemplate

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ComboBox comboBox;
            var comboBoxItem = container.GetVisualParent<ComboBoxItem>();

            if (comboBoxItem == null)
            {
                comboBox = container.GetVisualParent<ComboBox>();
                return GetSelectedTemplate(comboBox);
            }

            comboBox = ComboBox.ItemsControlFromItemContainer(comboBoxItem) as ComboBox;
            return GetDropDownTemplate(comboBox);
        }
    }
}

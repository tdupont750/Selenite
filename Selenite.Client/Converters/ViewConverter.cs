using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Selenite.Client.Converters
{
    public sealed class ViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var valueType = value.GetType();
            var viewName = valueType.Name.Replace("ViewModel", "View");
            var viewType = string.Format("{0}.{1}", (valueType.Namespace ?? "").Replace("ViewModels", "Views"), viewName);

            var returnType = Type.GetType(viewType, false);

            if (returnType == null)
                return null;

            var view = App.ServiceLocator.GetInstance(returnType) as UserControl;

            if (view != null)
            {
                view.DataContext = value;
                return view;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
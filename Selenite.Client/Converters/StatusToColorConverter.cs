using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Selenite.Models;

namespace Selenite.Client.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (ResultStatus) value;

            switch (status)
            {
                case ResultStatus.Passed:
                    return new SolidColorBrush(Colors.LimeGreen);

                case ResultStatus.Failed:
                    return new SolidColorBrush(Colors.Red);

                default:
                    return new SolidColorBrush(Colors.Yellow);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

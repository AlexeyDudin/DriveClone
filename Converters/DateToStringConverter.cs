using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Converters
{
    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime || value is DateTime?)
            {
                if (value != null)
                    return ((DateTime)value).ToShortDateString();
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                DateTime result;
                if (DateTime.TryParse(value.ToString(), out result))
                    return result;
            }
            return DependencyProperty.UnsetValue;
        }
    }
}

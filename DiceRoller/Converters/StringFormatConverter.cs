using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DiceRoller.Converters
{
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime)value).ToString(parameter.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime datetime;
            if (DateTime.TryParse(value.ToString(), out datetime))
            {
                return datetime;
            }

            return DependencyProperty.UnsetValue;
        }
    }
}

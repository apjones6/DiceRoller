using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DiceRoller.Converters
{
    public class VisibleWhenPositiveConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double number;
            if (value == null || !double.TryParse(value.ToString(), out number))
            {
                return Visibility.Collapsed;
            }

            return number > 0.0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DiceRoller.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolean;
            if (bool.TryParse(value.ToString(), out boolean))
            {
                return boolean ? Visibility.Visible : Visibility.Collapsed;
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = value as Visibility?;
            if (value != null)
            {
                return visibility.Value == Visibility.Visible;
            }

            return DependencyProperty.UnsetValue;
        }
    }
}

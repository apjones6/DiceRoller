using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DiceRoller.Converters
{
    public class ActiveBrushWhenPositiveConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double number;
            if (value == null || !double.TryParse(value.ToString(), out number))
            {
                return DependencyProperty.UnsetValue;
            }

            var key = number > 0.0 ? "PhoneAccentBrush" : "PhoneDisabledBrush";
            return Application.Current.Resources[key];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}

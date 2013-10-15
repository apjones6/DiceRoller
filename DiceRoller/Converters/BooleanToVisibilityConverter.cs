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
            var input = System.Convert.ToBoolean(value);

            if (ParseMode(parameter) == Mode.Invert)
            {
                input = !input;
            }

            return input ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = (Visibility)value == Visibility.Visible;

            if (ParseMode(parameter) == Mode.Invert)
            {
                input = !input;
            }

            return input;
        }

        private static Mode ParseMode(object parameter)
        {
            if (parameter is Mode)
            {
                return (Mode)parameter;
            }
            else if (parameter is string)
            {
                return (Mode)Enum.Parse(typeof(Mode), (string)parameter);
            }
            else
            {
                return Mode.Default;
            }
        }

        public enum Mode
        {
            Default = 0,
            Invert = 1
        }
    }
}

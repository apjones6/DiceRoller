using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace DiceRoller.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public Brush PhoneAccentBrush
        {
            get
            {
                return (Brush)Application.Current.Resources["PhoneAccentBrush"];
            }
        }

        public Brush PhoneDisabledBrush
        {
            get
            {
                return (Brush)Application.Current.Resources["PhoneDisabledBrush"];
            }
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
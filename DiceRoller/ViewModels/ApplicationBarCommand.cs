using DiceRoller.Resources;
using GalaSoft.MvvmLight.Command;
using System;
using System.ComponentModel;

namespace DiceRoller.ViewModels
{
    public class ApplicationBarCommand : RelayCommand, INotifyPropertyChanged
    {
        private BarItem item;
        private string iconUri;
        private string text;

        public ApplicationBarCommand(Action execute, BarItem item)
            : base(execute)
        {
            this.item = item;
            this.iconUri = ApplicationBar.ResourceManager.GetString(string.Concat(item, "_IconUri"));
            this.text = ApplicationBar.ResourceManager.GetString(item.ToString());
        }

        public ApplicationBarCommand(Action execute, Func<bool> canExecute, BarItem item)
            : base(execute, canExecute)
        {
            this.item = item;
            this.iconUri = ApplicationBar.ResourceManager.GetString(string.Concat(item, "_IconUri"));
            this.text = ApplicationBar.ResourceManager.GetString(item.ToString());
        }

        public BarItem Item
        {
            get { return item; }
            set
            {
                if (item != value)
                {
                    item = value;
                    IconUri = ApplicationBar.ResourceManager.GetString(string.Concat(item, "_IconUri"));
                    Text = ApplicationBar.ResourceManager.GetString(item.ToString());
                }
            }
        }

        public string IconUri
        {
            get { return iconUri; }
            private set
            {
                iconUri = value;
                RaisePropertyChanged("IconUri");
            }
        }

        public string Text
        {
            get { return text; }
            private set
            {
                text = value;
                RaisePropertyChanged("Text");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }  
}

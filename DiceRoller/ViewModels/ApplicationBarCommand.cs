using GalaSoft.MvvmLight.Command;
using System;
using System.ComponentModel;

namespace DiceRoller.ViewModels
{
    public class ApplicationBarCommand : RelayCommand, INotifyPropertyChanged
    {
        private string iconUri;
        private string text;

        public ApplicationBarCommand(Action execute, string text, string iconUri = null)
            : base(execute)
        {
            this.iconUri = iconUri;
            this.text = text;
        }

        public ApplicationBarCommand(Action execute, Func<bool> canExecute, string text, string iconUri = null)
            : base(execute, canExecute)
        {
            this.iconUri = iconUri;
            this.text = text;
        }

        public string IconUri
        {
            get { return iconUri; }
            set
            {
                iconUri = value;
                RaisePropertyChanged("IconUri");
            }
        }

        public string Text
        {
            get { return text; }
            set
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

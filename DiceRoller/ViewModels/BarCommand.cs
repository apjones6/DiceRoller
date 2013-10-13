using DiceRoller.Resources;
using DiceRoller.ViewModels.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows.Input;

namespace DiceRoller.ViewModels
{
    public class BarCommand : ObservableObject, ICommand
    {
        private readonly BarItem item;
        private string iconUri;
        private bool isEnabled;
        private string text;

        public BarCommand(BarItem item, bool isEnabled = true)
        {
            this.item = item;
            this.IconUri = ApplicationBar.ResourceManager.GetString(string.Concat(item, "_IconUri"));
            this.Text = ApplicationBar.ResourceManager.GetString(item.ToString());
            this.isEnabled = isEnabled;

            Messenger.Default.Register<ModifyBarMessage>(this, OnModifyBarMessage);
        }

        public BarItem Item
        {
            get { return item; }
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

        public bool CanExecute(object parameter)
        {
            return isEnabled;
        }

        public void Execute(object parameter)
        {
            Messenger.Default.Send(new BarMessage(Item));
        }

        public event EventHandler CanExecuteChanged;

        private void OnModifyBarMessage(ModifyBarMessage message)
        {
            if (message.BarItem != Item)
            {
                return;
            }

            if (message.IconUri != null)
            {
                IconUri = message.IconUri;
            }

            if (message.IsEnabled != null)
            {
                isEnabled = message.IsEnabled.Value;
                if (CanExecuteChanged != null)
                {
                    CanExecuteChanged(this, EventArgs.Empty);
                }
            }

            if (message.Text != null)
            {
                Text = message.Text;
            }
        }
    }  
}

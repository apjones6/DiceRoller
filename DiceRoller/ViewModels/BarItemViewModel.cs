using GalaSoft.MvvmLight;
using System.Windows.Input;

namespace DiceRoller.ViewModels
{
    public class BarItemViewModel : ViewModelBase
    {
        private ICommand command;
        private object commandParameter;
        private string iconUri;
        private bool isEnabled;
        private string text;

        public ICommand Command
        {
            get { return command; }
            set
            {
                command = value;
                RaisePropertyChanged("Command");
            }
        }

        public object CommandParameter
        {
            get { return commandParameter; }
            set
            {
                commandParameter = value;
                RaisePropertyChanged("CommandParameter");
            }
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

        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                RaisePropertyChanged("IsEnabled");
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
    }
}

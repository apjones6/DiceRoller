using DiceRoller.Models;
using DiceRoller.ViewModels.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Navigation;

namespace DiceRoller.ViewModels
{
    public class IntegerSelectorViewModel : ViewModelBase
    {
        private IntegerSelectorDataSource dataSource;
        private Pool pool;
        private DiceType type;

        public IntegerSelectorViewModel()
        {
            Messenger.Default.Register<CountPickerMessage>(this, OnCountPickerMessage);

            if (IsInDesignMode)
            {
                DataSource.SelectedItem = 22;
            }
        }

        public ILoopingSelectorDataSource DataSource
        {
            get { return dataSource ?? (dataSource = new IntegerSelectorDataSource()); }
        }

        public ICommand CancelCommand
        {
            get { return new RelayCommand(GoBack); }
        }

        public ICommand DoneCommand
        {
            get { return new RelayCommand(OnDone); }
        }

        private void GoBack()
        {
            Messenger.Default.Send(new NavigateMessage(NavigationMode.Back));
        }

        private void OnCountPickerMessage(CountPickerMessage message)
        {
            pool = message.Pool;
            type = message.Type;

            if (pool != null)
            {
                DataSource.SelectedItem = pool[type];
            }
        }

        private void OnDone()
        {
            pool[type] = (int)DataSource.SelectedItem;
            GoBack();
        }
    }
}

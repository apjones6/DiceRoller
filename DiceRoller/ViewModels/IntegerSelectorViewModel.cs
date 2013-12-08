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
        private readonly ICommand cancel;
        private readonly IntegerSelectorDataSource dataSource;
        private readonly ICommand done;

        private Pool pool;
        private DiceType type;

        public IntegerSelectorViewModel()
        {
            Messenger.Default.Register<CountPickerMessage>(this, OnCountPickerMessage);

            cancel = new RelayCommand(GoBack);
            dataSource = new IntegerSelectorDataSource();
            done = new RelayCommand(OnDone);

            if (IsInDesignMode)
            {
                DataSource.SelectedItem = 22;
                type = DiceType.D6;
            }
        }

        public ILoopingSelectorDataSource DataSource
        {
            get { return dataSource; }
        }

        public ICommand CancelCommand
        {
            get { return cancel; }
        }

        public ICommand DoneCommand
        {
            get { return done; }
        }

        public DiceType Type
        {
            get { return type; }
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
                dataSource.SelectedItem = pool[type];
            }
        }

        private void OnDone()
        {
            pool[type] = (int)dataSource.SelectedItem;
            GoBack();
        }
    }
}

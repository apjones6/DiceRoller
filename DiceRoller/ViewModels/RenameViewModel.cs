using DiceRoller.Models;
using DiceRoller.ViewModels.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;
using System.Windows.Navigation;

namespace DiceRoller.ViewModels
{
    public class RenameViewModel : ViewModelBase
    {
        private readonly ICommand cancel;
        private readonly ICommand done;

        private string name;
        private Pool pool;

        public RenameViewModel()
        {
            Messenger.Default.Register<PoolMessage>(this, PoolMessage.TOKEN_RENAME, OnPoolMessage);

            cancel = new RelayCommand(GoBack);
            done = new RelayCommand(OnDone);

            if (IsInDesignMode)
            {
                Pool = new Pool("D12 + 21D6 + 4D4", "Attack");
            }
        }

        public ICommand CancelCommand
        {
            get { return cancel; }
        }

        public ICommand DoneCommand
        {
            get { return done; }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public Pool Pool
        {
            get { return pool; }
            set
            {
                if (pool != value)
                {
                    pool = value;
                    RaisePropertyChanged("Pool");
                }
            }
        }

        private void GoBack()
        {
            Messenger.Default.Send(new NavigateMessage(NavigationMode.Back));
        }

        private void OnDone()
        {
            pool.Favorite = true;
            pool.Name = Name;
            Messenger.Default.Send(new PoolMessage(pool), PoolMessage.TOKEN_FAVORITE);
            Messenger.Default.Send(new NavigateMessage("/MainPage.xaml"));
        }

        private void OnPoolMessage(PoolMessage message)
        {
            Name = message.Pool.Name;
            Pool = message.Pool;
        }
    }
}

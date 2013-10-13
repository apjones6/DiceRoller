using DiceRoller.Models;
using DiceRoller.ViewModels.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.ComponentModel;
using System.Windows.Input;

namespace DiceRoller.ViewModels
{
    public class PickViewModel : ViewModelBase
    {
        private ICommand holdCommand;
        private ICommand tapCommand;
        private Pool pool;

        public PickViewModel()
        {
            Messenger.Default.Register<BarMessage>(this, OnBarMessage);
            Messenger.Default.Register<PivotMessage>(this, OnPivotMessage);
            
            if (IsInDesignMode)
            {
                Pool = new Pool("D12 + 7D6 + 4D4");
            }
            else
            {
                Pool = new Pool();
            }
        }

        public Pool Pool
        {
            get { return pool; }
            set
            {
                if (pool != null)
                {
                    pool.PropertyChanged -= OnPoolPropertyChanged;
                }

                pool = value;

                if (pool != null)
                {
                    pool.PropertyChanged += OnPoolPropertyChanged;
                }

                RaisePropertyChanged("Pool");
                UpdateApplicationBar();
            }
        }

        public ICommand HoldCommand
        {
            get { return holdCommand ?? (holdCommand = new RelayCommand<DiceType>(t => Pool[t] += 5)); }
        }

        public ICommand TapCommand
        {
            get { return tapCommand ?? (tapCommand = new RelayCommand<DiceType>(t => Pool[t] += 1)); }
        }

        private void OnBarMessage(BarMessage message)
        {
            switch (message.BarItem)
            {
                case BarItem.Roll:
                    Messenger.Default.Send(new PoolMessage(Pool, new PoolResult(Pool)));
                    break;

                case BarItem.Reset:
                    Pool = new Pool();
                    break;
            }
        }

        private void OnPivotMessage(PivotMessage message)
        {
            if (message.Item == PivotItem.Pick)
            {
                UpdateApplicationBar();
            }
        }

        private void OnPoolPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DiceCount")
            {
                UpdateApplicationBar();
            }
        }

        private void UpdateApplicationBar()
        {
            Messenger.Default.Send(new ModifyBarMessage(BarItem.Roll, Pool.DiceCount > 0));
            Messenger.Default.Send(new ModifyBarMessage(BarItem.Reset, Pool.DiceCount > 0));
        }
    }
}

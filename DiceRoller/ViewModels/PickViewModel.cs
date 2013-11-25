using DiceRoller.Models;
using DiceRoller.Resources;
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
        private readonly ICommand hold;
        private readonly RelayCommand reset;
        private readonly RelayCommand roll;
        private readonly ICommand tap;

        private Pool pool;

        public PickViewModel()
        {
            hold = new RelayCommand<DiceType>(OnHold);
            reset = new ApplicationBarCommand(OnReset, () => pool.DiceCount > 0, ApplicationBar.Reset, ApplicationBar.Reset_IconUri);
            roll = new ApplicationBarCommand(OnRoll, () => pool.DiceCount > 0, ApplicationBar.Roll, ApplicationBar.Roll_IconUri);
            tap = new RelayCommand<DiceType>(OnTap);

            if (IsInDesignMode)
            {
                Pool = new Pool("D12 + 7D6 + 4D4");
            }
            else
            {
                Pool = new Pool();
            }
        }

        public ICommand HoldCommand
        {
            get { return hold; }
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
                reset.RaiseCanExecuteChanged();
                roll.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand ResetCommand
        {
            get { return reset; }
        }

        public RelayCommand RollCommand
        {
            get { return roll; }
        }

        public ICommand TapCommand
        {
            get { return tap; }
        }

        private void OnHold(DiceType type)
        {
            Messenger.Default.Send(new CountPickerMessage(pool, type));
        }

        private void OnPoolPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DiceCount")
            {
                reset.RaiseCanExecuteChanged();
                roll.RaiseCanExecuteChanged();
            }
        }

        private void OnReset()
        {
            Pool = new Pool();
        }

        private void OnRoll()
        {
            Messenger.Default.Send(new PoolMessage(pool, new PoolResult(pool)), PoolMessage.TOKEN_CREATE);
            //var message = new PoolMessage(pool, new PoolResult(pool));
            //foreach (var token in new[] { PoolMessage.TOKEN_CREATE, PoolMessage.TOKEN_VIEW })
            //{
            //    Messenger.Default.Send(message, token);
            //}
        }

        private void OnTap(DiceType type)
        {
            if (pool[type] < 99) pool[type] += 1;
        }
    }
}

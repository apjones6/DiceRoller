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
        private readonly ICommand hold;
        private readonly ApplicationBarCommand reset;
        private readonly ApplicationBarCommand roll;
        private readonly ICommand tap;

        private Pool pool;

        public PickViewModel()
        {
            hold = new RelayCommand<DiceType>(OnHold);
            reset = new ApplicationBarCommand(OnReset, () => pool.DiceCount > 0, BarItem.Reset);
            roll = new ApplicationBarCommand(OnRoll, () => pool.DiceCount > 0, BarItem.Roll);
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

        public ApplicationBarCommand ResetCommand
        {
            get { return reset; }
        }

        public ApplicationBarCommand RollCommand
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
            Messenger.Default.Send(new PoolMessage(pool, new PoolResult(pool)));
        }

        private void OnTap(DiceType type)
        {
            if (pool[type] < 99) pool[type] += 1;
        }
    }
}

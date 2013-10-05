using DiceRoller.Models;
using DiceRoller.ViewModels.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Linq;
using System.Windows.Input;

namespace DiceRoller.ViewModels
{
    public class PickViewModel : ViewModelBase
    {
        private Pool pool;
        private ICommand holdCommand;
        private ICommand tapCommand;

        public PickViewModel()
        {
            Messenger.Default.Register<ApplicationBarMessage>(this, OnApplicationBarMessage);

            if (IsInDesignMode)
            {
                Pool.Dice.Single(x => x.Type == DiceType.D4).Count = 4;
                Pool.Dice.Single(x => x.Type == DiceType.D6).Count = 7;
                Pool.Dice.Single(x => x.Type == DiceType.D12).Count = 1;
            }
        }

        public Pool Pool
        {
            get { return pool ?? (pool = new Pool()); }
            set
            {
                pool = value;
                RaisePropertyChanged("Pool");
            }
        }

        public ICommand HoldCommand
        {
            get { return holdCommand ?? (holdCommand = new RelayCommand<DiceType>(OnHold)); }
        }

        public ICommand TapCommand
        {
            get { return tapCommand ?? (tapCommand = new RelayCommand<DiceType>(OnTap)); }
        }

        private void OnHold(DiceType type)
        {
            var component = Pool.Dice.SingleOrDefault(x => x.Type == type);
            if (component != null)
            {
                component.Count += 5;
            }
        }

        private void OnTap(DiceType type)
        {
            var component = Pool.Dice.SingleOrDefault(x => x.Type == type);
            if (component != null)
            {
                component.Count += 1;
            }
        }

        private void OnApplicationBarMessage(ApplicationBarMessage message)
        {
            switch (message.BarItem)
            {
                case BarItem.Roll:
                    Messenger.Default.Send<PoolMessage>(new PoolMessage(Pool, new PoolResult(Pool)));
                    break;

                case BarItem.Reset:
                    Pool = new Pool();
                    break;
            }
        }
    }
}

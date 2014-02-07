using DiceRoller.Models;
using DiceRoller.Resources;
using DiceRoller.ViewModels.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace DiceRoller.ViewModels
{
    public class PickViewModel : ViewModelBase
    {
        private static readonly string[] WATCH_PROPS = new[] { "DiceCount" };

        private readonly RelayCommand favorite;
        private readonly ICommand hold;
        private readonly RelayCommand reset;
        private readonly RelayCommand roll;
        private readonly RelayCommand saveChanges;
        private readonly ICommand tap;

        private Pool original;
        private Pool pool;

        public PickViewModel()
        {
            Messenger.Default.Register<PoolMessage>(this, PoolMessage.TOKEN_PICK, OnPoolMessage);

            favorite = new ApplicationBarCommand(OnFavorite, () => pool.DiceCount > 0, Text.Favorite, IconUri.Favorite);
            hold = new RelayCommand<DiceType>(OnHold);
            reset = new ApplicationBarCommand(OnReset, () => pool.DiceCount > 0, Text.Reset, IconUri.Reset);
            roll = new ApplicationBarCommand(OnRoll, () => pool.DiceCount > 0, Text.Roll, IconUri.Roll);
            saveChanges = new ApplicationBarCommand(OnSaveChanges, () => original != null && pool.DiceCount > 0 && original.DiceExpression != pool.DiceExpression, Text.SaveChanges);
            tap = new RelayCommand<DiceType>(OnTap);

            if (IsInDesignMode)
            {
                Original = new Pool("D4", "Basic Attack");
                Pool = new Pool("D20 + 7D12 + 4D6");
                Pool.Favorite = true;
            }
            else
            {
                Pool = new Pool();
            }
        }

        public RelayCommand FavoriteCommand
        {
            get { return favorite; }
        }

        public ICommand HoldCommand
        {
            get { return hold; }
        }

        public Pool Original
        {
            get { return original; }
            set
            {
                if (original != value)
                {
                    original = value;
                    RaisePropertyChanged("Original");
                    Update();
                }
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
                Update();
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

        public RelayCommand SaveChangesCommand
        {
            get { return saveChanges; }
        }

        public ICommand TapCommand
        {
            get { return tap; }
        }

        private void OnFavorite()
        {
            Messenger.Default.Send(new PoolMessage(new Pool(Pool)), PoolMessage.TOKEN_RENAME);
        }

        private void OnHold(DiceType type)
        {
            Messenger.Default.Send(new CountPickerMessage(pool, type));
        }

        private void OnPoolMessage(PoolMessage message)
        {
            Pool = new Pool(message.Pool);
            Original = message.Pool;
        }

        private void OnPoolPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (WATCH_PROPS.Contains(e.PropertyName))
            {
                Update();
            }
        }

        private void OnReset()
        {
            Pool = new Pool();
            Original = null;
        }

        private void OnRoll()
        {
            var message = new PoolMessage(pool, new PoolResult(pool));
            Messenger.Default.Send(message, PoolMessage.TOKEN_CREATE);
            Messenger.Default.Send(message, PoolMessage.TOKEN_VIEW);
        }

        private void OnSaveChanges()
        {
            foreach (DiceType type in Enum.GetValues(typeof(DiceType)))
            {
                original[type] = Pool[type];
            }

            original.Favorite = true;
            Messenger.Default.Send(new PoolMessage(original), PoolMessage.TOKEN_FAVORITE);
            Update();
        }

        private void OnTap(DiceType type)
        {
            if (pool[type] < 99) pool[type] += 1;
        }

        private void Update()
        {
            favorite.RaiseCanExecuteChanged();
            reset.RaiseCanExecuteChanged();
            roll.RaiseCanExecuteChanged();
            saveChanges.RaiseCanExecuteChanged();
        }
    }
}

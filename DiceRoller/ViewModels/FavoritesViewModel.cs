using DiceRoller.Models;
using DiceRoller.Resources;
using DiceRoller.ViewModels.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DiceRoller.ViewModels
{
    public class FavoritesViewModel : ViewModelBase
    {
        private readonly ObservableCollection<Pool> pools;
        private readonly RelayCommand select;
        private readonly ICommand tap;

        private bool isSelectMode;

        public FavoritesViewModel()
        {
            Messenger.Default.Register<PoolMessage>(this, PoolMessage.TOKEN_FAVORITE, OnPoolMessage);

            pools = new ObservableCollection<Pool>();
            select = new ApplicationBarCommand(OnSelect, () => pools.Count > 0, Text.Select, IconUri.Select);
            tap = new RelayCommand<Pool>(OnTap);

            pools.CollectionChanged += (s, e) => RaisePropertyChanged("IsEmpty");

            if (IsInDesignMode)
            {
                isSelectMode = true;
                pools.Add(new Pool("6D4", "Attack"));
                pools.Add(new Pool("18D4", "Firestorm"));
                pools.Add(new Pool("D20"));
                pools.Add(new Pool("D20 + 2D6"));
                pools.Add(new Pool("D4", "Attack"));
            }
        }

        public bool IsEmpty
        {
            get { return pools.Count == 0; }
        }

        public bool IsSelectMode
        {
            get { return isSelectMode; }
            set
            {
                if (isSelectMode != value)
                {
                    isSelectMode = value;
                    RaisePropertyChanged("IsSelectMode");
                    Update();
                }
            }
        }

        public ObservableCollection<Pool> Pools
        {
            get { return pools; }
        }

        public RelayCommand SelectCommand
        {
            get { return select; }
        }

        public ICommand TapCommand
        {
            get { return tap; }
        }

        private void OnPoolMessage(PoolMessage message)
        {
            if (message.Pool.Favorite)
            {
                pools.Add(message.Pool);
            }
            else
            {
                pools.Remove(message.Pool);
            }
        }

        private void OnSelect()
        {
            // TODO: Show checkboxes, update application bar, lock pivot
            IsSelectMode = !isSelectMode;
        }

        private void OnTap(Pool pool)
        {
            var message = new PoolMessage(pool, new PoolResult(pool));
            Messenger.Default.Send(message, PoolMessage.TOKEN_CREATE);
            Messenger.Default.Send(message, PoolMessage.TOKEN_VIEW);
        }

        private void Update()
        {
            Messenger.Default.Send(new PivotMessage(isSelectMode));
        }
    }
}

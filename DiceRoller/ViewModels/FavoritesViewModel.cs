﻿using DiceRoller.Models;
using DiceRoller.Resources;
using DiceRoller.ViewModels.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace DiceRoller.ViewModels
{
    public class FavoritesViewModel : ViewModelBase
    {
        private readonly RelayCommand delete;
        private readonly ApplicationBarCommand instant;
        private readonly ObservableCollection<Pool> pools;
        private readonly ICommand rename;
        private readonly RelayCommand select;
        private readonly List<Pool> selected;
        private readonly ICommand selectionChanged;
        private readonly ICommand tap;

        private bool isInstant;
        private bool isSelectMode;

        public FavoritesViewModel()
        {
            Messenger.Default.Register<PoolMessage>(this, PoolMessage.TOKEN_FAVORITE, OnPoolMessage);

            delete = new ApplicationBarCommand(OnDelete, () => selected.Count > 0, Text.Delete, IconUri.Delete);
            instant = new ApplicationBarCommand(OnInstant, Text.Instant, IconUri.InstantOff);
            pools = new ObservableCollection<Pool>();
            rename = new RelayCommand<Pool>(OnRename);
            select = new ApplicationBarCommand(OnSelect, () => pools.Count > 0, Text.Select, IconUri.Select);
            selected = new List<Pool>();
            selectionChanged = new RelayCommand<SelectionChangedEventArgs>(OnSelectionChanged);
            tap = new RelayCommand<Pool>(OnTap);

            isInstant = false;
            IsSelectMode = false;

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

        public RelayCommand InstantCommand
        {
            get { return instant; }
        }

        public bool IsEmpty
        {
            get { return pools.Count == 0; }
        }

        public bool IsInstant
        {
            get { return isInstant; }
            set
            {
                if (isInstant != value)
                {
                    instant.IconUri = value ? IconUri.InstantOn : IconUri.InstantOff;
                    isInstant = value;
                }
            }
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

        public ICommand RenameCommand
        {
            get { return rename; }
        }

        public RelayCommand SelectCommand
        {
            get { return select; }
        }

        public ICommand SelectionChangedCommand
        {
            get { return selectionChanged; }
        }

        public ICommand TapCommand
        {
            get { return tap; }
        }

        public void OnBack(CancelEventArgs e)
        {
            if (isSelectMode)
            {
                IsSelectMode = false;
                e.Cancel = true;
            }
        }

        private void OnDelete()
        {
            // Copy, as the selected list will update as each pool removes
            foreach (var pool in selected.ToArray())
            {
                pool.Favorite = false;
                pools.Remove(pool);
            }

            IsSelectMode = false;
        }

        private void OnInstant()
        {
            IsInstant = !isInstant;
        }

        private void OnPoolMessage(PoolMessage message)
        {
            if (message.Pool.Favorite)
            {
                // Ensure not duplicate
                if (pools.IndexOf(message.Pool) == -1)
                {
                    pools.Add(message.Pool);
                }
            }
            else
            {
                pools.Remove(message.Pool);
            }
        }

        private void OnRename(Pool pool)
        {
            Messenger.Default.Send(new PoolMessage(pool), PoolMessage.TOKEN_RENAME);
        }

        private void OnSelect()
        {
            IsSelectMode = !isSelectMode;
        }

        private void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            foreach (Pool pool in e.RemovedItems)
            {
                selected.Remove(pool);
            }

            foreach (Pool pool in e.AddedItems)
            {
                selected.Add(pool);
            }

            delete.RaiseCanExecuteChanged();
        }

        private void OnTap(Pool pool)
        {
            if (isInstant)
            {
                var message = new PoolMessage(pool, new PoolResult(pool));
                Messenger.Default.Send(message, PoolMessage.TOKEN_CREATE);
                Messenger.Default.Send(message, PoolMessage.TOKEN_VIEW);
            }
            else
            {
                Messenger.Default.Send(new PoolMessage(pool), PoolMessage.TOKEN_PICK);
            }
        }

        private void Update()
        {
            Messenger.Default.Send(new PivotMessage(isSelectMode));

            // TODO: This should be accessed through a better mechanism than the singleton property
            var buttons = App.ViewModel.Buttons;
            while (buttons.Count > 0) buttons.RemoveAt(0);

            if (isSelectMode)
            {
                buttons.Add(delete);
            }
            else
            {
                buttons.Add(select);
                buttons.Add(instant);
                selected.Clear();
            }
        }
    }
}

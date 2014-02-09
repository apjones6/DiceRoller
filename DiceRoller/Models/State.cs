using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading;

namespace DiceRoller.Models
{
    public class State
    {
        private const int TIMER_DELAY = 1000;

        private const string KEY_FAVORITES = "KEY_FAVORITES";
        private const string KEY_VERSION = "KEY_VERSION";

        private static ObservableCollection<Pool> favorites;
        private static bool isLoaded;
        private static Modify modify;
        private static IsolatedStorageSettings storage;
        private static Timer timer;

        public static ObservableCollection<Pool> Favorites
        {
            get { return favorites; }
        }

        public static bool IsLoaded
        {
            get { return IsLoaded; }
        }

        public static void Load()
        {
            if (isLoaded)
            {
                Debug.WriteLine("Unnecessary call to State.Load()");
                return;
            }

            storage = IsolatedStorageSettings.ApplicationSettings;

            // Load properties from storage
            favorites = new ObservableCollection<Pool>(storage.Contains(KEY_FAVORITES) ? (Pool[])storage[KEY_FAVORITES] : new Pool[0]);

            // Attach events so we can auto update storage on changes
            favorites.CollectionChanged += ListenToCollectionItems;
            favorites.CollectionChanged += QueueSave;

            // Attach events to collection items, as they don't bubble
            ListenToCollectionItems(favorites);

            isLoaded = true;
        }

        public static void Save()
        {
            Debug.WriteLine("State.Save() - Modify {0}", modify);

            // TODO: Handle concurrency
            if (modify != Modify.None)
            {
                if (modify.HasFlag(Modify.Favorites))
                {
                    storage[KEY_FAVORITES] = favorites.ToArray();
                }

                // Stop any current timer
                if (timer != null)
                {
                    timer.Change(Timeout.Infinite, Timeout.Infinite);
                }

                // Update in storage
                modify = Modify.None;
                storage.Save();
            }
        }

        private static void ListenToCollectionItems(object sender, NotifyCollectionChangedEventArgs e = null)
        {
            if (e != null)
            {
                if (e.NewItems != null)
                {
                    foreach (INotifyPropertyChanged item in e.NewItems) item.PropertyChanged += QueueSave;
                }

                if (e.OldItems != null)
                {
                    foreach (INotifyPropertyChanged item in e.OldItems) item.PropertyChanged -= QueueSave;
                }
            }
            else
            {
                var collection = (IEnumerable)sender;
                foreach (INotifyPropertyChanged item in collection) item.PropertyChanged += QueueSave;
            }
        }

        private static void QueueSave(object sender, EventArgs e)
        {
            // Update favorites
            if (sender == favorites || favorites.Contains(sender))
            {
                modify |= Modify.Favorites;
            }

            // Start/Restart timer; for repeated updates we save once they stop
            if (timer == null)
            {
                timer = new Timer(s => Save(), null, TIMER_DELAY, Timeout.Infinite);
            }
            else
            {
                timer.Change(TIMER_DELAY, Timeout.Infinite);
            }
        }

        [Flags]
        private enum Modify
        {
            None = 0,

            Favorites = 1
        }
    }
}

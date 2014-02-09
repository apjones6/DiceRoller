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
        private const string KEY_FAVORITES = "KEY_FAVORITES";
        private const string KEY_VERSION = "KEY_VERSION";

        private static ObservableCollection<Pool> favorites;
        private static bool isLoaded;
        private static bool isModified;
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

            // TODO: Ensure storage is not updated more than once when a number of items are changed in close succession

            isLoaded = true;
        }

        public static void Save()
        {
            Debug.WriteLine("State.Save() call - {0}", isModified ? "Has Changes" : "No Changes");

            // TODO: Handle concurrency
            if (isModified)
            {
                // Update in storage
                storage[KEY_FAVORITES] = favorites.ToArray();
                storage.Save();

                isModified = false;
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
                var collection = (ICollection)sender;
                foreach (INotifyPropertyChanged item in collection) item.PropertyChanged += QueueSave;
            }
        }

        private static void QueueSave(object sender, EventArgs e)
        {
            isModified = true;
            if (timer == null)
            {
                timer = new Timer(s => Save(), null, 1000, Timeout.Infinite);
            }
            else
            {
                timer.Change(0, Timeout.Infinite);
            }
        }
    }
}

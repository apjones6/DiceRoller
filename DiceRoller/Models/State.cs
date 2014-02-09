using DiceRoller.Models.Updates;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace DiceRoller.Models
{
    public class State
    {
        private const int TIMER_DELAY = 1000;

        private const string KEY_FAVORITES = "KEY_FAVORITES";
        private const string KEY_VERSION = "KEY_VERSION";

        private readonly static Tuple<Version, Version, IUpdater>[] updaters = new[]
            {
                new Tuple<Version, Version, IUpdater>(new Version("1.0.0"), CurrentVersion, new DoNothingUpdater(CurrentVersion))
            };

        private static ObservableCollection<Pool> favorites;
        private static bool isLoaded;
        private static Modify modify;
        private static IsolatedStorageSettings storage;
        private static Timer timer;

        public static Version CurrentVersion
        {
            get
            {
                var v = Assembly.GetCallingAssembly().GetName().Version;
                return new Version(v.Major, v.Minor, v.Build);
            }
        }

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
            timer = new Timer(state => Save(), null, Timeout.Infinite, Timeout.Infinite);

            // Ensure storage is up to date
            Update();

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

            // TODO: Handle concurrency if we support multiple threads
            if (modify != Modify.None)
            {
                if (modify.HasFlag(Modify.Favorites))
                {
                    storage[KEY_FAVORITES] = favorites.ToArray();
                }

                // Stop any current timer
                timer.Change(Timeout.Infinite, Timeout.Infinite);
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
            timer.Change(TIMER_DELAY, Timeout.Infinite);
        }

        private static void Update()
        {
            // Find previous run and application version now
            // NOTE: Version 1.0.0 did not store its version, so if not found it must be 1.0.0
            var previous = new Version(storage.Contains(KEY_VERSION) ? (string)storage[KEY_VERSION] : "1.0.0");
            var now = CurrentVersion;

            // Up to date
            if (previous == now)
            {
                return;
            }

            // Perform updates through registered update functions
            while (previous < now)
            {
                var updater = updaters.Single(x => x.Item1 <= previous && x.Item2 >= previous).Item3;
                previous = updater.Update(storage);
            }

            // Write the new version
            storage[KEY_VERSION] = previous.ToString(3);
            storage.Save();
        }

        [Flags]
        private enum Modify
        {
            None = 0,

            Favorites = 1
        }
    }
}

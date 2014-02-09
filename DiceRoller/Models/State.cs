using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;

namespace DiceRoller.Models
{
    public class State
    {
        private const string KEY_FAVORITES = "KEY_FAVORITES";
        private const string KEY_VERSION = "KEY_VERSION";

        private static ObservableCollection<Pool> favorites;
        private static bool isLoaded;
        private static IsolatedStorageSettings storage;

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

            // TODO: Attach events so we can auto update storage on changes
            // TODO: Ensure storage is not updated more than once when a number of items are changed in close succession

            isLoaded = true;
        }

        public static void Save()
        {
            // TODO: Track the changes since last save, and only store necessary properties
            storage[KEY_FAVORITES] = favorites.ToArray();
            storage.Save();
        }
    }
}

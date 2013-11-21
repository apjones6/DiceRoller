using System.IO.IsolatedStorage;

namespace DiceRoller.Models
{
    public class Settings
    {
        private const string SHOW_FULL_RESULTS = "ShowFullResults";

        private readonly IsolatedStorageSettings storage;

        public Settings()
        {
            storage = IsolatedStorageSettings.ApplicationSettings;
        }

        public bool ShowFullResults
        {
            get { return Get(SHOW_FULL_RESULTS, true); }
            set { Set(SHOW_FULL_RESULTS, value); }
        }

        private T Get<T>(string key, T fallback)
        {
            if (storage.Contains(key))
            {
                return (T)storage[key];
            }
            else
            {
                return fallback;
            }
        }

        private void Set<T>(string key, T value)
        {
            storage[key] = value;
            storage.Save();
        }
    }
}

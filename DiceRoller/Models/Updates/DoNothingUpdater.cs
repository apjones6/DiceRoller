using System;
using System.IO.IsolatedStorage;

namespace DiceRoller.Models.Updates
{
    public class DoNothingUpdater : IUpdater
    {
        private readonly Version version;

        public DoNothingUpdater(string version)
        {
            this.version = new Version(version);
        }

        public Version Update(IsolatedStorageSettings storage)
        {
            return version;
        }
    }
}

using System;
using System.IO.IsolatedStorage;

namespace DiceRoller.Models.Updates
{
    public class DoNothingUpdater : IUpdater
    {
        private readonly Version version;

        public DoNothingUpdater(Version version)
        {
            if (version == null)
            {
                throw new ArgumentNullException("version");
            }

            this.version = version;
        }

        public Version Update(IsolatedStorageSettings storage)
        {
            return version;
        }
    }
}

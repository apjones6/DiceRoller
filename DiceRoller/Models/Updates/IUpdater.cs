using System;
using System.IO.IsolatedStorage;

namespace DiceRoller.Models.Updates
{
    public interface IUpdater
    {
        // NOTE: Update the values in memory only; the final result will be saved once all updaters have run
        Version Update(IsolatedStorageSettings storage);
    }
}

using DiceRoller.Models;
using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;

namespace DiceRoller.ViewModels
{
    public class HistoryViewModel : ViewModelBase
    {
        private ObservableCollection<PoolResult> results;

        public HistoryViewModel()
        {
            Results.Add(new PoolResult { Name = "Attack", Sum = 15, Time = new DateTime(1, 1, 1, 10, 23, 0) });
            Results.Add(new PoolResult { Name = "Firestorm", Sum = 45, Time = new DateTime(1, 1, 1, 10, 22, 0) });
            Results.Add(new PoolResult { Name = "D20", Sum = 8, Time = new DateTime(1, 1, 1, 10, 14, 0) });
            Results.Add(new PoolResult { Name = "D20 + 2D6", Sum = 24, Time = new DateTime(1, 1, 1, 10, 0, 0) });
            Results.Add(new PoolResult { Name = "Attack", Sum = 2, Time = new DateTime(1, 1, 1, 9, 56, 0) });
        }

        public ObservableCollection<PoolResult> Results
        {
            get { return results ?? (results = new ObservableCollection<PoolResult>()); }
        }
    }
}

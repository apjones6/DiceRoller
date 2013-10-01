using System;
using System.Collections.Generic;

namespace DiceRoller.Models
{
    public class PoolResult : Pool
    {
        private Dictionary<DiceType, int[]> results;
        private DateTime time;
        private int sum;

        public Dictionary<DiceType, int[]> Results
        {
            get { return results; }
            set
            {
                results = value;
                RaisePropertyChanged("Results");
            }
        }

        public DateTime Time
        {
            get { return time; }
            set
            {
                time = value;
                RaisePropertyChanged("Time");
            }
        }

        public int Sum
        {
            get { return sum; }
            set
            {
                sum = value;
                RaisePropertyChanged("Sum");
            }
        }
    }
}

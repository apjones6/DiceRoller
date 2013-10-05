using System;
using System.Collections.Generic;
using System.Linq;

namespace DiceRoller.Models
{
    public class PoolResult : Pool
    {
        private Pool pool;
        private Dictionary<DiceType, int[]> results;
        private DateTime time;
        private int sum;

        public PoolResult(Pool pool)
            : base(pool)
        {
            var random = new Random();
            this.pool = pool;
            this.results = pool.Dice.ToDictionary(x => x.Type, x => Enumerable.Repeat(0, x.Count).Select(y => random.Next(1, (int)x.Type + 1)).ToArray());
            this.sum = results.SelectMany(x => x.Value).Sum();
            this.time = DateTime.Now;
        }

        public Pool Pool
        {
            get { return pool; }
        }

        public Dictionary<DiceType, int[]> Results
        {
            get { return results; }
            set
            {
                results = value;
                RaisePropertyChanged("Results");
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

        public DateTime Time
        {
            get { return time; }
            set
            {
                time = value;
                RaisePropertyChanged("Time");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace DiceRoller.Models
{
    public class PoolResult : Pool
    {
        private readonly Pool pool;
        private readonly Dictionary<DiceType, int[]> results;
        private readonly DateTime time;
        private readonly int sum;

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
        }

        public int Sum
        {
            get { return sum; }
        }

        public DateTime Time
        {
            get { return time; }
        }
    }
}

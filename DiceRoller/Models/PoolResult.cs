using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DiceRoller.Models
{
    public class PoolResult : Pool
    {
        private static readonly Random RANDOM;
        private readonly Pool pool;
        private readonly ReadOnlyDictionary<DiceType, int[]> results;
        private readonly DateTime time;
        private readonly int sum;

        static PoolResult()
        {
            RANDOM = new Random();
        }

        public PoolResult(string expression, string name = null, Random random = null)
            : this(new Pool(expression, name), random)
        {
        }

        public PoolResult(Pool pool, Random random = null)
            : base(pool)
        {
            // Ensure a random instance is available to use
            random = random ?? RANDOM;

#if DEBUG
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
#endif

            this.pool = pool;
            this.results = new ReadOnlyDictionary<DiceType, int[]>(pool.Dice.ToDictionary(x => x.Type, x => Generate(x.Type, random).Take(x.Count).ToArray()));
            this.sum = results.SelectMany(x => x.Value).Sum();
            this.time = DateTime.Now;

#if DEBUG
            stopwatch.Stop();
            System.Diagnostics.Debug.WriteLine("PoolResult: {0} ticks", stopwatch.ElapsedTicks);
#endif
        }

        public Pool Pool
        {
            get { return pool; }
        }

        public ReadOnlyDictionary<DiceType, int[]> Results
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

        private static IEnumerable<int> Generate(DiceType type, Random random)
        {
            while (true)
            {
                yield return random.Next((int)type) + 1;
            }
        }
    }
}

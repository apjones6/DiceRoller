using DiceRoller.Models;

namespace DiceRoller.ViewModels.Messages
{
    public class PoolMessage
    {
        private readonly Pool pool;
        private readonly PoolResult result;

        public const int TOKEN_CREATE = 0;
        public const int TOKEN_VIEW = 1;
        public const int TOKEN_FAVORITE = 2;

        public PoolMessage(PoolResult result)
            : this(result.Pool, result)
        {
        }

        public PoolMessage(Pool pool, PoolResult result = null)
        {
            this.pool = pool;
            this.result = result;
        }

        public Pool Pool
        {
            get { return pool; }
        }

        public PoolResult Result
        {
            get { return result; }
        }
    }
}

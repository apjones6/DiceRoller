using DiceRoller.Models;

namespace DiceRoller.ViewModels.Messages
{
    public class PoolMessage
    {
        public const int TOKEN_CREATE = 0;
        public const int TOKEN_VIEW = 1;
        public const int TOKEN_FAVORITE = 2;
        public const int TOKEN_UNFAVORITE = 3;

        public PoolMessage(PoolResult result)
            : this(result.Pool, result)
        {
        }

        public PoolMessage(Pool pool, PoolResult result = null)
        {
            Pool = pool;
            Result = result;
        }

        public Pool Pool { get; set; }

        public PoolResult Result { get; set; }
    }
}

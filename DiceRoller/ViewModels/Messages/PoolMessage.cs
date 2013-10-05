using DiceRoller.Models;

namespace DiceRoller.ViewModels.Messages
{
    public class PoolMessage
    {
        public PoolMessage(Pool pool, PoolResult result = null)
        {
            Pool = pool;
            Result = result;
        }

        public Pool Pool { get; set; }

        public PoolResult Result { get; set; }
    }
}

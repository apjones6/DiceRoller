using DiceRoller.Models;

namespace DiceRoller.ViewModels.Messages
{
    public class InfoMessage
    {
        public InfoMessage(PoolResult result)
        {
            Result = result;
        }

        public PoolResult Result { get; set; }
    }
}

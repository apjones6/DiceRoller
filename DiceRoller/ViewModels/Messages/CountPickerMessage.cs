using DiceRoller.Models;

namespace DiceRoller.ViewModels.Messages
{
    public class CountPickerMessage
    {
        private readonly Pool pool;
        private readonly DiceType type;

        public CountPickerMessage(Pool pool, DiceType type)
        {
            this.pool = pool;
            this.type = type;
        }

        public Pool Pool
        {
            get { return pool; }
        }

        public DiceType Type
        {
            get { return type; }
        }
    }
}

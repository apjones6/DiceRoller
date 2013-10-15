using DiceRoller.Models;

namespace DiceRoller.ViewModels.Messages
{
    public class CountPickerMessage
    {
        public CountPickerMessage(Pool pool, DiceType type)
        {
            Pool = pool;
            Type = type;
        }

        public Pool Pool { get; set; }

        public DiceType Type { get; set; }
    }
}

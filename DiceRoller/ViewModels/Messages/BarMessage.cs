namespace DiceRoller.ViewModels.Messages
{
    public class BarMessage
    {
        public BarMessage(BarItem item)
        {
            BarItem = item;
        }

        public BarItem BarItem { get; private set; }
    }
}

namespace DiceRoller.ViewModels.Messages
{
    public class ApplicationBarMessage
    {
        public ApplicationBarMessage(BarItem item)
        {
            BarItem = item;
        }

        public BarItem BarItem { get; set; }
    }
}

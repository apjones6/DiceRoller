namespace DiceRoller.ViewModels.Messages
{
    public class PivotMessage
    {
        public PivotMessage(PivotItem item)
        {
            Item = item;
        }

        public PivotItem Item { get; private set; }
    }
}

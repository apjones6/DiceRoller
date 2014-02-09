namespace DiceRoller.ViewModels.Messages
{
    public class PivotMessage
    {
        private readonly bool isLocked;

        public PivotMessage(bool isLocked)
        {
            this.isLocked = isLocked;
        }

        public bool IsLocked
        {
            get { return isLocked; }
        }
    }
}

namespace DiceRoller.ViewModels.Messages
{
    public class ModifyBarMessage
    {
        public ModifyBarMessage(BarItem item)
        {
            BarItem = item;
        }

        public ModifyBarMessage(BarItem item, bool enabled)
        {
            BarItem = item;
            IsEnabled = enabled;
        }

        public BarItem BarItem { get; private set; }

        public string IconUri { get; set; }

        public bool? IsEnabled { get; set; }

        public string Text { get; set; }
    }
}

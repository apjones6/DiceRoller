namespace DiceRoller.ViewModels
{
    public abstract class SelectableViewModel : BaseViewModel
    {
        private bool selected;

        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                if (value != selected)
                {
                    selected = value;
                    NotifyPropertyChanged("Selected");
                }
            }
        }
    }
}
using GalaSoft.MvvmLight;

namespace DiceRoller.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private HistoryViewModel history;
        private PickViewModel pick;

        public HistoryViewModel History
        {
            get { return history ?? (history = new HistoryViewModel()); }
        }

        public PickViewModel Pick
        {
            get { return pick ?? (pick = new PickViewModel()); }
        }
    }
}
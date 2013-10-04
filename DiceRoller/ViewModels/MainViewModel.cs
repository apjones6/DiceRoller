using GalaSoft.MvvmLight;

namespace DiceRoller.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private BarViewModel bar;
        private HistoryViewModel history;
        private PickViewModel pick;

        public BarViewModel Bar
        {
            get { return bar ?? (bar = new BarViewModel()); }
        }

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
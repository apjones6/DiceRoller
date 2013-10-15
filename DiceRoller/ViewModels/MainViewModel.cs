using GalaSoft.MvvmLight;

namespace DiceRoller.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly BarViewModel bar;
        private readonly IntegerSelectorViewModel countPicker;
        private readonly HistoryViewModel history;
        private readonly PickViewModel pick;

        public MainViewModel()
        {
            bar = new BarViewModel();
            countPicker = new IntegerSelectorViewModel();
            history = new HistoryViewModel();
            pick = new PickViewModel();
        }

        public BarViewModel Bar
        {
            get { return bar; }
        }

        public IntegerSelectorViewModel CountPicker
        {
            get { return countPicker; }
        }

        public HistoryViewModel History
        {
            get { return history; }
        }

        public PickViewModel Pick
        {
            get { return pick; }
        }
    }
}
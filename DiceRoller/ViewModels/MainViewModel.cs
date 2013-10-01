using GalaSoft.MvvmLight;

namespace DiceRoller.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private PoolEditorViewModel activePool;
        private HistoryViewModel history;

        public PoolEditorViewModel ActivePool
        {
            get { return activePool ?? (activePool = new PoolEditorViewModel()); }
        }

        public HistoryViewModel History
        {
            get { return history ?? (history = new HistoryViewModel()); }
        }
    }
}
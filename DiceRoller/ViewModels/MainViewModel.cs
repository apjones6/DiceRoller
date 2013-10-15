using DiceRoller.ViewModels.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace DiceRoller.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly BarViewModel bar;
        private readonly IntegerSelectorViewModel countPicker;
        private readonly HistoryViewModel history;
        private readonly InfoViewModel info;
        private readonly PickViewModel pick;

        public MainViewModel()
        {
            Messenger.Default.Register<CountPickerMessage>(this, OnCountPickerMessage);
            Messenger.Default.Register<InfoMessage>(this, OnInfoMessage);
            Messenger.Default.Register<PoolMessage>(this, OnPoolMessage);

            bar = new BarViewModel();
            countPicker = new IntegerSelectorViewModel();
            history = new HistoryViewModel();
            info = new InfoViewModel();
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

        public InfoViewModel Info
        {
            get { return info; }
        }

        public PickViewModel Pick
        {
            get { return pick; }
        }

        private void OnCountPickerMessage(CountPickerMessage message)
        {
            Messenger.Default.Send(new NavigateMessage("/CountPickerPage.xaml"));
        }

        private void OnInfoMessage(InfoMessage message)
        {
            Messenger.Default.Send(new NavigateMessage("/InfoPage.xaml"));
        }

        private void OnPoolMessage(PoolMessage message)
        {
            Messenger.Default.Send(new NavigateMessage("/InfoPage.xaml"));
        }
    }
}
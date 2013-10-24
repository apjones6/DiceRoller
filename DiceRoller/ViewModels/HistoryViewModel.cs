using DiceRoller.Models;
using DiceRoller.ViewModels.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace DiceRoller.ViewModels
{
    public class HistoryViewModel : ViewModelBase
    {
        private readonly ApplicationBarCommand clearHistory;
        private readonly ObservableCollection<PoolResult> results;
        private readonly ICommand tap;

        public HistoryViewModel()
        {
            Messenger.Default.Register<PoolMessage>(this, OnPoolMessage);

            clearHistory = new ApplicationBarCommand(OnClearHistory, () => results.Count > 0, BarItem.ClearHistory);
            results = new ObservableCollection<PoolResult>();
            tap = new RelayCommand<PoolResult>(OnTap);

            results.CollectionChanged += (s, e) => RaisePropertyChanged("IsEmpty");

            if (IsInDesignMode)
            {
                results.Add(new PoolResult("6D4", "Attack"));
                results.Add(new PoolResult("18D4", "Firestorm"));
                results.Add(new PoolResult("D20"));
                results.Add(new PoolResult("D20 + 2D6"));
                results.Add(new PoolResult("D4", "Attack"));
            }
        }

        public ApplicationBarCommand ClearHistoryCommand
        {
            get { return clearHistory; }
        }

        public bool IsEmpty
        {
            get { return results.Count == 0; }
        }

        public ObservableCollection<PoolResult> Results
        {
            get { return results; }
        }

        public ICommand TapCommand
        {
            get { return tap; }
        }

        private void OnClearHistory()
        {
            results.Clear();
            clearHistory.RaiseCanExecuteChanged();
        }

        private void OnPoolMessage(PoolMessage message)
        {
            // NOTE: Results.Insert() seems to cause a cyclic layout exception, but I'm sure it wasn't
            //       happening every time before (it is now). This implies possibly timing, or something
            //       not disposed as it should be?
            //Results.Insert(0, message.Result);
            var items = results.ToList();
            items.Insert(0, message.Result);
            results.Clear();
            foreach (var item in items) results.Add(item);
        }

        private void OnTap(PoolResult result)
        {
            Messenger.Default.Send(new InfoMessage(result));
        }
    }
}

using DiceRoller.Models;
using DiceRoller.Resources;
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
        private readonly RelayCommand clearHistory;
        private readonly ObservableCollection<PoolResult> results;
        private readonly ICommand tap;

        public HistoryViewModel()
        {
            Messenger.Default.Register<PoolMessage>(this, PoolMessage.TOKEN_CREATE, OnPoolMessage);

            clearHistory = new ApplicationBarCommand(OnClearHistory, () => results.Count > 0, ApplicationBar.ClearHistory);
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

        public RelayCommand ClearHistoryCommand
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
            results.Insert(0, message.Result);

            // TODO: Make this value (25) configurable in settings
            while (results.Count > 25)
            {
                results.RemoveAt(25);
            }
        }

        private void OnTap(PoolResult result)
        {
            Messenger.Default.Send(new PoolMessage(result), PoolMessage.TOKEN_VIEW);
        }
    }
}

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
        private readonly ObservableCollection<PoolResult> results;
        private ICommand tapCommand;

        public HistoryViewModel()
        {
            results = new ObservableCollection<PoolResult>();
            results.CollectionChanged += (s, e) => RaisePropertyChanged("IsEmpty");

            Messenger.Default.Register<BarMessage>(this, OnBarMessage);
            Messenger.Default.Register<PivotMessage>(this, OnPivotMessage);
            Messenger.Default.Register<PoolMessage>(this, OnPoolMessage);

            if (IsInDesignMode)
            {
                Results.Add(new PoolResult("6D4", "Attack"));
                Results.Add(new PoolResult("18D4", "Firestorm"));
                Results.Add(new PoolResult("D20"));
                Results.Add(new PoolResult("D20 + 2D6"));
                Results.Add(new PoolResult("D4", "Attack"));
            }
        }

        public bool IsEmpty
        {
            get { return Results.Count == 0; }
        }

        public ObservableCollection<PoolResult> Results
        {
            get { return results; }
        }

        public ICommand TapCommand
        {
            get { return tapCommand ?? (tapCommand = new RelayCommand<PoolResult>(OnTap)); }
        }

        private void OnTap(PoolResult result)
        {
            Messenger.Default.Send(new InfoMessage(result));
        }

        private void OnBarMessage(BarMessage message)
        {
            switch (message.BarItem)
            {
                case BarItem.ClearHistory:
                    Results.Clear();
                    Messenger.Default.Send(new ModifyBarMessage(BarItem.ClearHistory, false));
                    break;
            }
        }

        private void OnPivotMessage(PivotMessage message)
        {
            if (message.Item == PivotItem.History)
            {
                Messenger.Default.Send(new ModifyBarMessage(BarItem.ClearHistory, !IsEmpty));
            }
        }

        private void OnPoolMessage(PoolMessage message)
        {
            // NOTE: Results.Insert() seems to cause a cyclic layout exception, but I'm sure it wasn't
            //       happening every time before (it is now). This implies possibly timing, or something
            //       not disposed as it should be?
            //Results.Insert(0, message.Result);
            var items = Results.ToList();
            items.Insert(0, message.Result);
            Results.Clear();
            foreach (var item in items) Results.Add(item);
        }
    }
}

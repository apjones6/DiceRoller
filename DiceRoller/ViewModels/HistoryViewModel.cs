using DiceRoller.Models;
using DiceRoller.ViewModels.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Linq;

namespace DiceRoller.ViewModels
{
    public class HistoryViewModel : ViewModelBase
    {
        private ObservableCollection<PoolResult> results;

        public HistoryViewModel()
        {
            results = new ObservableCollection<PoolResult>();
            results.CollectionChanged += (s, e) => RaisePropertyChanged("IsEmpty");

            Messenger.Default.Register<ApplicationBarMessage>(this, OnApplicationBarMessage);
            Messenger.Default.Register<PoolMessage>(this, OnPoolMessage);

            if (IsInDesignMode)
            {
                Results.Add(new PoolResult(new Pool { Name = "Attack", Dice = new ObservableCollection<PoolComponent> { new PoolComponent(DiceType.D4, 6) } }));
                Results.Add(new PoolResult(new Pool { Name = "Firestorm", Dice = new ObservableCollection<PoolComponent> { new PoolComponent(DiceType.D4, 18) } }));
                Results.Add(new PoolResult(new Pool { Dice = new ObservableCollection<PoolComponent> { new PoolComponent(DiceType.D20, 1) } }));
                Results.Add(new PoolResult(new Pool { Dice = new ObservableCollection<PoolComponent> { new PoolComponent(DiceType.D20, 1), new PoolComponent(DiceType.D6, 2) } }));
                Results.Add(new PoolResult(new Pool { Name = "Attack", Dice = new ObservableCollection<PoolComponent> { new PoolComponent(DiceType.D4, 1) } }));
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

        private void OnApplicationBarMessage(ApplicationBarMessage message)
        {
            switch (message.BarItem)
            {
                case BarItem.ClearHistory:
                    Results.Clear();
                    break;
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

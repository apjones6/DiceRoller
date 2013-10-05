using DiceRoller.Models;
using DiceRoller.ViewModels.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;

namespace DiceRoller.ViewModels
{
    public class HistoryViewModel : ViewModelBase
    {
        private ObservableCollection<PoolResult> results;

        public HistoryViewModel()
        {
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

        public ObservableCollection<PoolResult> Results
        {
            get { return results ?? (results = new ObservableCollection<PoolResult>()); }
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
            Results.Insert(0, message.Result);
        }
    }
}

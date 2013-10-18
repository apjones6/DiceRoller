using DiceRoller.Models;
using DiceRoller.ViewModels.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace DiceRoller.ViewModels
{
    public class InfoViewModel : ViewModelBase
    {
        private PoolResult result;

        public InfoViewModel()
        {
            Messenger.Default.Register<InfoMessage>(this, OnInfoMessage);
            Messenger.Default.Register<PoolMessage>(this, OnPoolMessage);

            if (IsInDesignMode)
            {
                Result = new PoolResult("D12 + 21D6 + 4D4", "Attack");
            }
        }

        public PoolResult Result
        {
            get { return result; }
            set
            {
                if (result != value)
                {
                    result = value;
                    RaisePropertyChanged("Result");
                }
            }
        }

        private void OnInfoMessage(InfoMessage message)
        {
            Result = message.Result;
        }

        private void OnPoolMessage(PoolMessage message)
        {
            Result = message.Result;
        }
    }
}

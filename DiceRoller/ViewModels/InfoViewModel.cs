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
            Messenger.Default.Register<PoolMessage>(this, PoolMessage.TOKEN_CREATE, OnPoolMessage);
            Messenger.Default.Register<PoolMessage>(this, PoolMessage.TOKEN_VIEW, OnPoolMessage);

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

        private void OnPoolMessage(PoolMessage message)
        {
            Result = message.Result;
        }
    }
}

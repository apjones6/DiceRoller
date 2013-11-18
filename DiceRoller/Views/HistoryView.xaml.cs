using DiceRoller.ViewModels.Messages;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Windows.Controls;

namespace DiceRoller.Views
{
    public partial class HistoryView : UserControl
    {
        private bool scrollTop = false;
        private bool loaded = false;

        public HistoryView()
        {
            Messenger.Default.Register<PoolMessage>(this, PoolMessage.TOKEN_CREATE, OnPoolMessage);
            InitializeComponent();
        }

        private void OnPoolMessage(PoolMessage message)
        {
            scrollTop = true;
            ScrollTop();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            loaded = true;
            ScrollTop();
        }

        private void ScrollTop()
        {
            if (scrollTop && loaded)
            {
                Dispatcher.BeginInvoke(() =>
                    {
                        LongListSelector.ScrollTo(LongListSelector.ItemsSource[0]);
                        scrollTop = false;
                    });
            }
        }
    }
}

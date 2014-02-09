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
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Register<PoolMessage>(this, PoolMessage.TOKEN_CREATE, OnPoolMessage);
            Unloaded += OnUnloaded;
            Loaded -= OnLoaded;
        }

        private void OnLongListSelectorLoaded(object sender, RoutedEventArgs e)
        {
            loaded = true;
            ScrollTop();
        }

        private void OnPoolMessage(PoolMessage message)
        {
            scrollTop = true;
            ScrollTop();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister(this);
            Unloaded -= OnUnloaded;
            Loaded += OnLoaded;
        }

        private void ScrollTop()
        {
            if (scrollTop && loaded)
            {
                Dispatcher.BeginInvoke(() =>
                    {
                        HistoryLongListSelector.ScrollTo(HistoryLongListSelector.ItemsSource[0]);
                        scrollTop = false;
                    });
            }
        }
    }
}

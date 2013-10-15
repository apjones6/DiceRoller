using DiceRoller.ViewModels.Messages;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;

namespace DiceRoller
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            Messenger.Default.Register<NavigateMessage>(this, OnNavigateMessage);
            DataContext = App.ViewModel;
        }
        
        private void OnNavigateMessage(NavigateMessage message)
        {
            if (message.Mode == NavigationMode.Back)
            {
                NavigationService.GoBack();
            }
            else
            {
                NavigationService.Navigate(message.Uri);
            }
        }
    }
}
using DiceRoller.ViewModels.Messages;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;
using System.ComponentModel;
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

            App.ViewModel.PropertyChanged += OnPropertyChanged;
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            App.ViewModel.OnBack(e);
            base.OnBackKeyPress(e);
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

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsLocked")
            {
                Pivot.IsLocked = App.ViewModel.IsLocked;
            }
        }
    }
}
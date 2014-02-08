using Microsoft.Phone.Controls;
using System.ComponentModel;
using System.Windows;
using System.Windows.Navigation;

namespace DiceRoller.Pages
{
    public partial class Main : PhoneApplicationPage
    {
        public Main()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
            Loaded += OnLoaded;
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            App.ViewModel.OnBack(e);
            base.OnBackKeyPress(e);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            App.ViewModel.PropertyChanged += OnPropertyChanged;
            Unloaded += OnUnloaded;
            Loaded -= OnLoaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // This is the main page for the application, so back should always exit
            while (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsLocked")
            {
                Dispatcher.BeginInvoke(() =>
                    {
                        MainPagePivot.IsLocked = App.ViewModel.IsLocked;
                    });
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            App.ViewModel.PropertyChanged -= OnPropertyChanged;
            Unloaded -= OnUnloaded;
            Loaded += OnLoaded;
        }
    }
}
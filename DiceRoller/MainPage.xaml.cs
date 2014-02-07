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
            App.ViewModel.PropertyChanged += OnPropertyChanged;
            DataContext = App.ViewModel;
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            App.ViewModel.OnBack(e);
            base.OnBackKeyPress(e);
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
    }
}
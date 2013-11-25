using Microsoft.Phone.Controls;

namespace DiceRoller
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel.Settings;
        }
    }
}
using Microsoft.Phone.Controls;

namespace DiceRoller
{
    public partial class InfoPage : PhoneApplicationPage
    {
        public InfoPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel.Info;
        }
    }
}
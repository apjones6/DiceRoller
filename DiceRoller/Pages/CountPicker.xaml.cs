using Microsoft.Phone.Controls;

namespace DiceRoller.Pages
{
    public partial class CountPicker : PhoneApplicationPage
    {
        public CountPicker()
        {
            InitializeComponent();
            DataContext = App.ViewModel.CountPicker;
        }
    }
}
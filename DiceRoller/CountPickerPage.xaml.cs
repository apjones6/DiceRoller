using Microsoft.Phone.Controls;

namespace DiceRoller
{
    public partial class CountPickerPage : PhoneApplicationPage
    {
        public CountPickerPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel.CountPicker;
        }
    }
}
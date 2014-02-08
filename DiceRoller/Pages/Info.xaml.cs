using Microsoft.Phone.Controls;

namespace DiceRoller.Pages
{
    public partial class Info : PhoneApplicationPage
    {
        public Info()
        {
            InitializeComponent();
            DataContext = App.ViewModel.Info;
        }
    }
}
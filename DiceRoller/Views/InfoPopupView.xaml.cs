using System.Windows.Controls;

namespace DiceRoller.Views
{
    public partial class InfoPopupView : UserControl
    {
        public InfoPopupView()
        {
            InitializeComponent();

            DataContext = App.ViewModel.Info;
            Height = App.Current.Host.Content.ActualHeight;
            Width = App.Current.Host.Content.ActualWidth;
        }
    }
}

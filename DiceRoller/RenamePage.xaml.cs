using Microsoft.Phone.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DiceRoller
{
    public partial class RenamePage : PhoneApplicationPage
    {
        public RenamePage()
        {
            InitializeComponent();
            DataContext = App.ViewModel.Rename;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Focus the view, to remove from TextBox, which hides the keyboard
                this.Focus();
            }
        }

        private void OnTextChanged(object sender, RoutedEventArgs e)
        {
            var binding = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }
    }
}
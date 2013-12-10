using System.Windows.Controls;
using System.Windows.Input;

namespace DiceRoller.Views
{
    public partial class PoolEditorView : UserControl
    {
        public PoolEditorView()
        {
            InitializeComponent();
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Focus the view, to remove from TextBox, which hides the keyboard
                this.Focus();
            }
        }
    }
}

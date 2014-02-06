using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DiceRoller.Views
{
    public partial class PickView : UserControl
    {
        public PickView()
        {
            InitializeComponent();
            Wrapper.Projection = new PlaneProjection { LocalOffsetY = (-1 * NameBox.ActualHeight) + 12 };
            NameBox.IsEnabledChanged += OnNameBoxEnabled;
        }

        //public override void OnApplyTemplate()
        //{
        //    base.OnApplyTemplate();
        //}

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Focus the view, to remove from TextBox, which hides the keyboard
                this.Focus();
            }
        }

        private void OnNameBoxEnabled(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (NameBox.IsEnabled)
            {
                var storyboard = Resources["ShowNameBox"] as Storyboard;
                //if (!double.IsNaN(NameBox.Height))
                //{
                    var animation = (DoubleAnimationUsingKeyFrames)storyboard.Children[0];
                    animation.KeyFrames[0].Value = NameBox.ActualHeight - 12;
                //}

                storyboard.Begin();
            }
            else
            {
                var storyboard = Resources["HideNameBox"] as Storyboard;
                //if (!double.IsNaN(NameBox.Height))
                //{
                    var animation = (DoubleAnimationUsingKeyFrames)storyboard.Children[0];
                    animation.KeyFrames[1].Value = (-1 * NameBox.ActualHeight) + 12;
                //}

                storyboard.Begin();
            }
        }
    }
}

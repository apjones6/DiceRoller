using Microsoft.Phone.Controls;
using System.Windows;
using System.Windows.Controls;

namespace DiceRoller.Views
{
    public partial class PickView : UserControl
    {
        public static readonly DependencyProperty TileHeightProperty = DependencyProperty.Register("TileHeight", typeof(double), typeof(PickView), new PropertyMetadata(210.0));
        public static readonly DependencyProperty TileWidthProperty = DependencyProperty.Register("TileWidth", typeof(double), typeof(PickView), new PropertyMetadata(210.0));

        public PickView()
        {
            InitializeComponent();
        }

        public double TileHeight
        {
            get { return (double)GetValue(TileHeightProperty); }
            set { SetValue(TileHeightProperty, value); }
        }

        public double TileWidth
        {
            get { return (double)GetValue(TileWidthProperty); }
            set { SetValue(TileWidthProperty, value); }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = base.ArrangeOverride(finalSize);

            var orientation = ((PhoneApplicationFrame)Application.Current.RootVisual).Orientation;
            if (orientation == PageOrientation.Landscape || orientation == PageOrientation.LandscapeLeft || orientation == PageOrientation.LandscapeRight)
            {
                TileHeight = 194;
                TileWidth = 194;
            }
            else
            {
                TileHeight = 210;
                TileWidth = 210;
            }

            return size;
        }
    }
}

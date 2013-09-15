using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DiceRoller.Controls
{
    public partial class PoolTileButton : UserControl
    {
        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register("DisplayName", typeof(string), typeof(PoolTileButton), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty CountProperty = DependencyProperty.Register("Count", typeof(int), typeof(PoolTileButton), new PropertyMetadata(0, new PropertyChangedCallback(OnCountChanged)));

        // TODO: Find correct way to do readonly dependency properties in WP8
        public static readonly DependencyProperty CountVisibilityProperty = DependencyProperty.Register("CountVisibility", typeof(Visibility), typeof(PoolTileButton), new PropertyMetadata(Visibility.Collapsed));

        // TODO: Find correct way to do readonly dependency properties in WP8
        public static readonly DependencyProperty BorderBackgroundBrushProperty = DependencyProperty.Register("BorderBackgroundBrush", typeof(Brush), typeof(PoolTileButton), new PropertyMetadata(PhoneDisabledBrush));

        public PoolTileButton()
        {
            InitializeComponent();
        }

        public string DisplayName
        {
            get { return (string)GetValue(DisplayNameProperty); }
            set { SetValue(DisplayNameProperty, value); }
        }

        public int Count
        {
            get { return (int)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }

        // TODO: Find correct way to do readonly dependency properties in WP8
        private Visibility CountVisibility
        {
            get { return (Visibility)GetValue(CountVisibilityProperty); }
            set { SetValue(CountVisibilityProperty, value); }
        }

        // TODO: Find correct way to do readonly dependency properties in WP8
        private Brush BorderBackgroundBrush
        {
            get { return (Brush)GetValue(BorderBackgroundBrushProperty); }
            set { SetValue(BorderBackgroundBrushProperty, value); }
        }

        private static Brush PhoneAccentBrush
        {
            get { return (Brush)Application.Current.Resources["PhoneAccentBrush"]; }
        }

        private static Brush PhoneDisabledBrush
        {
            get { return (Brush)Application.Current.Resources["PhoneDisabledBrush"]; }
        }

        private void OnTap(object sender, GestureEventArgs e)
        {
            Count++;
        }

        private static void OnCountChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = (PoolTileButton)o;
            if (sender.Count > 0)
            {
                sender.BorderBackgroundBrush = PhoneAccentBrush;
                sender.CountVisibility = Visibility.Visible;
            }
            else
            {
                sender.BorderBackgroundBrush = PhoneDisabledBrush;
                sender.CountVisibility = Visibility.Collapsed;
            }
        }
    }
}

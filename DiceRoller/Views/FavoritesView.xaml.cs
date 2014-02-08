using DiceRoller.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace DiceRoller.Views
{
    public partial class FavoritesView : UserControl
    {
        public FavoritesView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var context = (FavoritesViewModel)DataContext;
            context.PropertyChanged += OnPropertyChanged;
            FavoritesLongListSelector.IsSelectionEnabledChanged += OnSelectionEnabledChanged;
            Unloaded += OnUnloaded;
            Loaded -= OnLoaded;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelectMode")
            {
                FavoritesLongListSelector.IsSelectionEnabled = ((FavoritesViewModel)sender).IsSelectMode;
            }
        }

        private void OnSelectionEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(bool)e.NewValue)
            {
                var context = (FavoritesViewModel)DataContext;
                context.IsSelectMode = false;
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            var context = (FavoritesViewModel)DataContext;
            context.PropertyChanged -= OnPropertyChanged;
            FavoritesLongListSelector.IsSelectionEnabledChanged -= OnSelectionEnabledChanged;
            Unloaded -= OnUnloaded;
            Loaded += OnLoaded;
        }
    }
}

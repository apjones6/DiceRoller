using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

namespace DiceRoller.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            this.Favorites = new ObservableCollection<PoolViewModel>();
            this.History = new ObservableCollection<RollViewModel>();
            this.Pool = new PoolViewModel();
        }

        public Popup Popup
        {
            get;
            private set;
        }

        public PoolViewModel Pool
        {
            get;
            set;
        }

        public ObservableCollection<PoolViewModel> Favorites
        {
            get;
            private set;
        }

        public ObservableCollection<RollViewModel> History
        {
            get;
            private set;
        }

        public bool IsInitialized
        {
            get;
            private set;
        }

        public void Initialize()
        {
            Favorites.Add(new PoolViewModel("Attack") { Selected = true });
            Favorites.Add(new PoolViewModel("d20"));
            Favorites.Add(new PoolViewModel("Flame Blast"));

            var now = DateTime.Now;
            History.Add(new RollViewModel(17, "Attack") { CreatedDate = now, Favorite = true });
            History.Add(new RollViewModel(25, "4d10 + d4") { CreatedDate = now.AddMinutes(-1) });
            History.Add(new RollViewModel(8, "Reflex") { CreatedDate = now.AddMinutes(-4), Favorite = true, Selected = true });
            History.Add(new RollViewModel(12, "d20") { CreatedDate = now.AddMinutes(-16) });
            History.Add(new RollViewModel(2, "d6") { CreatedDate = now.AddMinutes(-38) });

            this.IsInitialized = true;
        }

        public void OnRoll()
        {
            // Create some content to show in the popup. Typically you would 
            // create a user control.
            var border = new Border();
            border.Background = (Brush)Application.Current.Resources["PhoneAccentBrush"];

            var text = new TextBlock();
            text.Text = "25";
            text.Margin = new Thickness(5.0);
            text.FontSize = 96.0;
            border.Child = text;

            // Set the Child property of Popup to the border 
            // which contains a stackpanel, textblock and button.
            Popup = new Popup();
            Popup.Child = border;

            // Set where the popup will show up on the screen.
            //Popup.VerticalOffset = 150;
            //Popup.HorizontalOffset = 150;
            Popup.HorizontalAlignment = HorizontalAlignment.Center;
            Popup.VerticalAlignment = VerticalAlignment.Center;
            Popup.Height = 200;
            Popup.Width = 200;

            // Open the popup.
            Popup.IsOpen = true;

            // Close 2 seconds later
            var dispatcherTimer = new DispatcherTimer();
            EventHandler handler = null;
            handler = (s, e) =>
            {
                dispatcherTimer.Tick -= handler;
                dispatcherTimer.Stop();
                if (Popup != null && Popup.IsOpen)
                {
                    Popup.IsOpen = false;
                    Popup = null;
                }
            };
            dispatcherTimer.Tick += handler;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(2);
            dispatcherTimer.Start();
        }
    }
}
using DiceRoller.Models;
using DiceRoller.ViewModels.Messages;
using DiceRoller.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace DiceRoller.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private const int PIVOT_PICK = 0;
        private const int PIVOT_HISTORY = 1;
        private const int PIVOT_FAVS = 2;

        private readonly Settings appSettings;
        private readonly RelayCommand<CancelEventArgs> back;
        private readonly ObservableCollection<RelayCommand> buttons;
        private readonly IntegerSelectorViewModel countPicker;
        private readonly HistoryViewModel history;
        private readonly ObservableCollection<RelayCommand> items;
        private readonly InfoViewModel info;
        private readonly PickViewModel pick;
        private readonly RelayCommand settings;

        private PageOrientation orientation;
        private Popup popup;
        private int selectedIndex;

        public MainViewModel()
        {
            Messenger.Default.Register<CountPickerMessage>(this, x => Navigate("/CountPickerPage.xaml"));
            Messenger.Default.Register<PoolMessage>(this, PoolMessage.TOKEN_CREATE, OnPoolMessageCreate);
            Messenger.Default.Register<PoolMessage>(this, PoolMessage.TOKEN_VIEW, x => Navigate("/InfoPage.xaml"));

            appSettings = new Settings();
            back = new RelayCommand<CancelEventArgs>(OnBackKeyPress);
            buttons = new ObservableCollection<RelayCommand>();
            countPicker = new IntegerSelectorViewModel();
            history = new HistoryViewModel();
            info = new InfoViewModel();
            items = new ObservableCollection<RelayCommand>();
            pick = new PickViewModel();
            settings = new ApplicationBarCommand(OnSettings, Resources.ApplicationBar.Settings);

            orientation = PageOrientation.Portrait;
            popup = new Popup();
            selectedIndex = 0;

            Update();
        }

        public ICommand BackCommand
        {
            get { return back; }
        }

        public ObservableCollection<RelayCommand> BarButtons
        {
            get { return buttons; }
        }

        public IntegerSelectorViewModel CountPicker
        {
            get { return countPicker; }
        }

        public HistoryViewModel History
        {
            get { return history; }
        }

        public InfoViewModel Info
        {
            get { return info; }
        }

        public bool IsLandscape
        {
            get { return orientation == PageOrientation.Landscape || orientation == PageOrientation.LandscapeLeft || orientation == PageOrientation.LandscapeRight; }
        }

        public bool IsBarVisible
        {
            get { return buttons.Count > 0 || items.Count > 0; }
        }

        public ApplicationBarMode BarMode
        {
            get { return buttons.Count > 0 || IsLandscape ? ApplicationBarMode.Default : ApplicationBarMode.Minimized; }
        }

        public ObservableCollection<RelayCommand> BarMenuItems
        {
            get { return items; }
        }

        public PageOrientation Orientation
        {
            get { return orientation; }
            set
            {
                if (orientation != value)
                {
                    orientation = value;
                    RaisePropertyChanged("BarMode");
                    RaisePropertyChanged("Orientation");
                }
            }
        }

        public PickViewModel Pick
        {
            get { return pick; }
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (selectedIndex != value)
                {
                    selectedIndex = value;
                    RaisePropertyChanged("SelectedIndex");
                    Update();
                }
            }
        }

        public Settings Settings
        {
            get { return appSettings; }
        }

        public RelayCommand SettingsCommand
        {
            get { return settings; }
        }

        private void Navigate(string uri)
        {
            Messenger.Default.Send(new NavigateMessage(uri));
        }

        private void OnBackKeyPress(CancelEventArgs e)
        {
            if (popup.IsOpen)
            {
                popup.IsOpen = false;
                e.Cancel = true;
            }
        }

        private void OnPoolMessageCreate(PoolMessage message)
        {
            if (appSettings.ShowFullResults)
            {
                Messenger.Default.Send(message, PoolMessage.TOKEN_VIEW);
            }
            else
            {
                // Dispose previous popup
                if (popup.IsOpen)
                {
                    popup.IsOpen = false;
                }

                // Open popup to close after 1 second, and clean up
                TimerCallback callback = x => popup.Dispatcher.BeginInvoke(() => popup.IsOpen = false);
                var timer = new Timer(callback, popup, 1000, Timeout.Infinite);
                popup.Closed += (s, e) => timer.Dispose();
                popup.Child = new InfoPopupView();
                popup.IsOpen = true;
            }
        }

        private void OnSettings()
        {
            Navigate("/SettingsPage.xaml");
        }

        private void Update()
        {
            while (buttons.Count > 0) buttons.RemoveAt(0);
            while (items.Count > 0) items.RemoveAt(0);
            // NOTE: The Clear() method is not listened to by the Bindable Application Bar
            //       see: https://bindableapplicationb.codeplex.com/workitem/446
            //buttons.Clear();
            //items.Clear();

            switch (selectedIndex)
            {
                case PIVOT_PICK:
                    buttons.Add(pick.RollCommand);
                    //Buttons.Add(new BarCommand(BarItem.Favorite, false));
                    buttons.Add(pick.ResetCommand);
                    items.Add(SettingsCommand);
                    break;

                case PIVOT_HISTORY:
                    //Buttons.Add(new BarCommand(BarItem.Select, false));
                    items.Add(history.ClearHistoryCommand);
                    items.Add(SettingsCommand);
                    break;

                case PIVOT_FAVS:
                    //items.Add(SettingsCommand);
                    break;
            }

            // Ensure buttons and items enable state
            foreach (var i in buttons.Union(items)) i.RaiseCanExecuteChanged();

            // Update properties
            RaisePropertyChanged("BarMode");
            RaisePropertyChanged("IsBarVisible");
        }
    }
}
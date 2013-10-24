using DiceRoller.ViewModels.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using System.Linq;

namespace DiceRoller.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private const int PIVOT_PICK = 0;
        private const int PIVOT_HISTORY = 1;
        private const int PIVOT_FAVS = 2;

        private readonly ObservableCollection<RelayCommand> buttons;
        private readonly IntegerSelectorViewModel countPicker;
        private readonly HistoryViewModel history;
        private readonly ObservableCollection<RelayCommand> items;
        private readonly InfoViewModel info;
        private readonly PickViewModel pick;

        private PageOrientation orientation;
        private int selectedIndex;

        public MainViewModel()
        {
            Messenger.Default.Register<CountPickerMessage>(this, x => Navigate("/CountPickerPage.xaml"));
            Messenger.Default.Register<InfoMessage>(this, x => Navigate("/InfoPage.xaml"));
            Messenger.Default.Register<PoolMessage>(this, x => Navigate("/InfoPage.xaml"));

            buttons = new ObservableCollection<RelayCommand>();
            countPicker = new IntegerSelectorViewModel();
            history = new HistoryViewModel();
            info = new InfoViewModel();
            items = new ObservableCollection<RelayCommand>();
            pick = new PickViewModel();

            orientation = PageOrientation.Portrait;
            selectedIndex = 0;

            Update();
        }

        public ObservableCollection<RelayCommand> Buttons
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

        public bool IsVisible
        {
            get { return buttons.Count > 0 || items.Count > 0; }
        }

        public ApplicationBarMode Mode
        {
            get { return buttons.Count > 0 || IsLandscape ? ApplicationBarMode.Default : ApplicationBarMode.Minimized; }
        }

        public ObservableCollection<RelayCommand> MenuItems
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
                    RaisePropertyChanged("Orientation");
                    RaisePropertyChanged("Mode");
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

        private void Navigate(string uri)
        {
            Messenger.Default.Send(new NavigateMessage(uri));
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
                    //MenuItems.Add(new BarCommand(BarItem.Settings));
                    break;

                case PIVOT_HISTORY:
                    //Buttons.Add(new BarCommand(BarItem.Select, false));
                    items.Add(history.ClearHistoryCommand);
                    //MenuItems.Add(new BarCommand(BarItem.Settings));
                    break;

                case PIVOT_FAVS:
                    //MenuItems.Add(new BarCommand(BarItem.Settings));
                    break;
            }

            // Ensure buttons and items enable state
            foreach (var i in buttons.Union(items)) i.RaiseCanExecuteChanged();

            // Update properties
            RaisePropertyChanged("IsVisible");
            RaisePropertyChanged("Mode");
        }
    }
}
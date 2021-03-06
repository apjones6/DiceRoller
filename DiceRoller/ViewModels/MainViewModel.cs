using DiceRoller.ViewModels.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace DiceRoller.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private const int PIVOT_FAVORITES = 2;
        private const int PIVOT_HISTORY = 1;
        private const int PIVOT_PICK = 0;

        private readonly ObservableCollection<RelayCommand> buttons;
        private readonly IntegerSelectorViewModel countPicker;
        private readonly FavoritesViewModel favorites;
        private readonly HistoryViewModel history;
        private readonly ObservableCollection<RelayCommand> items;
        private readonly InfoViewModel info;
        private readonly PickViewModel pick;
        private readonly RenameViewModel rename;

        private bool isLocked;
        private PageOrientation orientation;
        private int selectedIndex;

        public MainViewModel()
        {
            Messenger.Default.Register<CountPickerMessage>(this, x => Navigate("/Pages/CountPicker.xaml"));
            Messenger.Default.Register<PivotMessage>(this, x => IsLocked = x.IsLocked);
            Messenger.Default.Register<PoolMessage>(this, PoolMessage.TOKEN_CREATE, x => Navigate("/Pages/Info.xaml"));
            Messenger.Default.Register<PoolMessage>(this, PoolMessage.TOKEN_FAVORITE, x => SelectedIndex = PIVOT_FAVORITES);
            Messenger.Default.Register<PoolMessage>(this, PoolMessage.TOKEN_PICK, x => SelectedIndex = PIVOT_PICK);
            Messenger.Default.Register<PoolMessage>(this, PoolMessage.TOKEN_RENAME, x => Navigate("/Pages/Rename.xaml"));
            Messenger.Default.Register<PoolMessage>(this, PoolMessage.TOKEN_VIEW, x => Navigate("/Pages/Info.xaml"));

            buttons = new ObservableCollection<RelayCommand>();
            countPicker = new IntegerSelectorViewModel();
            favorites = new FavoritesViewModel();
            history = new HistoryViewModel();
            info = new InfoViewModel();
            items = new ObservableCollection<RelayCommand>();
            pick = new PickViewModel();
            rename = new RenameViewModel();

            isLocked = false;
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

        public FavoritesViewModel Favorites
        {
            get { return favorites; }
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

        public bool IsLocked
        {
            get { return isLocked; }
            set
            {
                if (isLocked != value)
                {
                    isLocked = value;
                    RaisePropertyChanged("IsLocked");
                }
            }
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

        public RenameViewModel Rename
        {
            get { return rename; }
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

        public void OnBack(CancelEventArgs e)
        {
            favorites.OnBack(e);
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
                    buttons.Add(pick.FavoriteCommand);
                    buttons.Add(pick.RollCommand);
                    buttons.Add(pick.ResetCommand);
                    items.Add(pick.SaveChangesCommand);
                    //items.Add(settings);
                    break;

                case PIVOT_HISTORY:
                    //buttons.Add(history.SelectCommand);
                    items.Add(history.ClearHistoryCommand);
                    //items.Add(settings);
                    break;

                case PIVOT_FAVORITES:
                    buttons.Add(favorites.SelectCommand);
                    buttons.Add(favorites.InstantCommand);
                    //items.Add(settings);
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
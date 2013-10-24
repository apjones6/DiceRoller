using DiceRoller.ViewModels.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using System.Linq;

namespace DiceRoller.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ObservableCollection<ApplicationBarCommand> buttons;
        private readonly IntegerSelectorViewModel countPicker;
        private readonly HistoryViewModel history;
        private readonly ObservableCollection<ApplicationBarCommand> items;
        private readonly InfoViewModel info;
        private readonly PickViewModel pick;

        private PageOrientation orientation;
        private int selectedIndex;

        public MainViewModel()
        {
            Messenger.Default.Register<CountPickerMessage>(this, OnCountPickerMessage);
            Messenger.Default.Register<InfoMessage>(this, OnInfoMessage);
            Messenger.Default.Register<PoolMessage>(this, OnPoolMessage);

            buttons = new ObservableCollection<ApplicationBarCommand>();
            countPicker = new IntegerSelectorViewModel();
            history = new HistoryViewModel();
            info = new InfoViewModel();
            items = new ObservableCollection<ApplicationBarCommand>();
            pick = new PickViewModel();

            orientation = PageOrientation.Portrait;
            selectedIndex = 0;

            Update();
        }

        public ObservableCollection<ApplicationBarCommand> Buttons
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

        public ObservableCollection<ApplicationBarCommand> MenuItems
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

        private void OnCountPickerMessage(CountPickerMessage message)
        {
            Messenger.Default.Send(new NavigateMessage("/CountPickerPage.xaml"));
        }

        private void OnInfoMessage(InfoMessage message)
        {
            Messenger.Default.Send(new NavigateMessage("/InfoPage.xaml"));
        }

        private void OnPoolMessage(PoolMessage message)
        {
            Messenger.Default.Send(new NavigateMessage("/InfoPage.xaml"));
        }

        private void Update()
        {
            while (buttons.Count > 0) buttons.RemoveAt(0);
            while (items.Count > 0) items.RemoveAt(0);
            // NOTE: The Clear() method is not listened to by the Bindable Application Bar
            //       see: https://bindableapplicationb.codeplex.com/workitem/446
            //buttons.Clear();
            //items.Clear();

            switch ((PivotItem)selectedIndex)
            {
                case PivotItem.Pick:
                    buttons.Add(pick.RollCommand);
                    //Buttons.Add(new BarCommand(BarItem.Favorite, false));
                    buttons.Add(pick.ResetCommand);
                    //MenuItems.Add(new BarCommand(BarItem.Settings));
                    break;

                case PivotItem.History:
                    //Buttons.Add(new BarCommand(BarItem.Select, false));
                    items.Add(history.ClearHistoryCommand);
                    //MenuItems.Add(new BarCommand(BarItem.Settings));
                    break;

                case PivotItem.Favorites:
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
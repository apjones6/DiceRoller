using DiceRoller.ViewModels.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;

namespace DiceRoller.ViewModels
{
    public class BarViewModel : ViewModelBase
    {
        private int index;
        private readonly ObservableCollection<BarCommand> buttons;
        private readonly ObservableCollection<BarCommand> items;
        private PageOrientation orientation;

        public BarViewModel()
        {
            buttons = new ObservableCollection<BarCommand>();
            items = new ObservableCollection<BarCommand>();
            Update();
        }

        public int SelectedIndex
        {
            get { return index; }
            set
            {
                if (index != value)
                {
                    index = value;
                    RaisePropertyChanged("SelectedIndex");
                    Update();
                }
            }
        }

        public bool IsLandscape
        {
            get { return Orientation == PageOrientation.Landscape || Orientation == PageOrientation.LandscapeLeft || Orientation == PageOrientation.LandscapeRight; }
        }

        public bool IsVisible
        {
            get { return Buttons.Count > 0 || MenuItems.Count > 0; }
        }

        public ApplicationBarMode Mode
        {
            get { return Buttons.Count > 0 || IsLandscape ? ApplicationBarMode.Default : ApplicationBarMode.Minimized; }
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

        public ObservableCollection<BarCommand> Buttons
        {
            get { return buttons; }
        }

        public ObservableCollection<BarCommand> MenuItems
        {
            get { return items; }
        }

        private void Update()
        {
            while (Buttons.Count > 0) Buttons.RemoveAt(0);
            while (MenuItems.Count > 0) MenuItems.RemoveAt(0);
            // NOTE: The Clear() method is not listened to by the Bindable Application Bar
            //       see: https://bindableapplicationb.codeplex.com/workitem/446
            //Buttons.Clear();
            //MenuItems.Clear();

            var item = (PivotItem)SelectedIndex;
            switch (item)
            {
                case PivotItem.Pick:
                    Buttons.Add(new BarCommand(BarItem.Roll, false));
                    //Buttons.Add(new BarCommand(BarItem.Favorite, false));
                    Buttons.Add(new BarCommand(BarItem.Reset, false));
                    //MenuItems.Add(new BarCommand(BarItem.Settings));
                    break;

                case PivotItem.History:
                    //Buttons.Add(new BarCommand(BarItem.Select, false));
                    MenuItems.Add(new BarCommand(BarItem.ClearHistory, false));
                    //MenuItems.Add(new BarCommand(BarItem.Settings));
                    break;

                case PivotItem.Favorites:
                    //MenuItems.Add(new BarCommand(BarItem.Settings));
                    break;
            }

            // Message for other view models to act accordingly
            Messenger.Default.Send(new PivotMessage(item));

            // Update properties
            RaisePropertyChanged("IsVisible");
            RaisePropertyChanged("Mode");
        }
    }
}

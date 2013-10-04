using GalaSoft.MvvmLight;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;

namespace DiceRoller.ViewModels
{
    public class BarViewModel : ViewModelBase
    {
        private int index;
        private ObservableCollection<BarItemViewModel> buttons;
        private ObservableCollection<BarItemViewModel> items;

        public BarViewModel()
        {
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

        public bool IsVisible
        {
            get { return Buttons.Count > 0 || MenuItems.Count > 0; }
        }

        public ApplicationBarMode Mode
        {
            get { return Buttons.Count > 0 ? ApplicationBarMode.Default : ApplicationBarMode.Minimized; }
        }

        public ObservableCollection<BarItemViewModel> Buttons
        {
            get { return buttons ?? (buttons = new ObservableCollection<BarItemViewModel>()); }
        }

        public ObservableCollection<BarItemViewModel> MenuItems
        {
            get { return items ?? (items = new ObservableCollection<BarItemViewModel>()); }
        }

        private void Update()
        {
            while (Buttons.Count > 0) Buttons.RemoveAt(0);
            while (MenuItems.Count > 0) MenuItems.RemoveAt(0);
            // NOTE: The Clear() method is not listened to by the Bindable Application Bar
            //       see: https://bindableapplicationb.codeplex.com/workitem/446
            //Buttons.Clear();
            //MenuItems.Clear();

            switch (SelectedIndex)
            {
                case 0:
                    Buttons.Add(new BarItemViewModel { Text = "roll", IconUri = "/Assets/Images/check.png" });
                    Buttons.Add(new BarItemViewModel { Text = "favorite", IconUri = "/Assets/Images/favs.png" });
                    Buttons.Add(new BarItemViewModel { Text = "reset", IconUri = "/Assets/Images/delete.png" });
                    MenuItems.Add(new BarItemViewModel { Text = "settings" });
                    break;

                case 1:
                    Buttons.Add(new BarItemViewModel { Text = "select", IconUri = "/Assets/Images/list.png" });
                    MenuItems.Add(new BarItemViewModel { Text = "clear history" });
                    MenuItems.Add(new BarItemViewModel { Text = "settings" });
                    break;

                case 2:
                    MenuItems.Add(new BarItemViewModel { Text = "settings" });
                    break;
            }

            // Update properties
            RaisePropertyChanged("IsVisible");
            RaisePropertyChanged("Mode");
        }
    }
}

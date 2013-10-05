using DiceRoller.ViewModels.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DiceRoller.ViewModels
{
    public class BarViewModel : ViewModelBase
    {
        private int index;
        private ObservableCollection<BarItemViewModel> buttons;
        private ObservableCollection<BarItemViewModel> items;
        private ICommand command;

        public BarViewModel()
        {
            command = new RelayCommand<BarItem>(OnClick);
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

            var item = (PivotItem)SelectedIndex;
            switch (item)
            {
                case PivotItem.Pick:
                    Buttons.Add(Item(BarItem.Roll));
                    Buttons.Add(Item(BarItem.Favorite));
                    Buttons.Add(Item(BarItem.Reset));
                    MenuItems.Add(Item(BarItem.Settings));
                    break;

                case PivotItem.History:
                    Buttons.Add(Item(BarItem.Select));
                    MenuItems.Add(Item(BarItem.ClearHistory));
                    MenuItems.Add(Item(BarItem.Settings));
                    break;

                case PivotItem.Favorites:
                    MenuItems.Add(Item(BarItem.Settings));
                    break;
            }

            // Update properties
            RaisePropertyChanged("IsVisible");
            RaisePropertyChanged("Mode");
        }

        private void OnClick(BarItem item)
        {
            Messenger.Default.Send<ApplicationBarMessage>(new ApplicationBarMessage(item));
        }

        private BarItemViewModel Item(BarItem item)
        {
            return new BarItemViewModel
                {
                    Command = command,
                    CommandParameter = item,
                    IconUri = Resources.ApplicationBar.ResourceManager.GetString(string.Format("{0}_IconUri", item)),
                    Text = Resources.ApplicationBar.ResourceManager.GetString(item.ToString())
                };
        }
    }
}

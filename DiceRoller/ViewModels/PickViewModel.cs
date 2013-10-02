using DiceRoller.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace DiceRoller.ViewModels
{
    public class PickViewModel : ViewModelBase
    {
        private ObservableCollection<PoolComponent> poolComponents;
        private ICommand holdCommand;
        private ICommand tapCommand;

        public PickViewModel()
        {
            foreach (DiceType type in Enum.GetValues(typeof(DiceType)))
            {
                PoolComponents.Add(new PoolComponent(type));
            }
        }

        public ObservableCollection<PoolComponent> PoolComponents
        {
            get { return poolComponents ?? (poolComponents = new ObservableCollection<PoolComponent>()); }
        }

        public ICommand HoldCommand
        {
            get { return holdCommand ?? (holdCommand = new RelayCommand<DiceType>(OnHold)); }
        }

        public ICommand TapCommand
        {
            get { return tapCommand ?? (tapCommand = new RelayCommand<DiceType>(OnTap)); }
        }

        private void OnHold(DiceType type)
        {
            var component = PoolComponents.SingleOrDefault(x => x.Type == type);
            if (component != null)
            {
                component.Count += 5;
            }
        }

        private void OnTap(DiceType type)
        {
            var component = PoolComponents.SingleOrDefault(x => x.Type == type);
            if (component != null)
            {
                component.Count += 1;
            }
        }
    }
}

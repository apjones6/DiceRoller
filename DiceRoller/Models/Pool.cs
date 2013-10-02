using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;

namespace DiceRoller.Models
{
    public class Pool : ObservableObject
    {
        private ObservableCollection<PoolComponent> dice = new ObservableCollection<PoolComponent>();
        private string name;

        public Pool()
        {
            foreach (DiceType type in Enum.GetValues(typeof(DiceType)))
            {
                Dice.Add(new PoolComponent(type));
            }
        }

        public ObservableCollection<PoolComponent> Dice
        {
            get { return dice; }
            set
            {
                dice = value;
                RaisePropertyChanged("Dice");
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }
    }
}

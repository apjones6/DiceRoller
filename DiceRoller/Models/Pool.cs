using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace DiceRoller.Models
{
    public class Pool : ObservableObject
    {
        private ObservableCollection<PoolComponent> dice;
        private string name;

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

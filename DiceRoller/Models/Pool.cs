using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;
using System.Linq;

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

        public Pool(Pool pool)
        {
            Dice = new ObservableCollection<PoolComponent>(pool.Dice.Select(x => new PoolComponent(x)));
            Name = pool.Name;
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

        public string DiceExpression
        {
            get
            {
                var components = Dice
                    .Where(x => x.Count > 0)
                    .OrderByDescending(x => (int)x.Type)
                    .Select(x => string.Concat(x.Count != 1 ? x.Count.ToString() : string.Empty, x.Type))
                    .ToArray();
                return string.Join(" + ", components);
            }
        }

        public string DisplayName
        {
            get { return Name ?? DiceExpression; }
        }
    }
}

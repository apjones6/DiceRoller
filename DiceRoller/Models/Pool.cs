using GalaSoft.MvvmLight;
using System;
using System.ComponentModel;
using System.Linq;

namespace DiceRoller.Models
{
    public class Pool : ObservableObject
    {
        private static readonly DiceType[] DEFAULT_DICE = Enum.GetValues(typeof(DiceType)).Cast<DiceType>().OrderByDescending(x => x).ToArray();
        private PoolComponent[] dice;
        private string name;

        public Pool()
        {
            Dice = DEFAULT_DICE.Select(x => new PoolComponent(x)).ToArray();
        }

        public Pool(string expression, string name = null)
        {
            var components = expression
                .Split('+')
                .Select(x => new PoolComponent(x.Trim()))
                .ToArray();
            Dice = DEFAULT_DICE
                .Select(x => components.SingleOrDefault(c => c.Type == x) ?? new PoolComponent(x))
                .ToArray();
            Name = name;
        }

        public Pool(Pool pool)
        {
            Dice = pool.Dice.Select(x => new PoolComponent(x)).ToArray();
            Name = pool.Name;
        }

        public int this[DiceType type]
        {
            get { return Dice.Single(x => x.Type == type).Count; }
            set { Dice.Single(x => x.Type == type).Count = value; }
        }

        public PoolComponent[] Dice
        {
            get { return dice; }
            private set
            {
                dice = value;
                foreach (var d in dice)
                {
                    d.PropertyChanged += OnPoolComponentPropertyChanged;
                }
            }
        }

        public int DiceCount
        {
            get { return Dice.Sum(x => x.Count); }
        }

        public string DiceExpression
        {
            get
            {
                var components = Dice
                    .Where(x => x.Count > 0)
                    .OrderByDescending(x => (int)x.Type)
                    .Select(x => x.ToString())
                    .ToArray();
                return string.Join(" + ", components);
            }
        }

        public string DisplayName
        {
            get { return IsDefaultName ? DiceExpression : Name; }
        }

        public bool IsDefaultName
        {
            get { return string.IsNullOrWhiteSpace(Name); }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged("DisplayName");
                    RaisePropertyChanged("IsDefaultName");
                    RaisePropertyChanged("Name");
                }
            }
        }

        public override string ToString()
        {
            return DisplayName;
        }

        private void OnPoolComponentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("DiceCount");
            RaisePropertyChanged("DiceExpression");
            if (IsDefaultName)
            {
                RaisePropertyChanged("DisplayName");
            }
        }
    }
}

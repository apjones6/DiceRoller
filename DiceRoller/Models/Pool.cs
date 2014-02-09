using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

namespace DiceRoller.Models
{
    [DataContract]
    public class Pool : INotifyPropertyChanged
    {
        private static readonly DiceType[] DEFAULT_DICE = Enum.GetValues(typeof(DiceType)).Cast<DiceType>().OrderByDescending(x => x).ToArray();

        private PoolComponent[] dice;
        private string expression;
        private bool favorite;
        private string name;

        public Pool()
        {
            DiceExpression = string.Empty;
        }

        public Pool(string expression, string name = null)
        {
            DiceExpression = expression;
            Name = name;
        }

        public Pool(Pool pool)
        {
            Dice = pool.Dice.Select(x => new PoolComponent(x)).ToArray();
            Name = pool.Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

        [DataMember]
        public string DiceExpression
        {
            get
            {
                if (dice == null)
                {
                    return string.Empty;
                }
                
                if (expression == null)
                {
                    var components = Dice
                        .Where(x => x.Count > 0)
                        .OrderByDescending(x => (int)x.Type)
                        .Select(x => x.ToString())
                        .ToArray();
                    expression = string.Join(" + ", components);
                }

                return expression;
            }
            set
            {
                if (expression != value)
                {
                    var components = value == string.Empty ? new PoolComponent[0] : value.Split('+').Select(x => new PoolComponent(x.Trim())).ToArray();
                    Dice = DEFAULT_DICE
                        .Select(x => components.SingleOrDefault(c => c.Type == x) ?? new PoolComponent(x))
                        .ToArray();
                }
            }
        }

        public string DisplayName
        {
            get { return IsDefaultName ? DiceExpression : Name; }
        }

        [DataMember]
        public bool Favorite
        {
            get { return favorite; }
            set
            {
                if (favorite != value)
                {
                    favorite = value;
                    RaisePropertyChanged("Favorite");
                }
            }
        }

        public bool IsDefaultName
        {
            get { return string.IsNullOrWhiteSpace(Name); }
        }

        [DataMember]
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
            // Need to recalculate, but do it lazily
            expression = null;

            RaisePropertyChanged("DiceCount");
            RaisePropertyChanged("DiceExpression");
            if (IsDefaultName)
            {
                RaisePropertyChanged("DisplayName");
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

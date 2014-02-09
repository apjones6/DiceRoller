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
        private string expression = string.Empty;
        private bool favorite;
        private string name;

        public Pool(string expression, string name = null)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            // NOTE: This setter sets the dice counts; don't directly set the field
            DiceExpression = expression;
            Name = name;
        }

        public Pool(Pool pool)
            : this(pool.DiceExpression, pool.name)
        {
        }

        public Pool()
            : this(string.Empty)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int this[DiceType type]
        {
            get { return dice.Single(x => x.Type == type).Count; }
            set { dice.Single(x => x.Type == type).Count = value; }
        }

        public PoolComponent[] Dice
        {
            get { return dice; }
        }

        public int DiceCount
        {
            get { return dice.Sum(x => x.Count); }
        }

        [DataMember]
        public string DiceExpression
        {
            get
            {
                if (expression == null)
                {
                    var components = dice
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
                // When deserialized from isolated storage, no constructors apply
                if (dice == null)
                {
                    InitializeDice();
                }

                if (expression != value)
                {
                    // Update the count for each dice
                    // NOTE: This will encounter issues if/when we support dynamic dice, where we
                    //       won't want to include every dice type in every pool
                    var components = value != string.Empty
                        ? value.Split('+').Select(x => new PoolComponent(x.Trim())).ToArray()
                        : new PoolComponent[0];
                    foreach (var die in dice)
                    {
                        var match = components.SingleOrDefault(x => x.Type == die.Type);
                        die.Count = match != null
                            ? match.Count
                            : 0;
                    }

                    expression = value;
                    RaisePropertyChanged("DiceExpression");
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

        private void InitializeDice()
        {
#if DEBUG
            if (dice != null)
            {
                throw new InvalidOperationException("Unexpected call to Pool.InitializeDice()");
            }
#endif
            dice = DEFAULT_DICE
                .OrderByDescending(x => (int)x)
                .Select(x => new PoolComponent(x))
                .ToArray();

            foreach (var die in dice)
            {
                die.PropertyChanged += OnPoolComponentPropertyChanged;
            }
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

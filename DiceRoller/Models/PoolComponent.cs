using GalaSoft.MvvmLight;
using System;
using System.Text.RegularExpressions;

namespace DiceRoller.Models
{
    public class PoolComponent : ObservableObject
    {
        private static readonly Regex EXPRESSION_SPLITTER = new Regex("^(?<count>\\d*)(?<type>D(4|6|8|10|12|20|100))$");
        private readonly DiceType type;
        private int count;

        public PoolComponent(DiceType type = DiceType.D6, int count = 0)
        {
            this.type = type;
            this.count = count;
        }

        public PoolComponent(string expression)
        {
            var match = EXPRESSION_SPLITTER.Match(expression);
            if (!match.Success)
            {
                throw new ArgumentException(string.Format("Could not parse the expression '{0}'.", expression), "expression");
            }

            this.count = match.Groups["count"].Length == 1 ? int.Parse(match.Groups["count"].Value) : 1;
            this.type = (DiceType)Enum.Parse(typeof(DiceType), match.Groups["type"].Value);
        }

        public PoolComponent(PoolComponent component)
            : this(component.type, component.count)
        {
        }

        public DiceType Type
        {
            get { return type; }
        }

        public int Count
        {
            get { return count; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, "Count must be a non-negative integer.");
                }

                if (count != value)
                {
                    count = value;
                    RaisePropertyChanged("Count");
                }
            }
        }

        public override string ToString()
        {
            if (count > 0)
            {
                return string.Concat(count == 1 ? string.Empty : count.ToString(), type);
            }

            return string.Empty;
        }
    }
}

using Microsoft.Phone.Controls.Primitives;
using System;
using System.Windows.Controls;

namespace DiceRoller.Models
{
    public class IntegerSelectorDataSource : ILoopingSelectorDataSource
    {
        private readonly int minimum;
        private readonly int maximum;
        private int selected;

        public IntegerSelectorDataSource(int selected = 0)
            : this(selected, 0, 99)
        {
        }

        public IntegerSelectorDataSource(int selected, int maximum)
            : this(selected, 0, maximum)
        {
        }

        public IntegerSelectorDataSource(int selected, int minimum, int maximum)
        {
            if (maximum < minimum)
            {
                throw new ArgumentException("Maximum must be equal or greater than minimum.", "maximum");
            }

            if (selected < minimum || selected > maximum)
            {
                throw new ArgumentOutOfRangeException("selected", selected, string.Format("Selected value must be between {0} and {1}.", minimum, maximum));
            }

            this.selected = selected;
            this.minimum = minimum;
            this.maximum = maximum;
        }

        public object GetNext(object relativeTo)
        {
            var next = (int)relativeTo + 1;
            if (next > maximum)
            {
                return minimum;
            }

            return next;
        }

        public object GetPrevious(object relativeTo)
        {
            var next = (int)relativeTo - 1;
            if (next < minimum)
            {
                return maximum;
            }

            return next;
        }

        public object SelectedItem
        {
            get { return selected; }
            set
            {
                if (value.GetType() != typeof(int))
                {
                    throw new ArgumentException("SelectedItem must be a valid integer value.");
                }

                selected = (int)value;
                if (SelectionChanged != null)
                {
                    SelectionChanged(this, new SelectionChangedEventArgs(new object[0], new object[0]));
                }
            }
        }

        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;
    }
}

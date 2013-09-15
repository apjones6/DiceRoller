using System;
using System.Linq;
using System.Windows.Input;

namespace DiceRoller.ViewModels
{
    public class DieTypeViewModel : BaseViewModel
    {
        #region Fields

        private int count;
        private int maximum;
        private int[] results;
        private int sum;

        #endregion

        #region Constructors

        public DieTypeViewModel(int maximum, int count = 0, int[] results = null)
        {
            this.count = count;
            this.maximum = maximum;
            if (results != null)
            {
                this.results = results;
                this.sum = results.Sum();
            }
            else
            {
                this.results = new int[0];
                this.sum = 0;
            }
        }

        #endregion

        #region Properties

        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                if (value != count)
                {
                    count = value;
                    NotifyPropertyChanged("DisplayCount");
                    NotifyPropertyChanged("Count");
                }
            }
        }

        #endregion

        #region Readonly Properties

        public string DisplayName
        {
            get
            {
                return "D" + maximum;
            }
        }

        public string DisplayCount
        {
            get
            {
                return count > 0 ? count.ToString() : null;
            }
        }

        public int Maximum
        {
            get
            {
                return maximum;
            }
        }

        public int[] Results
        {
            get
            {
                return results;
            }
        }

        public int Sum
        {
            get
            {
                return sum;
            }
        }

        #endregion

        #region Methods

        public DieTypeViewModel Roll(Random random)
        {
            var results = Enumerable.Repeat(0, count).Select(x => random.Next(1, maximum)).OrderByDescending(x => x).ToArray();
            return new DieTypeViewModel(maximum, count, results);
        }

        #endregion
    }
}
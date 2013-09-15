using System;
using System.Windows.Media;

namespace DiceRoller.ViewModels
{
    public class RollViewModel : SelectableViewModel
    {
        #region Fields

        private string poolName;
        private DateTime createdDate;
        private int sum;
        private bool favorite;

        #endregion

        #region Constructors

        public RollViewModel()
        {
            this.createdDate = DateTime.Now;
        }

        public RollViewModel(int sum, string poolName = null)
            : this()
        {
            this.poolName = poolName;
            this.sum = sum;
        }

        #endregion

        #region Properties

        public string PoolName
        {
            get
            {
                return poolName;
            }
            set
            {
                if (value != poolName)
                {
                    poolName = value;
                    NotifyPropertyChanged("PoolName");
                }
            }
        }

        public DateTime CreatedDate
        {
            get
            {
                return createdDate;
            }
            set
            {
                if (value != createdDate)
                {
                    createdDate = value;
                    NotifyPropertyChanged("ConciseCreatedDate");
                    NotifyPropertyChanged("CreatedDate");
                }
            }
        }

        public int Sum
        {
            get
            {
                return sum;
            }
            set
            {
                if (value != sum)
                {
                    sum = value;
                    NotifyPropertyChanged("Sum");
                }
            }
        }

        public bool Favorite
        {
            get
            {
                return favorite;
            }
            set
            {
                if (value != favorite)
                {
                    favorite = value;
                    NotifyPropertyChanged("Favorite");
                }
            }
        }

        #endregion

        #region Readonly Properties

        public string ConciseCreatedDate
        {
            get
            {
                return CreatedDate.ToString("HH:mm");
            }
        }

        public Brush PoolNameBrush
        {
            get
            {
                return Favorite ? PhoneAccentBrush : PhoneDisabledBrush;
            }
        }

        public Brush BorderBrush
        {
            get
            {
                return Selected ? PhoneAccentBrush : null;
            }
        }

        #endregion
    }
}
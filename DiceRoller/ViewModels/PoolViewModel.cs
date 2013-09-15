using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace DiceRoller.ViewModels
{
    public class PoolViewModel : SelectableViewModel
    {
        #region Fields

        private string poolName;
        private string poolDescription;
        private bool favorite;

        private IDictionary<int, DieTypeViewModel> dice;

        #endregion

        #region Constructors

        public PoolViewModel(string poolName = null)
        {
            this.poolName = poolName;
            this.dice = new Dictionary<int, DieTypeViewModel>
                {
                    { 4, new DieTypeViewModel(4) },
                    { 6, new DieTypeViewModel(6) },
                    { 8, new DieTypeViewModel(8) },
                    { 10, new DieTypeViewModel(10) },
                    { 12, new DieTypeViewModel(12) },
                    { 20, new DieTypeViewModel(20) },
                    { 100, new DieTypeViewModel(100) }
                };
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

        public string PoolDescription
        {
            get
            {
                return poolDescription;
            }
            set
            {
                if (value != poolDescription)
                {
                    poolDescription = value;
                    NotifyPropertyChanged("PoolDescription");
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

        public Brush BorderBrush
        {
            get { return Selected ? PhoneAccentBrush : null; }
        }

        // TODO: Use a generic system with something like ObservableCollection for this
        public DieTypeViewModel D4 { get { return dice[4]; } }
        public DieTypeViewModel D6 { get { return dice[6]; } }
        public DieTypeViewModel D8 { get { return dice[8]; } }
        public DieTypeViewModel D10 { get { return dice[10]; } }
        public DieTypeViewModel D12 { get { return dice[12]; } }
        public DieTypeViewModel D20 { get { return dice[20]; } }
        public DieTypeViewModel D100 { get { return dice[100]; } }

        #endregion

        #region Methods

        public RollViewModel Roll(Random random)
        {
            return new RollViewModel(25, PoolName);
        }

        #endregion
    }
}
using GalaSoft.MvvmLight;

namespace DiceRoller.Models
{
    public class PoolComponent : ObservableObject
    {
        private DiceType type;
        private int count;

        public PoolComponent()
            : this(DiceType.D6)
        {
        }

        public PoolComponent(DiceType type, int count = 0)
        {
            this.type = type;
            this.count = count;
        }

        public DiceType Type
        {
            get { return type; }
            set
            {
                type = value;
                RaisePropertyChanged("Type");
            }
        }

        public int Count
        {
            get { return count; }
            set
            {
                count = value;
                RaisePropertyChanged("Count");
            }
        }
    }
}

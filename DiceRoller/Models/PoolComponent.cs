using GalaSoft.MvvmLight;

namespace DiceRoller.Models
{
    public class PoolComponent : ObservableObject
    {
        private readonly DiceType type;
        private int count;

        public PoolComponent(DiceType type = DiceType.D6, int count = 0)
        {
            this.type = type;
            this.count = count;
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
                count = value;
                RaisePropertyChanged("Count");
            }
        }
    }
}

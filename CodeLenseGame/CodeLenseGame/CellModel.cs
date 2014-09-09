using System.ComponentModel;
using System.Runtime.CompilerServices;
using CodeLenseGame.Annotations;

namespace CodeLenseGame
{
    public class CellModel : INotifyPropertyChanged
    {
        private int _value;
        private int _x;
        private int _y;

        public bool IsEmpty { get; private set; }

        public CellModel(int x, int y)
            : this(0, x, y)
        {
            IsEmpty = true;
        }

        public CellModel(int value, int x, int y)
        {
            Value = value;
            _x = x;
            _y = y;
        }

        public int X
        {
            get { return _x; }
            set
            {
                if (_x != value)
                {
                    _x = value;
                    OnPropertyChanged("X");
                }
            }
        }

        public int Y
        {
            get { return _y; }
            set
            {
                if (_y != value)
                {
                    _y = value;
                    OnPropertyChanged("Y");
                }
            }
        }

        public int Value
        {
            get { return _value; }
            set
            {
                if (value != _value)
                {
                    _value = value;
                    OnPropertyChanged("Value"); 
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
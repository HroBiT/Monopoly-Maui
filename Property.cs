using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MonopolyBoard
{
    public class Property : INotifyPropertyChanged
    {
        private string _name;
        private int _cost;
        private int _rent;
        private Player _owner;
        private string _color;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public int Cost
        {
            get => _cost;
            set
            {
                _cost = value;
                OnPropertyChanged();
            }
        }

        public int Rent
        {
            get => _rent;
            set
            {
                _rent = value;
                OnPropertyChanged();
            }
        }

        public Player Owner
        {
            get => _owner;
            set
            {
                _owner = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsOwned));
            }
        }

        public string Color
        {
            get => _color;
            set
            {
                _color = value;
                OnPropertyChanged();
            }
        }

        public bool IsOwned => Owner != null;

        public Property(string name, int cost, int rent, string color)
        {
            Name = name;
            Cost = cost;
            Rent = rent;
            Color = color;
            Owner = null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;


        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

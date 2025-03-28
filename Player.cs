using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Graphics;

namespace MonopolyBoard
{
    public class Player : INotifyPropertyChanged
    {
        private int _money;
        private int _position;
        private Color _color;

        public string Name { get; }

        public int Money
        {
            get => _money;
            set
            {
                _money = value;
                OnPropertyChanged();
            }
        }

        public int Position
        {
            get => _position;
            set
            {
                _position = value;
                OnPropertyChanged();
            }
        }

        public Color Color 
        {
            get => _color;
            set
            {
                _color = value;
                OnPropertyChanged();
            }
        }

        public Player(string name, int red, int green, int blue)
        {
            Name = name;
            Money = 1500;
            Position = 0;
            Color = new Color(red / 255f, green / 255f, blue / 255f); 
        }

        public void ThrowDice()
        {
            Random rnd = new Random();
            int dice1 = rnd.Next(1, 7);
            int dice2 = rnd.Next(1, 7);
            Move(dice1 + dice2);
        }

        public void Move(int steps)
        {
            Position += steps;
            if (Position >= 28) 
            {
                Position -= 28;
                Money += 200;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

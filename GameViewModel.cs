using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace MonopolyBoard
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private int _currentRound;
        private string _playerInfo;
        private int _currentPlayerIndex;
        private Square _currentSquare;
        private bool _canBuyProperty;
        private bool _isTurnActive; // Dodana właściwość

        private (int red, int green, int blue)[] _availableColors = {
            (255, 0, 0),   // Red
            (0, 0, 255),   // Blue
            (255, 255, 0), // Yellow
            (128, 0, 128)  // Purple
        };

        public ObservableCollection<Player> Players { get; private set; }
        public List<Square> Squares { get; private set; }

        public int CurrentRound
        {
            get => _currentRound;
            set
            {
                _currentRound = value;
                OnPropertyChanged();
                UpdatePlayerInfo();
            }
        }

        public string PlayerInfo
        {
            get => _playerInfo;
            set
            {
                _playerInfo = value;
                OnPropertyChanged();
            }
        }

        public int CurrentPlayerIndex
        {
            get => _currentPlayerIndex;
            set
            {
                _currentPlayerIndex = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentPlayer));
                CheckCurrentSquare();
            }
        }

        public Player CurrentPlayer => Players.Count > 0 ? Players[CurrentPlayerIndex] : null;

        public Square CurrentSquare
        {
            get => _currentSquare;
            set
            {
                _currentSquare = value;
                OnPropertyChanged();
            }
        }

        public bool CanBuyProperty
        {
            get => _canBuyProperty;
            set
            {
                _canBuyProperty = value;
                OnPropertyChanged();
            }
        }

        public bool IsTurnActive 
        {
            get => _isTurnActive;
            set
            {
                _isTurnActive = value;
                OnPropertyChanged();
            }
        }

        public ICommand PlayRoundCommand { get; }
        public ICommand BuyPropertyCommand { get; }
        public ICommand EndTurnCommand { get; }

        public GameViewModel()
        {
            Players = new ObservableCollection<Player>();
            Squares = new List<Square>();
            CurrentRound = 0;
            CurrentPlayerIndex = 0;
            PlayRoundCommand = new Command(PlayRound, () => !IsTurnActive);
            BuyPropertyCommand = new Command(BuyProperty, () => CanBuyProperty && IsTurnActive);
            EndTurnCommand = new Command(EndTurn); 
            InitializeGame();
            UpdatePlayerInfo();
            IsTurnActive = false;
        }

        private void InitializeGame()
        {
            Players.Add(new Player("Gracz 1", _availableColors[0].red, _availableColors[0].green, _availableColors[0].blue));
            Players.Add(new Player("Gracz 2", _availableColors[1].red, _availableColors[1].green, _availableColors[1].blue));

            InitializeBoard();
        }

        private void InitializeBoard()
        {
            Squares.Add(new Square(0, "Start", SquareType.Go));

            Squares.Add(new Square(1, "Poznań", SquareType.Property,
                new Property("Poznań", 60, 2, "#8B4513")));
            Squares.Add(new Square(2, "Kasa społeczna", SquareType.CommunityChest));
            Squares.Add(new Square(3, "Warszawa", SquareType.Property,
                new Property("Warszawa", 60, 4, "#8B4513")));
            Squares.Add(new Square(4, "Podatek dochodowy", SquareType.Tax));
            Squares.Add(new Square(5, "Dworzec Centralny", SquareType.Property,
                new Property("Dworzec Centralny", 200, 25, "#000000")));
            Squares.Add(new Square(6, "Kraków", SquareType.Property,
                new Property("Kraków", 100, 6, "#87CEEB")));

            Squares.Add(new Square(7, "Więzienie", SquareType.Jail));


            Squares.Add(new Square(8, "Wrocław", SquareType.Property,
                new Property("Wrocław", 140, 10, "#FF69B4")));
            Squares.Add(new Square(9, "Elektrownia", SquareType.Property,
                new Property("Elektrownia", 150, 15, "#FFFFFF")));
            Squares.Add(new Square(10, "Łódź", SquareType.Property,
                new Property("Łódź", 140, 10, "#FF69B4")));
            Squares.Add(new Square(11, "Gdańsk", SquareType.Property,
                new Property("Gdańsk", 160, 12, "#FF69B4")));
            Squares.Add(new Square(12, "Dworzec Gdański", SquareType.Property,
                new Property("Dworzec Gdański", 200, 25, "#000000")));
            Squares.Add(new Square(13, "Szczecin", SquareType.Property,
                new Property("Szczecin", 180, 14, "#FFA500")));

            Squares.Add(new Square(14, "Darmowy parking", SquareType.FreeParking));

            Squares.Add(new Square(15, "Bydgoszcz", SquareType.Property,
                new Property("Bydgoszcz", 220, 18, "#FF0000")));
            Squares.Add(new Square(16, "Szansa", SquareType.Chance));
            Squares.Add(new Square(17, "Lublin", SquareType.Property,
                new Property("Lublin", 220, 18, "#FF0000")));
            Squares.Add(new Square(18, "Katowice", SquareType.Property,
                new Property("Katowice", 240, 20, "#FF0000")));
            Squares.Add(new Square(19, "Dworzec Zachodni", SquareType.Property,
                new Property("Dworzec Zachodni", 200, 25, "#000000")));
            Squares.Add(new Square(20, "Białystok", SquareType.Property,
                new Property("Białystok", 260, 22, "#FFFF00")));

            Squares.Add(new Square(21, "Idź do więzienia", SquareType.GoToJail));

            Squares.Add(new Square(22, "Częstochowa", SquareType.Property,
                new Property("Częstochowa", 300, 26, "#008000")));
            Squares.Add(new Square(23, "Kielce", SquareType.Property,
                new Property("Kielce", 300, 26, "#008000")));
            Squares.Add(new Square(24, "Kasa społeczna", SquareType.CommunityChest));
            Squares.Add(new Square(25, "Rzeszów", SquareType.Property,
                new Property("Rzeszów", 320, 28, "#008000")));
            Squares.Add(new Square(26, "Dworzec Wschodni", SquareType.Property,
                new Property("Dworzec Wschodni", 200, 25, "#000000")));
            Squares.Add(new Square(27, "Podatek od luksusu", SquareType.Tax));
        }

        public void PlayRound()
        {
            if (CurrentPlayer != null && !IsTurnActive)
            {
                IsTurnActive = true;
                CurrentPlayer.ThrowDice();
                CheckCurrentSquare();
                UpdatePlayerInfo();
                ((Command)PlayRoundCommand).ChangeCanExecute();
                ((Command)BuyPropertyCommand).ChangeCanExecute();
            }
        }

        private void EndTurn()
        {
            IsTurnActive = false;
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
            CheckCurrentSquare();
            UpdatePlayerInfo();
            ((Command)PlayRoundCommand).ChangeCanExecute();
            ((Command)BuyPropertyCommand).ChangeCanExecute();
        }

        private void CheckCurrentSquare()
        {
            if (CurrentPlayer == null) return;

            int position = CurrentPlayer.Position;
            CurrentSquare = Squares[position];

            CanBuyProperty = CurrentSquare.Type == SquareType.Property &&
                              CurrentSquare.Property != null &&
                              !CurrentSquare.Property.IsOwned &&
                              CurrentPlayer.CanBuyProperty(CurrentSquare.Property);

            if (CurrentSquare.Type == SquareType.Property &&
                CurrentSquare.Property != null &&
                CurrentSquare.Property.IsOwned &&
                CurrentSquare.Property.Owner != CurrentPlayer)
            {
                CurrentPlayer.PayRent(CurrentSquare.Property);
            }
            UpdatePlayerInfo();
            ((Command)BuyPropertyCommand).ChangeCanExecute();
        }

        private void BuyProperty()
        {
            if (CurrentPlayer != null && CurrentSquare != null &&
                CurrentSquare.Type == SquareType.Property &&
                CurrentSquare.Property != null &&
                !CurrentSquare.Property.IsOwned &&
                CurrentPlayer.CanBuyProperty(CurrentSquare.Property))
            {
                CurrentPlayer.BuyProperty(CurrentSquare.Property);
                CanBuyProperty = false;
                UpdatePlayerInfo();
            }
            ((Command)BuyPropertyCommand).ChangeCanExecute();
        }


        private void UpdatePlayerInfo()
        {
            PlayerInfo = "";
            foreach (var player in Players)
            {
                PlayerInfo += $"{player.Name}: ${player.Money}, Pozycja: {player.Position}";
                if (player.OwnedProperties.Count > 0)
                {
                    PlayerInfo += ", Działki: ";
                    foreach (var property in player.OwnedProperties)
                    {
                        PlayerInfo += $"{property.Name}, ";
                    }
                    PlayerInfo = PlayerInfo.TrimEnd(',', ' ');
                }
                PlayerInfo += " | ";
            }
            PlayerInfo = PlayerInfo.TrimEnd(' ', '|', ' ');
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

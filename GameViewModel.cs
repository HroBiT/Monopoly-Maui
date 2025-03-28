using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Graphics;

namespace MonopolyBoard
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private int _currentRound;
        private string _playerInfo;
        private int _currentPlayerIndex;
        private (int red, int green, int blue)[] _availableColors = {
            (255, 0, 0),   
            (0, 0, 255),   
            (255, 255, 0), 
            (128, 0, 128)  
        };

        public ObservableCollection<Player> Players { get; private set; }
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
            }
        }

        public Player CurrentPlayer => Players.Count > 0 ? Players[CurrentPlayerIndex] : null;

        public ICommand PlayRoundCommand { get; }

        public GameViewModel()
        {
            Players = new ObservableCollection<Player>();
            CurrentRound = 0;
            CurrentPlayerIndex = 0;
            PlayRoundCommand = new Command(PlayRound);
            InitializeGame();
            UpdatePlayerInfo();
        }

        private void InitializeGame()
        {
            Players.Add(new Player("Player 1", _availableColors[0].red, _availableColors[0].green, _availableColors[0].blue));
            Players.Add(new Player("Player 2", _availableColors[1].red, _availableColors[1].green, _availableColors[1].blue));
        }

        public void PlayRound()
        {
            CurrentRound++;

            Player currentPlayer = CurrentPlayer;

            if (currentPlayer != null)
            {
                currentPlayer.ThrowDice();
            }

            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;

            UpdatePlayerInfo();
        }

        private void UpdatePlayerInfo()
        {
            PlayerInfo = "";
            foreach (var player in Players)
            {
                PlayerInfo += $"{player.Name}: Money = {player.Money}, Position = {player.Position} | ";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace MonopolyBoard
{
    public partial class MainPage : ContentPage
    {
        private GameViewModel _viewModel;
        private Dictionary<Player, Border> _playerTokens = new Dictionary<Player, Border>();

        public MainPage()
        {
            InitializeComponent();
            _viewModel = (GameViewModel)BindingContext;
            _viewModel.Players.CollectionChanged += (sender, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (Player player in e.NewItems)
                    {
                        CreatePlayerToken(player);
                    }
                }
            };
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, EventArgs e)
        {
            CreatePlayerTokens();
        }

        private void CreatePlayerTokens()
        {
            foreach (var player in _viewModel.Players)
            {
                CreatePlayerToken(player);
            }
        }

        private void CreatePlayerToken(Player player)
        {
            var token = new Border
            {
                Stroke = Colors.Black,
                StrokeThickness = 1,
                BackgroundColor = player.Color,
                WidthRequest = 20,
                HeightRequest = 20,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            _playerTokens[player] = token;
            BoardGrid.Children.Add(token);
            UpdatePlayerPosition(player);

            player.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(player.Position))
                {
                    UpdatePlayerPosition(player);
                }
            };
        }

        private void UpdatePlayerPosition(Player player)
        {
            int row = 0, column = 0;
            GetRowColumnFromPosition(player.Position, out row, out column);

            if (_playerTokens.ContainsKey(player))
            {
                Border token = _playerTokens[player];
                Grid.SetRow(token, row);
                Grid.SetColumn(token, column);
            }
        }

        private void GetRowColumnFromPosition(int position, out int row, out int column)
        {
            row = 0;
            column = 0;

            if (position >= 0 && position <= 7)
            {
                row = 0;
                column = position;
            }
            else if (position > 7 && position <= 13)
            {
                row = position - 7;
                column = 7;
            }
            else if (position > 13 && position <= 20)
            {
                row = 7;
                column = 20 - position;
            }
            else
            {
                row = 27 - position;
                column = 0;
            }
        }

        private void UpdatePropertyVisualization(Property property)
        {
            if (property != null && property.IsOwned)
            {
                // Znajdź odpowiednie pole na planszy na podstawie nazwy
                var square = _viewModel.Squares.FirstOrDefault(s => s.Property == property);
                if (square != null)
                {
                    int row = 0, column = 0;
                    GetRowColumnFromPosition(square.Position, out row, out column);

                    // Znajdź Border dla tego pola
                    var borders = BoardGrid.Children.OfType<Border>();
                    var border = borders.FirstOrDefault(b =>
                        Grid.GetRow(b) == row && Grid.GetColumn(b) == column);

                    if (border != null)
                    {
                        // Dodaj kolorowy pasek na górze Border, aby zaznaczyć własność
                        border.Stroke = property.Owner.Color;
                        border.StrokeThickness = 5;
                    }
                }
            }
        }

    }
}

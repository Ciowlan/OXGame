using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;

namespace OXGame
{
    public partial class MainPage : ContentPage
    {
        private List<Button> buttons;
        private char currentPlayer;
        private bool gameEnded;

        public ICommand CellClickCommand { get; }

        public MainPage()
        {
            InitializeComponent();
            CellClickCommand = new Command<string>(OnCellClick);
            InitializeGame();

            // 初始化 Reset 按鈕點擊事件
            reset.Clicked += OnResetClicked;
        }

        private void InitializeGame()
        {
            buttons = new List<Button>
            {
                btn1, btn2, btn3,
                btn4, btn5, btn6,
                btn7, btn8, btn9
            };

            foreach (var button in buttons)
            {
                
                button.CommandParameter = button.AutomationId;
                button.Command = CellClickCommand;
            }

            ResetGame();
        }

        private void OnCellClick(string parameter)
        {
            if (!gameEnded)
            {
                var button = buttons.FirstOrDefault(btn => btn.AutomationId == parameter);

                if (button != null && string.IsNullOrEmpty(button.Text))
                {
                    button.Text = currentPlayer.ToString();

                    if (CheckForWinner())
                    {
                        DisplayAlert("Game Over", $"{currentPlayer} wins!", "OK");
                        ResetGame();
                    }
                    else if (buttons.All(btn => !string.IsNullOrEmpty(btn.Text)))
                    {
                        DisplayAlert("Game Over", "It's a draw!", "OK");
                        ResetGame();
                    }
                    else
                    {
                        currentPlayer = (currentPlayer == 'X') ? 'O' : 'X';
                    }
                }
            }
        }

        private bool CheckForWinner()
        {
            // Check rows, columns, and diagonals for a win
            for (int i = 0; i < 3; i++)
            {
                if (buttons[i * 3].Text == currentPlayer.ToString() && buttons[i * 3 + 1].Text == currentPlayer.ToString() && buttons[i * 3 + 2].Text == currentPlayer.ToString())
                    return true; // Check rows

                if (buttons[i].Text == currentPlayer.ToString() && buttons[i + 3].Text == currentPlayer.ToString() && buttons[i + 6].Text == currentPlayer.ToString())
                    return true; // Check columns
            }

            if (buttons[0].Text == currentPlayer.ToString() && buttons[4].Text == currentPlayer.ToString() && buttons[8].Text == currentPlayer.ToString())
                return true; // Check diagonal (top-left to bottom-right)

            if (buttons[2].Text == currentPlayer.ToString() && buttons[4].Text == currentPlayer.ToString() && buttons[6].Text == currentPlayer.ToString())
                return true; // Check diagonal (top-right to bottom-left)

            return false;
        }

        private void ResetGame()
        {
            foreach (var button in buttons)
            {
                button.Text = "";
            }

            currentPlayer = 'X';
            gameEnded = false;
        }

        private void OnResetClicked(object sender, EventArgs e)
        {
            ResetGame();
        }
    }
}

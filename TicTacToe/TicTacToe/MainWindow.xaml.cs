using System.Windows.Media;
using System.Linq;
using System;
using System.Windows;
using System.Windows.Controls;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Project by Carl Hamilton to display a simple Tic Tack Toe game.
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Members

        /// <summary>
        /// Holds the current results of cells in the active game
        /// </summary>
        private MarkType[] mResults;

        /// <summary>
        /// True if it is player 1's turn (X) or player 2's turn (O)
        /// </summary>
        private bool mPlayer1Turn;

        /// <summary>
        /// True if the game has ended
        /// </summary>
        private bool mGameEnded;


        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            NewGame();

        }



        #endregion

        /// <summary>
        /// Start a new game and clears all values back to the start
        /// </summary>
        private void NewGame()
        {
            mResults = new MarkType[9];

            // Set initial value of free for the array (grid) in tic tac toe
            for (int i = 0; i < mResults.Length; i++)
                mResults[i] = MarkType.Free;

            // Make sure that player 1 starts the game
            mPlayer1Turn = true;


            // Iterate every button on the grid
            Container.Children.Cast<Button>().ToList().ForEach(button =>
            {
                button.Content = string.Empty;
                button.Background = System.Windows.Media.Brushes.Gray;

            });

            //make sure the game hasn't finished
            mGameEnded = false;
        }

        #region Private Functions 
        /// <summary>
        /// Handles a button click event
        /// </summary>
        /// <param name="sender">the button that was clicked</param>
        /// <param name="e">The event of the click</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Check if game has ended
            if (mGameEnded)
            {
                NewGame();
                return;
            }

            // assign the sender to a variable
            var button = (Button)sender;
            // Find the button's position in the array
            var column = Grid.GetColumn(button);
            var row = Grid.GetRow(button);
            var index = column + (row * Container.RowDefinitions.Count);

            // Don't do anything if the cell already has a value in it
            if (mResults[index] != MarkType.Free)
                return;

            // Set the cell value based on which players turn it it
            mResults[index] = mPlayer1Turn ? MarkType.Cross : MarkType.Nought;

            // Show the result
            switch (mResults[index])
            {
                case MarkType.Nought:
                    button.Content = "O";
                    button.Foreground = Brushes.GreenYellow;
                    break;
                case MarkType.Cross:
                    button.Content = "X";
                    button.Foreground = Brushes.Navy;
                    break;
            }


            // Change player turn
            mPlayer1Turn ^= true;

            // Check for end of a game or a winner
            CheckForWinner();

        }

        /// <summary>
        /// Checks for a winner or a draw, and reset the game
        /// </summary>
        private void CheckForWinner()
        {

            #region No Winners

            // Check for no winner and full board
            if (!mResults.Any(f => f == MarkType.Free))
            {
                // Game ended
                mGameEnded = true;

                // Turn all cells orange
                Container.Children.Cast<Button>().ToList().ForEach(button =>
                {
                    button.Background = Brushes.Orange;
                });
                return;
            }
            #endregion
            else
                winnerCheck(); // algorithm to check for winner

            #endregion



        }

        private void winnerCheck()
        {
            var winColor = Brushes.Salmon;

            // initiating the strings that will be checked if it has similar characters
            string rowCheck = string.Empty;
            string colCheck = string.Empty;
            string diagCheck = string.Empty;
            string inverseDiagCheck = string.Empty;

            // iterating throughout the matrix
            for (int i = 0; i < mResults.Length / 3; i++)
            {
                for (int k = 0; k < mResults.Length / 3; k++)
                {
                    // Check if we have vertical winner
                    colCheck += GetCellContent(i, k);
                    if ((!colCheck.Contains("X") || !colCheck.Contains("O")) & colCheck.Length >= 3)
                    {
                        mGameEnded = true;
                        // Show message for the winner 
                        ShowWinnerMessage();
                        ShowFinalMessage();
                        return;
                    }

                    //Check if we have horizontal winner 
                    rowCheck += GetCellContent(k, i);
                    if ((!rowCheck.Contains("X") || !rowCheck.Contains("O")) & rowCheck.Length >= 3)
                    {
                        mGameEnded = true;
                        // Show message for the winner 
                        ShowWinnerMessage();
                        ShowFinalMessage();
                        return;
                    }



                }
                rowCheck = string.Empty;
                colCheck = string.Empty;

                // Check if we have normal diagonal winner
                diagCheck += GetCellContent(i, i);
                if ((!diagCheck.Contains("X") || !diagCheck.Contains("O")) & diagCheck.Length >= 3)
                {
                    mGameEnded = true;
                    // Show message for the winner 
                    ShowWinnerMessage();
                    ShowFinalMessage();

                    return;

                }

                // Check if we have inverse diagonal winner
                inverseDiagCheck += GetCellContent(i, (mResults.Length / 3) - 1 - i);
                if ((!inverseDiagCheck.Contains("X") || !inverseDiagCheck.Contains("O")) & inverseDiagCheck.Length >= 3)
                {
                    mGameEnded = true;
                    // Show message for the winner 
                    ShowWinnerMessage();
                    ShowFinalMessage();

                    return;

                }


            }
        }

        private static void ShowFinalMessage()
        {
            MessageBox.Show("Press any square to start a new game");
        }

        /// <summary>
        /// method to check who is the winner and return a message 
        /// </summary>
        private void ShowWinnerMessage()
        {
            switch (mPlayer1Turn)
            {
                case true:

                    MessageBox.Show("Player 2 Wins !!!");
                    break;
                case false:
                    MessageBox.Show("Player 1 Wins !!!");
                    break;
            }
        }

        /// <summary>
        /// method to get cell value based on row/column 
        /// </summary>
        /// <param name="C1"></param>
        /// <param name="C2"></param>
        /// <returns></returns>
        string GetCellContent(int C1, int C2)
        {
            return Container.Children.Cast<Button>().First(e => Grid.GetRow(e) == C1 && Grid.GetColumn(e) == C2).Content.ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Oscilloscope_Tic_Tac_Toe
{
    class TicTacToe : TicTacToeGraphics
    {
        public List<Point> board { get; set; }
        public List<Point> player { get; set; }
        public bool gameEnded { get; set; }
        private PlayerMarker[,] BoardMap;
        private Point PlayerPosition;
        private PlayerMarker PlayerTurn;

        public TicTacToe()
        {
            BoardMap = new PlayerMarker[3, 3];
            board = new List<Point>();
            player = new List<Point>();
            ResetGame();
        }

        public void playerMove(KeyEventArgs e)
        {
            player.Clear();
            if (e.KeyCode != Keys.R && !gameEnded)
            {

                // sets offset position according to user's input
                if (e.KeyCode == Keys.Down && PlayerPosition.Y - 30 > -31) PlayerPosition.Y -= 30;
                else if (e.KeyCode == Keys.Up && PlayerPosition.Y + 30 < 31) PlayerPosition.Y += 30;
                else if (e.KeyCode == Keys.Left && PlayerPosition.X - 30 > -34) PlayerPosition.X -= 30;
                else if (e.KeyCode == Keys.Right && PlayerPosition.X + 30 < 34) PlayerPosition.X += 30;

                // gives the player their new coordinates
                if (!gameEnded) player.AddRange(GetPlayerPiece(PlayerTurn, PlayerPosition, true)); // character's piece

                // Place the player's piece on the board if another is not already there
                if (e.KeyCode == Keys.Space && BoardMap[(PlayerPosition.X + 30) / 30, (PlayerPosition.Y + 30) / 30] == PlayerMarker.None)
                {
                    // adds the player's piece to the board
                    board.AddRange(GetPlayerPiece(PlayerTurn, PlayerPosition, false));

                    player.Clear();

                    BoardMap[(PlayerPosition.X + 30) / 30, (PlayerPosition.Y + 30) / 30] = PlayerTurn;

                    // checks that the game has ended
                    gameEnded = checkGameEnd();

                    if (!gameEnded)
                    {
                        // start next player's move in the middle of the screen
                        PlayerPosition = new Point(0, 0);

                        // changes to player 2
                        ChangePlayerTurns();

                        // sets the next player's piece in the middle
                        player.AddRange(GetPlayerPiece(PlayerTurn, PlayerPosition, true));
                    }
                }
            }
            else if (e.KeyCode == Keys.R) ResetGame();
        }

        public void ResetGame()
        {
            board.Clear();
            board.AddRange(GetEmptyBoardPoints());
            PlayerTurn = PlayerMarker.PlayerX;
            PlayerPosition = new Point(0, 0);
            player.AddRange(GetPlayerPiece(PlayerTurn, PlayerPosition, true));

            // Set board array to all zeroes
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    BoardMap[i, j] = PlayerMarker.None;
                }
            }

            gameEnded = false;
        }

        private void ChangePlayerTurns()
        {
            if (PlayerTurn == PlayerMarker.PlayerX) 
            {
                PlayerTurn = PlayerMarker.PlayerO;
            }
            else
            {
                PlayerTurn = PlayerMarker.PlayerX;
            }
        }

        // CHECK END OF GAME
        public Boolean checkGameEnd()
        {
            // Detects a victory in any columns
            for (int c = 0; c <= 2; c++)
            {
                if (BoardMap[c,0] != 0 && BoardMap[c, 0] == BoardMap[c, 1] && BoardMap[c, 0] == BoardMap[c, 2])
                {
                    // Adds the victory line to the board display
                    board.AddRange(GetVictoryLinePoints(c, 0));
                    return true;
                }
            }

            // Detects a victory in any rows
            for (int r = 0; r <= 2; r++)
            {
                if (BoardMap[0, r] != 0 && BoardMap[0, r] == BoardMap[1, r] && BoardMap[0, r] == BoardMap[2, r])
                {
                    // Adds the victory line to the board display
                    board.AddRange(GetVictoryLinePoints(r, 1));
                    return true;
                }
            }

            // Detects a diagonal victory
            if (BoardMap[0, 0] != 0 && BoardMap[0, 0] == BoardMap[1, 1] && BoardMap[0, 0] == BoardMap[2, 2])
            {
                board.AddRange(GetVictoryLinePoints(0, 2));
                return true;
            }
            else if (BoardMap[0, 2] != 0 && BoardMap[0, 2] == BoardMap[1, 1] && BoardMap[0, 2] == BoardMap[2, 0])
            {
                board.AddRange(GetVictoryLinePoints(0, 3));
                return true;
            }

            Boolean e = true;
            // Detects a draw
            for (int i = 0; i <= 2; i++) for (int j = 0; j <= 2; j++) if (BoardMap[i, j] == 0) e = false;
            
            // If it reaches this, there is a draw
            return e;
        }
    }
}

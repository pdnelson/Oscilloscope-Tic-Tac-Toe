using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Oscilloscope_Tic_Tac_Toe
{
    class OscilloscopeTicTacToe : TicTacToeGraphics
    {
        public bool gameEnded { get; set; }
        private List<Point> BoardGraphics;
        private List<Point> PlayerGraphics;
        private PlayerMarker[,] BoardMap;
        private Point PlayerPosition;
        private PlayerMarker PlayerTurn;

        public OscilloscopeTicTacToe()
        {
            BoardMap = new PlayerMarker[3, 3];
            BoardGraphics = new List<Point>();
            PlayerGraphics = new List<Point>();
            ResetGame();
        }

        public bool playerMove(Keys keyPress)
        {
            if (keyPress == Keys.R) ResetGame();
            else if (!gameEnded)
            {
                PlayerGraphics.Clear();

                // sets offset position according to user's input
                if (keyPress == Keys.Down && PlayerPosition.Y - 30 > -31) PlayerPosition.Y -= 30;
                else if (keyPress == Keys.Up && PlayerPosition.Y + 30 < 31) PlayerPosition.Y += 30;
                else if (keyPress == Keys.Left && PlayerPosition.X - 30 > -34) PlayerPosition.X -= 30;
                else if (keyPress == Keys.Right && PlayerPosition.X + 30 < 34) PlayerPosition.X += 30;


                // Place the player's piece on the board if another is not already there
                else if (keyPress == Keys.Space && BoardMap[(PlayerPosition.X + 30) / 30, (PlayerPosition.Y + 30) / 30] == PlayerMarker.None)
                {
                    // adds the player's piece to the board
                    BoardGraphics.AddRange(GetPlayerPiece(PlayerTurn, PlayerPosition, false));

                    PlayerGraphics.Clear();

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
                        PlayerGraphics.AddRange(GetPlayerPiece(PlayerTurn, PlayerPosition, true));
                    }
                }
                else
                {
                    return false; // The player cannot place a piece over top of another player's
                }

                // gives the player their new coordinates
                if (!gameEnded) PlayerGraphics.AddRange(GetPlayerPiece(PlayerTurn, PlayerPosition, true)); // character's piece
            }

            return true;
        }

        public void ResetGame()
        {
            BoardGraphics.Clear();
            BoardGraphics.AddRange(GetEmptyBoardPoints());
            PlayerTurn = PlayerMarker.PlayerX;
            PlayerPosition = new Point(0, 0);
            PlayerGraphics.AddRange(GetPlayerPiece(PlayerTurn, PlayerPosition, true));

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
        public bool checkGameEnd()
        {
            // Detects a victory in any columns
            for (int c = 0; c <= 2; c++)
            {
                if (BoardMap[c,0] != 0 && BoardMap[c, 0] == BoardMap[c, 1] && BoardMap[c, 0] == BoardMap[c, 2])
                {
                    // Adds the victory line to the board display
                    BoardGraphics.AddRange(GetVictoryLinePoints(c, 0));
                    return true;
                }
            }

            // Detects a victory in any rows
            for (int r = 0; r <= 2; r++)
            {
                if (BoardMap[0, r] != 0 && BoardMap[0, r] == BoardMap[1, r] && BoardMap[0, r] == BoardMap[2, r])
                {
                    // Adds the victory line to the board display
                    BoardGraphics.AddRange(GetVictoryLinePoints(r, 1));
                    return true;
                }
            }

            // Detects a diagonal victory
            if (BoardMap[0, 0] != 0 && BoardMap[0, 0] == BoardMap[1, 1] && BoardMap[0, 0] == BoardMap[2, 2])
            {
                BoardGraphics.AddRange(GetVictoryLinePoints(0, 2));
                return true;
            }
            else if (BoardMap[0, 2] != 0 && BoardMap[0, 2] == BoardMap[1, 1] && BoardMap[0, 2] == BoardMap[2, 0])
            {
                BoardGraphics.AddRange(GetVictoryLinePoints(0, 3));
                return true;
            }

            bool e = true;
            // Detects a draw
            for (int i = 0; i <= 2; i++) for (int j = 0; j <= 2; j++) if (BoardMap[i, j] == 0) e = false;
            
            // If it reaches this, there is a draw
            return e;
        }

        public List<Point> GetCurrentGameGraphics()
        {
            List<Point> currBoard = new List<Point>();
            currBoard.AddRange(BoardGraphics);
            currBoard.AddRange(PlayerGraphics);
            return currBoard;
        }
    }
}

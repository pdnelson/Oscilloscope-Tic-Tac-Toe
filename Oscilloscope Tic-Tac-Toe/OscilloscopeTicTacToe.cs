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

        /// <summary>
        /// Interprets keyboard commands into player movements, or a game reset.
        /// </summary>
        /// <param name="keyPress">The key the user pressed.</param>
        /// <returns>true if the move was successful; false otherwise</returns>
        public bool SendKeyCommand(Keys keyPress)
        {
            if (keyPress == Keys.R) ResetGame();
            else if (!gameEnded)
            {
                PlayerGraphics.Clear();

                // Validate that the user's movement is within the bounds of the board
                if      (keyPress == Keys.Down  && PlayerPosition.Y - 1 >= 0)   PlayerPosition.Y -= 1;
                else if (keyPress == Keys.Up    && PlayerPosition.Y + 1 <= 2)   PlayerPosition.Y += 1;
                else if (keyPress == Keys.Left  && PlayerPosition.X - 1 >= 0)   PlayerPosition.X -= 1;
                else if (keyPress == Keys.Right && PlayerPosition.X + 1 <= 2)   PlayerPosition.X += 1;

                // Place the player's piece on the board if another is not already there
                else if (keyPress == Keys.Space && BoardMap[PlayerPosition.X, PlayerPosition.Y] == PlayerMarker.None)
                {
                    // adds the player's piece to the board
                    BoardGraphics.AddRange(GetPlayerPiece(PlayerTurn, PlayerPosition, false));

                    PlayerGraphics.Clear();

                    BoardMap[PlayerPosition.X, PlayerPosition.Y] = PlayerTurn;

                    // checks that the game has ended
                    gameEnded = CheckGameStatus();

                    if (!gameEnded)
                    {
                        // start next player's move in the middle of the screen
                        PlayerPosition = new Point(1, 1);

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

        /// <summary>
        /// Resets the game, setting all fields back to their default values.
        /// </summary>
        private void ResetGame()
        {
            BoardGraphics.Clear();
            BoardGraphics.AddRange(GetEmptyBoardPoints());
            PlayerTurn = PlayerMarker.PlayerX;
            PlayerPosition = new Point(1, 1);
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

        /// <summary>
        /// Changes which player is currently active.
        /// </summary>
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

        /// <summary>
        /// Checks that the game has ended, and adds a victory line on the board if a victory is detected.
        /// </summary>
        /// <returns>true if the game has ended; false otherwise</returns>
        private bool CheckGameStatus()
        {
            // Check all rows for a vertical victory
            for (int c = 0; c <= 2; c++)
            {
                if (BoardMap[c,0] != 0 && BoardMap[c, 0] == BoardMap[c, 1] && BoardMap[c, 0] == BoardMap[c, 2])
                {
                    BoardGraphics.AddRange(GetVictoryLinePoints(c, VictoryOrientation.Vertical));
                    return true;
                }
            }

            // Check all rows for a horizontal victory
            for (int r = 0; r <= 2; r++)
            {
                if (BoardMap[0, r] != 0 && BoardMap[0, r] == BoardMap[1, r] && BoardMap[0, r] == BoardMap[2, r])
                {
                    BoardGraphics.AddRange(GetVictoryLinePoints(r, VictoryOrientation.Horizontal));
                    return true;
                }
            }

            // Check for a diagonal victory (top to bottom)
            if (BoardMap[0, 0] != 0 && BoardMap[0, 0] == BoardMap[1, 1] && BoardMap[0, 0] == BoardMap[2, 2])
            {
                BoardGraphics.AddRange(GetVictoryLinePoints(0, VictoryOrientation.DiagonalTopToBottom));
                return true;
            }

            // Check for a diagonal victory (bottom to top)
            else if (BoardMap[0, 2] != 0 && BoardMap[0, 2] == BoardMap[1, 1] && BoardMap[0, 2] == BoardMap[2, 0])
            {
                BoardGraphics.AddRange(GetVictoryLinePoints(0, VictoryOrientation.DiagonalBottomToTop));
                return true;
            }

            bool isDraw = true;

            // Check if there are any open spaces. If there are none, the game has reached a draw.
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    if (BoardMap[i, j] == PlayerMarker.None) isDraw = false;
                }
            }
            
            return isDraw;
        }

        /// <summary>
        /// Consolidates the game's graphics to be displayed on the oscilloscope.
        /// </summary>
        /// <returns>Game graphics to be displayed on the oscilloscope.</returns>
        public List<Point> GetCurrentGameGraphics()
        {
            List<Point> currBoard = new List<Point>();
            currBoard.AddRange(BoardGraphics);
            currBoard.AddRange(PlayerGraphics);
            return currBoard;
        }
    }
}

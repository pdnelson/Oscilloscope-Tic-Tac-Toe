using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Oscilloscope_Tic_Tac_Toe
{
    class Game
    {
        ScopeGraphics sg;
        private List<Point> board;
        private List<Point> player;
        private int[,] simBoard;
        private int oX;
        private int oY;
        private int turn;
        private Boolean gameEnded;

        public Game()
        {
            // Initialize the points
            sg = new ScopeGraphics();
            board = new List<Point>();
            player =  new List<Point>();
            simBoard = new int[3, 3];
            oX = 0;
            oY = 0;
            turn = 1;

            // Fill in the graphics
            board.AddRange(sg.getBoard());
            player.AddRange(sg.getChar(1, 0, 0, true));
        }

        // PLAYER MOVE
        public void playerMove(KeyEventArgs e)
        {
            player.Clear();
            if (e.KeyCode != Keys.R && !gameEnded)
            {

                // sets offset position according to user's input
                if (e.KeyCode == Keys.Down && oY - 30 > -31) oY -= 30;
                else if (e.KeyCode == Keys.Up && oY + 30 < 31) oY += 30;
                else if (e.KeyCode == Keys.Left && oX - 30 > -34) oX -= 30;
                else if (e.KeyCode == Keys.Right && oX + 30 < 34) oX += 30;

                // gives the player their new coordinates
                if (!gameEnded) player.AddRange(sg.getChar(turn, oX, oY, true)); // character's piece

                // places the player's piece on the board
                if (e.KeyCode == Keys.Space && simBoard[(oX + 30) / 30, (oY + 30) / 30] == 0)
                {
                    // adds the player's piece to the board
                    board.AddRange(sg.getChar(turn, oX, oY, false));

                    player.Clear();

                    simBoard[(oX + 30) / 30, (oY + 30) / 30] = turn;

                    // checks that the game has ended
                    gameEnded = checkGameEnd();

                    if (!gameEnded)
                    {
                        // start next player's move in the middle of the screen
                        returnPlayerToOrigin();

                        // changes to player 2
                        changeTurn();

                        // sets the next player's piece in the middle
                        player.AddRange(sg.getChar(turn, 0, 0, true)); // character's piece
                    }
                }
            }
            else if (e.KeyCode == Keys.R) resetGame();
        }

        // RESET GAME
        public void resetGame()
        {
            board.Clear();
            board.AddRange(sg.getBoard());
            turn = 1;
            oX = 0;
            oY = 0;
            player.AddRange(sg.getChar(turn, 0, 0, true)); // character's piece

            // Set board array to all zeroes
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    simBoard[i, j] = 0;
                }
            }

            gameEnded = false;
        }

        // CHANGE TURN
        public void changeTurn()
        {
            if (turn == 1) turn++;
            else turn--;
        }

        // RETURN PLAYER TO ORIGIN
        public void returnPlayerToOrigin()
        {
            oX = 0;
            oY = 0;
        }

        // GET GAME BOARD
        public List<Point> getBoard()
        {
            return board;
        }

        // GET PLAYER
        public List<Point> getPlayer()
        {
            return player;
        }

        // CHECK END OF GAME
        public Boolean checkGameEnd()
        {
            // Detects a victory in any columns
            for (int c = 0; c <= 2; c++)
            {
                if (simBoard[c,0] != 0 && simBoard[c, 0] == simBoard[c, 1] && simBoard[c, 0] == simBoard[c, 2])
                {
                    // Adds the victory line to the board display
                    board.AddRange(sg.getVictoryLine(c, 0));
                    return true;
                }
            }

            // Detects a victory in any rows
            for (int r = 0; r <= 2; r++)
            {
                if (simBoard[0, r] != 0 && simBoard[0, r] == simBoard[1, r] && simBoard[0, r] == simBoard[2, r])
                {
                    // Adds the victory line to the board display
                    board.AddRange(sg.getVictoryLine(r, 1));
                    return true;
                }
            }

            // Detects a diagonal victory
            if (simBoard[0, 0] != 0 && simBoard[0, 0] == simBoard[1, 1] && simBoard[0, 0] == simBoard[2, 2])
            {
                board.AddRange(sg.getVictoryLine(0, 2));
                return true;
            }
            else if (simBoard[0, 2] != 0 && simBoard[0, 2] == simBoard[1, 1] && simBoard[0, 2] == simBoard[2, 0])
            {
                board.AddRange(sg.getVictoryLine(0, 3));
                return true;
            }

            Boolean e = true;
            // Detects a draw
            for (int i = 0; i <= 2; i++) for (int j = 0; j <= 2; j++) if (simBoard[i, j] == 0) e = false;
            
            // If it reaches this, there is a draw
            return e;
        }

        // RETURN GAME STATUS
        public Boolean getGameStatus()
        {
            return gameEnded;
        }
    }
}

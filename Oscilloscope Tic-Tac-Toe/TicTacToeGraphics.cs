using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Oscilloscope_Tic_Tac_Toe
{
    class TicTacToeGraphics
    {

        /// <summary>
        /// Returns a character based on the player's turn,
        /// whether or not they're moving, and positioning offset
        /// </summary>
        /// <param name="playerTurn">The player whose turn it is.</param>
        /// <param name="playerPosition">The position the player's piece is on the board.</param>
        /// <param name="playerMoving">Tells if that piece is the one that the player is currently moving.</param>
        /// <returns></returns>
        public List<Point> GetPlayerPiece(PlayerMarker playerTurn, Point playerPosition, bool playerMoving) {
            List<Point> points = new List<Point>();

            /**
             * (position - 1) * 30 is to account for the offset between the board position and position to draw the characters on the oscilloscope
             **/

            if (playerTurn == PlayerMarker.PlayerX)
            {
                points.AddRange(GetXPoints((playerPosition.X - 1) * 30, (playerPosition.Y - 1) * 30));
            }
            else if (playerTurn == PlayerMarker.PlayerO)
            {
                points.AddRange(GetOPoints((playerPosition.X - 1) * 30, (playerPosition.Y - 1) * 30));
            }

            if (playerMoving)
            {
                points.AddRange(GetSquarePoints((playerPosition.X - 1) * 30, (playerPosition.Y - 1) * 30));
            }

            return points;
        }

        /// <summary>
        /// Returns X character based on offset
        /// </summary>
        public List<Point> GetXPoints(int scopePositionX, int scopePositionY)
        {
            List<Point> points = new List<Point>();
            // x
            // draw half of x
            for (int i = -11; i < 11; i++) points.Add(new Point(i + scopePositionX, i + scopePositionY));

            // makes it so that the lines don't get in the way as much
            points.Add(new Point(scopePositionX, scopePositionY));
            points.Add(new Point(scopePositionX, scopePositionY));

            // draw the other half of x
            for (int i = 11; i > -11; i--) points.Add(new Point(i + scopePositionX, -1 * i + scopePositionY));

            // makes it so that the lines don't get in the way as much
            points.Add(new Point(scopePositionX, scopePositionY));
            points.Add(new Point(scopePositionX, scopePositionY));

            return points;
        }

        /// <summary>
        /// Returns O character based on offset
        /// </summary>
        public List<Point> GetOPoints(int scopePositionX, int scopePositionY)
        {
            List<Point> points = new List<Point>();

            // draws a circle
            for (int i = -30; i < 10; i++) points.Add(new Point((int)(11 * Math.Cos(i) + scopePositionX), (int)(11 * Math.Sin(i) + scopePositionY)));

            return points;
        }

        /// <summary>
        /// Returns square based on offset. This is used to indicate a piece that the player is currently moving.
        /// </summary>
        public List<Point> GetSquarePoints(int scopePositionX, int scopePositionY)
        {
            List<Point> points = new List<Point>();

            for (int i = 11; i > -11; i--) points.Add(new Point(i + scopePositionX, 11 + scopePositionY));
            for (int i = 11; i > -11; i--) points.Add(new Point(-11 + scopePositionX, i + scopePositionY));
            for (int i = -11; i < 11; i++) points.Add(new Point(i + scopePositionX, -11 + scopePositionY));
            for (int i = -11; i < 11; i++) points.Add(new Point(11 + scopePositionX, i + scopePositionY));
            for (int i = 11; i > -11; i--) points.Add(new Point(i + scopePositionX, 11 + scopePositionY));

            return points;
        }

        /// <summary>
        /// Returns a blank game board.
        /// </summary>
        /// <returns></returns>
        public List<Point> GetEmptyBoardPoints()
        {
            List<Point> points = new List<Point>();

            // vertical lines
            for (int i = -50; i < 50; i++) points.Add(new Point(15, i));
            for (int i = 50; i > -50; i--) points.Add(new Point(-15, i));
            
            // draw a dot in the bottom left corner
            // this keeps the lines out of the way of the board
            points.Add(new Point(-60, -60));
            
            // horizontal lines
            for (int i = -50; i < 50; i++) points.Add(new Point(i, 15));
            for (int i = 50; i > -50; i--) points.Add(new Point(i, -15));
            
            // draw a dot in the bottom left corner
            points.Add(new Point(-60, -60));

            return points;
        }

        /// <summary>
        /// Will return a line across the screen indicating a victory.
        /// </summary>
        /// <param name="position">The position the victory line is in.</param>
        /// <param name="orientation">The orientation the victory line is in.</param>
        /// <returns></returns>
        public List<Point> GetVictoryLinePoints(int position, VictoryOrientation orientation)
        {
            List<Point> points = new List<Point>(); 

            if (orientation == VictoryOrientation.Vertical) for (int i = -50; i < 50; i++) points.Add(new Point((position * 30) - 30, i));

            else if (orientation == VictoryOrientation.Horizontal) for (int i = -50; i < 50; i++) points.Add(new Point(i, (position * 30) - 30));
                
            else if (orientation == VictoryOrientation.DiagonalBottomToTop) for (int i = -50; i < 50; i++) points.Add(new Point( -1 * i, i));

            else if (orientation == VictoryOrientation.DiagonalTopToBottom) for (int i = -50; i < 50; i++) points.Add(new Point(i, i));

            return points;
        }
    }
}

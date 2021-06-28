using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Oscilloscope_Tic_Tac_Toe
{
    class ScopeGraphics
    {
        // Returns a character based on the player's turn,
        // whether or not they're moving, and positioning offset
        public List<Point> getChar(int turn, int X, int Y, Boolean move) {
            List<Point> points = new List<Point>();

            if (turn == 1) points.AddRange(getX(X, Y));
            else if (turn == 2) points.AddRange(getO(X, Y));

            if (move) points.AddRange(getBox(X, Y));

            return points;
        }

        // Returns X character based on offset
        public List<Point> getX(int X, int Y)
        {
            List<Point> points = new List<Point>();
            // x
            // draw half of x
            for (int i = -11; i < 11; i++) points.Add(new Point(i + X, i + Y));

            // makes it so that the lines don't get in the way as much
            points.Add(new Point(0 + X, 0 + Y));
            points.Add(new Point(0 + X, 0 + Y));

            // draw the other half of x
            for (int i = 11; i > -11; i--) points.Add(new Point(i + X, -1 * i + Y));

            // makes it so that the lines don't get in the way as much
            points.Add(new Point(0 + X, 0 + Y));
            points.Add(new Point(0 + X, 0 + Y));

            return points;
        }

        // Returns O character based on offset
        public List<Point> getO(int X, int Y)
        {
            List<Point> points = new List<Point>();

            // draws a circle
            for (int i = -30; i < 10; i++) points.Add(new Point((int)(11 * Math.Cos(i) + X), (int)(11 * Math.Sin(i) + Y)));

            return points;
        }
        
        // Returns square based on offset
        public List<Point> getBox(int X, int Y)
        {
            List<Point> points = new List<Point>();

            for (int i = 11; i > -11; i--) points.Add(new Point(i + X, 11 + Y));
            for (int i = 11; i > -11; i--) points.Add(new Point(-11 + X, i + Y));
            for (int i = -11; i < 11; i++) points.Add(new Point(i + X, -11 + Y));
            for (int i = -11; i < 11; i++) points.Add(new Point(11 + X, i + Y));
            for (int i = 11; i > -11; i--) points.Add(new Point(i + X, 11 + Y));

            return points;
        }

        // Returns a blank game board
        public List<Point> getBoard()
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

        // Returns a victory line based on offset and orientation
        public List<Point> getVictoryLine(int p, int o)
        {
            List<Point> points = new List<Point>(); 

            // Vertical
            if (o == 0) for (int i = -50; i < 50; i++) points.Add(new Point((p * 30) - 30, i));

            // Horizontal
            else if (o == 1) for (int i = -50; i < 50; i++) points.Add(new Point(i, (p * 30) - 30));
                
            // Diagonal Bottom to Top
            else if (o == 3) for (int i = -50; i < 50; i++) points.Add(new Point( -1 * i, i));

            // Diagonal Top to Bottom
            else if (o == 2) for (int i = -50; i < 50; i++) points.Add(new Point(i, i));

            return points;
        }
    }
}

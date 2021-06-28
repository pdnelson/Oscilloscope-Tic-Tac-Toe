using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Media;

namespace Oscilloscope_Tic_Tac_Toe
{
    public partial class frmTicTacToe : Form
    {
        TicTacToe TicTacToeGame;
        OscilloscopeWavePlayer WavePlayer;

        public frmTicTacToe()
        {
            InitializeComponent();
            TicTacToeGame = new TicTacToe();
            WavePlayer = new OscilloscopeWavePlayer();
            WavePlayer.ImageAmplificationX = 500;
            WavePlayer.ImageAmplificationY = 500;
        }

        private void frmTicTacToe_Load(object sender, EventArgs e)
        {
            List<Point> totalPoints = new List<Point>();
            totalPoints.AddRange(TicTacToeGame.board);
            totalPoints.AddRange(TicTacToeGame.player);

            WavePlayer.BuildAndPlayWaveAsync(totalPoints);
        }

        private void frmTicTacToe_KeyDown(object sender, KeyEventArgs e)
        {

            // Doesn't let the user enter anything else if the game has ended, unless it is the reset function
            if (!TicTacToeGame.gameEnded || e.KeyCode == Keys.R)
            {
                // Updates the board to match player's movement
                TicTacToeGame.playerMove(e);

                List<Point> totalPoints = new List<Point>();
                totalPoints.AddRange(TicTacToeGame.board);
                totalPoints.AddRange(TicTacToeGame.player);

                WavePlayer.BuildAndPlayWaveAsync(totalPoints);
            }
        }
    }
}

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
        OscilloscopeTicTacToe TicTacToeGame;
        OscilloscopeWavePlayer WavePlayer;

        public frmTicTacToe()
        {
            InitializeComponent();
            TicTacToeGame = new OscilloscopeTicTacToe();
            WavePlayer = new OscilloscopeWavePlayer();
            WavePlayer.ImageAmplificationX = 500;
            WavePlayer.ImageAmplificationY = 500;
        }

        private void frmTicTacToe_Load(object sender, EventArgs e)
        {
            WavePlayer.BuildAndPlayWaveAsync(TicTacToeGame.GetCurrentGameGraphics());
        }

        private void frmTicTacToe_KeyDown(object sender, KeyEventArgs e)
        {
            if (TicTacToeGame.SendKeyCommand(e.KeyCode)) {
                WavePlayer.BuildAndPlayWaveAsync(TicTacToeGame.GetCurrentGameGraphics());
            }
        }
    }
}

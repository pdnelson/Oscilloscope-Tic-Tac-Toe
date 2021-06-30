using System;
using System.Windows.Forms;
using System.Threading;

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
                WavePlayer.StopWave();
                WavePlayer.BuildAndPlayWaveAsync(TicTacToeGame.GetCurrentGameGraphics());
            }
            Thread.Sleep(20);
        }
    }
}

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
        // GLOBAL VARIABLES
        BackgroundWorker displayWorker;
        SoundPlayer display;
        Game g;

        // INITIALIZATION
        public frmTicTacToe()
        {
            InitializeComponent();

            // Initialize game
            g = new Game();

            // game display worker
            displayWorker = new BackgroundWorker();
            displayWorker.DoWork += new DoWorkEventHandler(displayWorker_DoWork);
            displayWorker.WorkerSupportsCancellation = true;
        }

        // LOAD FORM
        private void frmTicTacToe_Load(object sender, EventArgs e)
        {
            displayWorker.RunWorkerAsync();
        }

        // GAMEPLAY MECHANICS AND LOGIC
        private void frmTicTacToe_KeyDown(object sender, KeyEventArgs e)
        {

            // Doesn't let the user enter anything else if the game has ended, unless it is the reset function
            if (!g.getGameStatus() || e.KeyCode == Keys.R)
            {
                // First stops the wave
                display.Stop();

                // Updates the board to match player's movement
                g.playerMove(e);

                // Plays the new wave
                if (!displayWorker.IsBusy) displayWorker.RunWorkerAsync();
            }
        }

        // GENERATES WAVE BASED ON PARAMETERS
        // takes in two lists of points
        // intended to be utilized with a BackgroundWorker
        // TODO: Move this into its own class
        public void genWave(List<Point> points, List<Point> playerMove)
        {
            // calculates number of samples we will be storing in the wave
            int samples = 441 * 50 / 10;

            // calculates size of the wave file based on the number of samples
            int bytes = samples * 4;

            // a counter for the number of samples written
            int sampWritten = 0;

            using (MemoryStream MS = new MemoryStream(44 + bytes))
            {
                using (BinaryWriter BW = new BinaryWriter(MS))
                {
                    // WRITING THE WAVE HEADER

                    //  specifies the "RIFF" part
                    BW.Write(Encoding.ASCII.GetBytes("RIFF"), 0, 4);

                    // "chunk" size
                    BW.Write(BitConverter.GetBytes(36 + bytes));

                    // specifies the format, which is "WAVE"
                    BW.Write(Encoding.ASCII.GetBytes("WAVE"), 0, 4);

                    // "Sub-chunk"
                    BW.Write(Encoding.ASCII.GetBytes("fmt "), 0, 4);

                    // "Sub-chunk" size
                    BW.Write(BitConverter.GetBytes(16), 0, 4);

                    // Format; does not indicate compression
                    BW.Write(0X20001);

                    // Number of channels (TWO)
                    BW.Write(44100);

                    // Sample rate
                    BW.Write(176400);

                    // Bit rate
                    BW.Write(0X100004);

                    // Block Align
                    BW.Write(0X61746164);

                    // Bits/sample
                    BW.Write(bytes);

                    // The sample that will get written to the wave
                    short sample = 5;

                    // combines the two lists of points
                    List<Point> combine = new List<Point>();
                    combine.AddRange(points);
                    combine.AddRange(playerMove);

                        // loops the array inside the wave
                        int i = 0;
                        while (sampWritten < samples)
                        {
                            // writes X and Y data on appropriate channels
                            sample = System.Convert.ToInt16(500 * combine[i].Y);
                            BW.Write(sample);
                            sample = System.Convert.ToInt16(500 * combine[i].X);
                            BW.Write(sample);

                            if (i >= (combine.Count - 1)) i = 0;
                            else i++;
                            sampWritten++;
                        }

                    

                    // end-of-the-day clean-up stuff
                    BW.Flush();
                    MS.Seek(0, SeekOrigin.Begin);

                    // plays the shiny, new wave file
                    using (display = new SoundPlayer(MS))
                    {
                        display.PlayLooping();
                    }
                    
                }
            }

        }

        // GAME DISPLAY WORKER
        // this is always running in the background
        public void displayWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            genWave(g.getBoard(), g.getPlayer());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Text;
using System.Threading;

namespace Oscilloscope_Clock
{
    public class OscilloscopeWavePlayer : IDisposable
    {
        private SoundPlayer TimeDisplay;
        private MemoryStream MS;
        private BinaryWriter BW;
        private Thread PlayWaveThread;
        public bool IsPlaying;

        public OscilloscopeWavePlayer()
        {
            IsPlaying = false;
        }

        /// <summary>
        /// Generates a wave based on a point array.
        /// </summary>
        /// <param name="points">Point list being converted to a wave file.</param>
        public void BuildWave(List<Point> points)
        {
            // Byte size of the Wave we are going to be creating
            int bytes = points.Count * 4;

            MS = new MemoryStream(44 + bytes);
            BW = new BinaryWriter(MS);

            //* BEGIN HEADER *//

            //  Specifies the "RIFF" section
            BW.Write(Encoding.ASCII.GetBytes("RIFF"), 0, 4);

            // Chunk size
            BW.Write(BitConverter.GetBytes(36 + bytes));

            // Specifies the format, which is "WAVE"
            BW.Write(Encoding.ASCII.GetBytes("WAVE"), 0, 4);

            // Sub-chunk
            BW.Write(Encoding.ASCII.GetBytes("fmt "), 0, 4);

            // Sub-chunk size
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

            //* END HEADER *//

            // Write each point to the Wave file
            foreach (Point sample in points)
            {
                BW.Write(Convert.ToInt16(sample.Y));
                BW.Write(Convert.ToInt16(sample.X));
            }

            // Prepare Wave to be played
            BW.Flush();
            MS.Seek(0, SeekOrigin.Begin);

        }

        /// <summary>
        /// Plays the built wave.
        /// </summary>
        public void PlayWave()
        {
            if (IsPlaying) StopWave();
            IsPlaying = true;
            using (TimeDisplay = new SoundPlayer(MS))
            {
                TimeDisplay.PlayLooping();
            }
        }

        /// <summary>
        /// Plays the built wave  in a separate thread from the main one.
        /// </summary>
        public void PlayWaveAsync()
        {
            if(PlayWaveThread.IsAlive)
            {
                throw new InvalidOperationException("Cannot play wave async while another wave is playing async.");
            }

            PlayWaveThread = new Thread(() =>
            {
                PlayWave();
            });

            PlayWaveThread.Start();
        }

        /// <summary>
        /// Builds and plays a wave file based on a point array in a separate thread from the main one.
        /// If the wave is already playing, it will be terminated and restarted.
        /// </summary>
        /// <param name="points">The points to be turned into a wave file.</param>
        public void BuildAndPlayWaveAsync(List<Point> points)
        {
            if(IsPlaying) StopWave();
            Dispose();

            PlayWaveThread = new Thread(() =>
            {
                BuildWave(points);
                PlayWave();
            });

            PlayWaveThread.Start();
        }

        /// <summary>
        /// Stops the currently-playing wave file.
        /// </summary>
        public void StopWave()
        {
            if (IsPlaying)
            {
                IsPlaying = false;
                TimeDisplay.Stop();
                if (PlayWaveThread.IsAlive) PlayWaveThread.Join();
            }
        }

        public void Dispose()
        {
            if(BW != null) BW.Dispose();
            if(MS != null) MS.Dispose();
        }
    }
}

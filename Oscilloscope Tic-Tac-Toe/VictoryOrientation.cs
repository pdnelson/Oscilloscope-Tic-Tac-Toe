using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oscilloscope_Tic_Tac_Toe
{
    public enum VictoryOrientation
    {
        /// <summary>
        /// No victory.
        /// </summary>
        None,

        /// <summary>
        /// A vertical victory.
        /// </summary>
        Vertical,

        /// <summary>
        /// A horizontal victory.
        /// </summary>
        Horizontal,

        /// <summary>
        /// A diagonal victory, right to left, bottom to top.
        /// </summary>
        DiagonalBottomToTop,

        /// <summary>
        /// A diagonal victory, right to left, top to bottom.
        /// </summary>
        DiagonalTopToBottom
    }
}

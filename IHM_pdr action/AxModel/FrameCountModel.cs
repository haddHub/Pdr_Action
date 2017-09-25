using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    /// <summary>
    /// Frame count class to count number of packets read/written.
    /// </summary>
    public class FrameCountModel
    {
        /// <summary>
        /// Private array of frame counts.
        /// </summary>
        private int[] frameCounts;

        /// <summary>
        /// Gets or sets the total number of frame count.
        /// </summary>
        public int TotalFrames { get { return frameCounts.Sum(); } }

        /// <summary>
        /// Gets or sets the error Error frame count.
        /// </summary>
        public int ErrorFrames { get { return frameCounts[(int)FrameHeaders.Error]; } set { frameCounts[(int)FrameHeaders.Error] = value; } }

        /// <summary>
        /// Gets or sets the Command frame count.
        /// </summary>
        public int AckStopFrames { get { return frameCounts[(int)FrameHeaders.ACK_Stop]; } set { frameCounts[(int)FrameHeaders.ACK_Stop] = value; } }

        /// <summary>
        /// Gets or sets the Force frame count.
        /// </summary>
        public int ForceFrames { get { return frameCounts[(int)FrameHeaders.Force]; } set { frameCounts[(int)FrameHeaders.Force] = value; } }

        /// <summary>
        /// Gets or sets the Couple frame count.
        /// </summary>
        public int Froce2Frames { get { return frameCounts[(int)FrameHeaders.Force2]; } set { frameCounts[(int)FrameHeaders.Force2] = value; } }

        /// <summary>
        /// Gets or sets the Position frame count.
        /// </summary>
        public int PositionFrames { get { return frameCounts[(int)FrameHeaders.Position]; } set { frameCounts[(int)FrameHeaders.Position] = value; } }

        /// <summary>
        /// Gets or sets the ForceRef frame count.
        /// </summary>
        public int ForceRefFrames { get { return frameCounts[(int)FrameHeaders.ForceRef]; } set { frameCounts[(int)FrameHeaders.ForceRef] = value; } }

        /// <summary>
        /// Gets or sets the ACK_Xdent frame count.
        /// </summary>
        public int ACK_XdentFrames { get { return frameCounts[(int)FrameHeaders.ACK_Xdent]; } set { frameCounts[(int)FrameHeaders.ACK_Xdent] = value; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameCount"/> class.
        /// </summary>
        public FrameCountModel()
        {
           // frameCounts = new int[Enum.GetValues(typeof(FrameHeaders)).Length];
            frameCounts = new int[256];
            Reset();
        }

        /// <summary>
        /// Resets all packet counts to zero.
        /// </summary>
        internal void Reset()
        {
            Array.Clear(frameCounts, 0, frameCounts.Length);
        }
    }
}

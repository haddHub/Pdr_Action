using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class MazeGameModel : aXdataModel
    {
        #region Fields

        private ushort x;
        private ushort y;
        private UniBiCodes uniBi;

        #endregion

        #region Constructors

        public MazeGameModel(ushort x, ushort y, UniBiCodes uB)
        {
            this.x = x;
            this.y = y;
            this.uniBi = uB;
        }

        #endregion

        #region Properties

        public ushort X
        {
            get
            {
                return this.x;
            }
            set
            {
                this.x = value;
            }
        }

        public ushort Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = value;
            }

        }

        public UniBiCodes UniBi
        {
            get
            {
                return this.uniBi;
            }
            set
            {
                this.uniBi = value;
            }
        }

        #endregion

        #region Methods

        public FrameExerciceDataModel MakeFrame()
        {
            FrameExerciceDataModel frame;
            byte xMSB, xLSByMSB, yLSB;
            ushort temp = (ushort)((short)this.x >> 4);
            xMSB = (byte)temp;
            xLSByMSB = (byte)((this.x) << 4 | this.y >> 8);
            yLSB = (byte)this.y;
            frame = new FrameExerciceDataModel(ConfigAddresses.modMazeGame, xMSB, xLSByMSB, yLSB, (byte)this.uniBi);

            return frame;
        }

        #endregion
    }
}

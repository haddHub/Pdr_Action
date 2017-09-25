using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class StreamPointModel : aXdataModel
    {
        #region Fields

        private ushort x;
        private ushort y;
        private byte nbrsPoint;

        #endregion

        #region Constructors

        public StreamPointModel(ushort x, ushort y, byte nP)
        {
            this.x = x;
            this.y = y;
            this.nbrsPoint = nP;
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

        public byte NumberOfThePoint
        {
            get
            {
                return this.nbrsPoint;
            }
            set
            {
                this.nbrsPoint = value;
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
            frame = new FrameExerciceDataModel(ConfigAddresses.StreamingPoint, xMSB, xLSByMSB, yLSB, this.nbrsPoint);

            return frame;
        }

        #endregion
    }
}

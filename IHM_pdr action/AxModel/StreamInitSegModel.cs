using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class StreamInitSegModel : aXdataModel
    {
        #region Fields

        private byte nbrsSeg;
        private byte nbrsPoint;

        #endregion

        #region Constructors

        public StreamInitSegModel(byte nS, byte nP)
        {
            this.nbrsSeg = nS;
            this.nbrsPoint = nP;
        }

        #endregion

        #region Properties

        public byte NumberOfTheSegment
        {
            get
            {
                return this.nbrsSeg;
            }
            set
            {
                this.nbrsSeg = value;
            }
        }

        public byte NumberOfPoints
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
            FrameExerciceDataModel frame = new FrameExerciceDataModel(ConfigAddresses.StreamingSeg, this.nbrsSeg, this.nbrsPoint, 0, 0);
            return frame;
        }

        #endregion
    }
}

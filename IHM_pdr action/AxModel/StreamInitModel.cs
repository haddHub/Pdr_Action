using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class StreamInitModel : aXdataModel
    {
        #region Fields

        private byte nbrsSeg;

        #endregion

        #region Constructors

        public StreamInitModel(byte nS)
        {
            this.nbrsSeg = nS;
        }

        #endregion

        #region Properties

        public byte NbrsSegment
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

        #endregion

        #region Methods

        public FrameExerciceDataModel MakeFrame()
        {
            FrameExerciceDataModel frame = new FrameExerciceDataModel(ConfigAddresses.StreamingInit, this.nbrsSeg, 0, 0, 0);
            return frame;
        }

        #endregion
    }
}

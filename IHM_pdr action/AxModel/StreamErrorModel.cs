using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class StreamErrorModel
    {
        #region Fields

        private ushort error;

        #endregion

        #region Constructors

        public StreamErrorModel(ushort er)
        {
            this.error = er;
        }

        #endregion

        #region Properties

        public ushort Error
        {
            get
            {
                return this.error;
            }
            set
            {
                this.error = value;
            }
        }
        
        #endregion

        #region Methods

        public FrameExerciceDataModel MakeFrame()
        {
            FrameExerciceDataModel frame;
            byte erMSB, erLSB;

            erMSB = (byte)(this.error >> (ushort)8);
            erLSB = (byte)this.error;
            frame = new FrameExerciceDataModel(ConfigAddresses.StreamingError, erMSB, erLSB, 0, 0);

            return frame;
        }

        #endregion
    }
}

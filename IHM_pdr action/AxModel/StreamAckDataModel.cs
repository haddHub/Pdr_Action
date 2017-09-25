using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class StreamAckDataModel : aXdataModel
    {
        #region Fields
        /// <summary>
        /// Gets or sets the config address. See <see cref="FrameHeaders"/>.
        /// </summary>
        public FrameHeaders Address { get; set; }

        /// <summary>
        /// Gets or sets 8-bit DATA 1 & 2 value.
        /// </summary>
        public short Data1 { get; set; }
        public short Data2 { get; set; }

        /// <summary>
        /// Gets or sets 8-bit DATA 3 & 4 value.
        /// </summary>
        public short Data3 { get; set; }
        public short Data4 { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the StreamAckDataModel class.
        /// </summary>
        public StreamAckDataModel(FrameHeaders configAddress, byte data1, byte data2, byte data3, byte data4)
        {
            this.Address = configAddress;
            this.Data1 = data1;
            this.Data2 = data2;
            this.Data3 = data3;
            this.Data4 = data4;
        }

        #endregion

        #region Properties

        #endregion

        #region Methods
        
        #endregion
    }
}

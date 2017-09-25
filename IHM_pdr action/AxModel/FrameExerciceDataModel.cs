using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class FrameExerciceDataModel : aXdataModel
    {
        /// <summary>
        /// Gets or sets the config address. See <see cref="ConfigAddresses"/>.
        /// </summary>
        public ConfigAddresses Address { get; set; }

        /// <summary>
        /// Gets or sets 8-bit DATA 1 value.
        /// </summary>
        public byte Data1 { get; set; }

        /// <summary>
        /// Gets or sets 8-bit DATA 2 value.
        /// </summary>
        public byte Data2 { get; set; }

        /// <summary>
        /// Gets or sets 8-bit DATA 3 value.
        /// </summary>
        public byte Data3 { get; set; }

        /// <summary>
        /// Gets or sets 8-bit DATA 4 value.
        /// </summary>
        public byte Data4 { get; set; }

        public FrameExerciceDataModel()
        {

        }

        public FrameExerciceDataModel(ConfigAddresses configAddress, byte data1, byte data2, byte data3, byte data4)
        {
            this.Address = configAddress;
            this.Data1 = data1;
            this.Data2 = data2;
            this.Data3 = data3;
            this.Data4 = data4;
        }
    }
}

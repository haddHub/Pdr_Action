using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class FrameConfigDataModel : aXdataModel
    {
        /// <summary>
        /// Gets or sets the config address. See <see cref="ConfigAddresses"/>.
        /// </summary>
        public ConfigAddresses Address { get; set; }

        /// <summary>
        /// Gets or sets 16-bit DATA 1 & 2 value.
        /// </summary>
        public short Data1_2 { get; set; }

        /// <summary>
        /// Gets or sets 16-bit DATA 3 & 4 value.
        /// </summary>
        public short Data3_4 { get; set; }

        public FrameConfigDataModel()
        {

        }

        public FrameConfigDataModel(ConfigAddresses configAddress, short data1, short data2)
        {
            this.Address = configAddress;
            this.Data1_2 = data1;
            this.Data3_4 = data2;
        }
    }
}

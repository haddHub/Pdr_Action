using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class ACKDataModel : aXdataModel
    {
        #region Fields
        /// <summary>
        /// Data UART
        /// </summary>
        private byte[] data;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the ACKDataModel class.
        /// </summary>
        /// <param name="d">data array</param>
        public ACKDataModel(byte[] d)
        {
            this.data = d;
        }

        public byte[] Donnee
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

        #endregion

        #region Properties

        #endregion

        #region Methods
        
        #endregion
    }
}

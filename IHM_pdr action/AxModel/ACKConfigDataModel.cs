using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class ACKConfigDataModel : aXdataModel
    {
        #region Fields
        /// <summary>
        /// Data UART
        /// </summary>
        private byte[] data;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the ACKConfigDataModel class.
        /// </summary>
        /// <param name="d"></param>
        public ACKConfigDataModel(byte[] d)
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class CoupleDataModel : aXdataModel
    {
        #region Fields
        /// <summary>
        /// Couple Reçu UART
        /// </summary>
        private int _coupleX, _coupleY;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the CoupleDataModel class.
        /// </summary>
        /// <param name="coX"></param>
        /// <param name="coY"></param>
        public CoupleDataModel(int coX, int coY)
        {
            this._coupleX = coX;
            this._coupleY = coY;
        }

        public int CoupleX
        {
            get
            {
                return _coupleX;
            }
            set
            {
                _coupleX = value;
            }
        }

        public int CoupleY
        {
            get
            {
                return _coupleY;
            }
            set
            {
                _coupleY = value;
            }
        }

        #endregion

        #region Properties
        #endregion

        #region Methods
        #endregion
    }
}

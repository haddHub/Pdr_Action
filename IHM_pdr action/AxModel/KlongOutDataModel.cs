using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class KlongOutDataModel : aXdataModel
    {
        #region Fields
        /// <summary>
        /// Ppr reçue UART
        /// </summary>
        private int _klongOutX, _klongOutY;

        #endregion

        #region Constructors

        public KlongOutDataModel(int klOutX, int klOutY)
        {
            this._klongOutX = klOutX;
            this._klongOutY = klOutY;
        }

        public KlongOutDataModel()
        {
            this._klongOutX = 0;
            this._klongOutY = 0;
        }

        public int KlongOutX
        {
            get
            {
                return _klongOutX;
            }
            set
            {
                _klongOutX = value;
            }
        }

        public int KlongOutY
        {
            get
            {
                return _klongOutY;
            }
            set
            {
                _klongOutY = value;
            }
        }

        #endregion

        #region Properties
        #endregion

        #region Methods
        #endregion
    }
}

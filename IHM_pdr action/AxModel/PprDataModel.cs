using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class PprDataModel : aXdataModel
    {
        #region Fields
        /// <summary>
        /// Ppr reçue UART
        /// </summary>
        private double _pprX, _pprY;

        #endregion

        #region Constructors

        public PprDataModel(int pprX, int pprY)
        {
            this._pprX = pprX;
            this._pprY = pprY;
        }

        public PprDataModel(double pprX, double pprY)
        {
            this._pprX = pprX;
            this._pprY = pprY;
        }

        public PprDataModel()
        {
            this._pprX = 0;
            this._pprY = 0;
        }

        public int PprX
        {
            get
            {
                return (int)_pprX;
            }
            set
            {
                _pprX = value;
            }
        }

        public int PprY
        {
            get
            {
                return (int)_pprY;
            }
            set
            {
                _pprY = value;
            }
        }

        public double PpprXd
        {
            get
            {
                return _pprX;
            }
            set
            {
                _pprX = value;
            }
        }

        public double PprYd
        {
            get
            {
                return _pprY;
            }
            set
            {
                _pprY = value;
            }
        }

        #endregion

        #region Properties
        #endregion

        #region Methods
        #endregion
    }
}

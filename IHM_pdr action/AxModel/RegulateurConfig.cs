using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class RegulateurConfig
    {
        #region Fields

        private byte _kp, _ki, _cGlob;

        #endregion

        #region Constructors

        public RegulateurConfig()
        {
            this._kp = 0;
            this._ki = 8;
            this._cGlob = 15;
        }

        public RegulateurConfig(byte kp, byte ki, byte cg)
        {
            this._kp = kp;
            this._ki = ki;
            this._cGlob = cg;
        }

        #endregion

        #region Properties

        public byte Kp
        {
            get
            {
                return _kp;
            }
            set
            {
                _kp = value;
            }
        }

        public byte Ki
        {
            get
            {
                return _ki;
            }
            set
            {
                _ki = value;
            }
        }

        public byte CGlob
        {
            get
            {
                return _cGlob;
            }
            set
            {
                _cGlob = value;
            }
        }


        #endregion

        #region Methods

        #endregion

        #region RelayCommand

        #endregion

        #region Actions

        #endregion
    }
}

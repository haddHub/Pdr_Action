using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class InteractionConfig
    {
        #region Fields

        private bool _vs, _jss;
        private byte _parmInterac;
        private byte _kInteracL, _cInteracL, _kInteracR, _cInteracR;

        #endregion

        #region Constructors

        public InteractionConfig()
        {
            this._vs = false;
            this._jss = false;
            this._parmInterac = 30;
            this._kInteracL = 0;
            this._cInteracL = 0;
            this._kInteracR = 0;
            this._cInteracR = 0;
        }

        public InteractionConfig(bool vs, bool js, byte pa, byte kil, byte cil, byte kir, byte cir)
        {
            this._vs = vs;
            this._jss = js;
            this._parmInterac = pa;
            this._kInteracL = kil;
            this._cInteracL = cil;
            this._kInteracR = kir;
            this._cInteracR = cir;
        }

        #endregion

        #region Properties

        public bool Vs
        {
            get
            {
                return _vs;
            }
            set
            {
                _vs = value;
            }
        }

        public bool Jss
        {
            get
            {
                return _jss;
            }
            set
            {
                _jss = value;
            }
        }

        public byte ParamInterac
        {
            get
            {
                return _parmInterac;
            }
            set
            {
                _parmInterac = value;
            }
        }

        public byte KInteracL
        {
            get
            {
                return _kInteracL;
            }
            set
            {
                _kInteracL = value;
            }
        }

        public byte CInteractL
        {
            get
            {
                return _cInteracL;
            }
            set
            {
                _cInteracL = value;
            }
        }

        public byte KInteractR
        {
            get
            {
                return _kInteracR;
            }
            set
            {
                _kInteracR = value;
            }
        }

        public byte CInteractR
        {
            get
            {
                return _cInteracR;
            }
            set
            {
                _cInteracR = value;
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

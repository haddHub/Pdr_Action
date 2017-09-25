using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class ChasseurModel : EntiteeModel
    {
        #region Fields

        private string _sound;
        private string _shapeHit;

        #endregion


        #region Constructors

        public ChasseurModel()
            : base()
        {
            this._sound = string.Empty;
        }

        public ChasseurModel(string shh, string sh, string na, string so)
            : base(sh, na)
        {
            this._sound = so;
            this._shapeHit = shh;
        }

        #endregion

        #region Properties

        public string Sound
        {
            get
            {
                return this._sound;
            }
            set
            {
                this._sound = value;
            }
        }

        public string ShapeHit
        {
            get
            {
                return _shapeHit;
            }
            set
            {
                _shapeHit = value;
            }
        }

        #endregion
    }
}

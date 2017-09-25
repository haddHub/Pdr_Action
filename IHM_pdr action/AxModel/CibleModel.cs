using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class CibleModel : EntiteeModel
    {
        #region Fields

        private string _shapeHit;

        #endregion

        #region Constructors

        public CibleModel()
            : base()
        {

        }

        public CibleModel(string shh, string sh, string na)
            : base(sh, na)
        {
            this._shapeHit = shh;
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

        #region Properties

        #endregion

    }
}

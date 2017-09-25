using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class BorneDataModel : aXdataModel
    {
        #region Fields
        /// <summary>
        /// Borne
        /// </summary>
        private char _xGauche, _xDroite, _bBorneCircH, _borneCircH;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the BorneDataModel class.
        /// </summary>
        /// <param name="xG"></param>
        /// <param name="xD"></param>
        /// <param name="yH"></param>
        /// <param name="yB"></param>
        public BorneDataModel(char xG, char xD, char bBC, char bC)
        {
            _xGauche = xG;
            _xDroite = xD;
            _bBorneCircH = bBC;
            _borneCircH = bC;
        }

        #endregion

        #region Properties

        public char XGauche
        {
            get
            {
                return _xGauche;
            }

            set
            {
                _xGauche = value;
            }
        }

        public char XDroite
        {
            get
            {
                return _xDroite;
            }

            set
            {
                _xDroite = value;
            }
        }

        public char BBorneCircH
        {
            get
            {
                return _bBorneCircH;
            }

            set
            {
                _bBorneCircH = value;
            }
        }

        public char BorneCircH
        {
            get
            {
                return _borneCircH;
            }

            set
            {
                _borneCircH = value;
            }
        }

        #endregion

        #region Methods

        #endregion
    }
}

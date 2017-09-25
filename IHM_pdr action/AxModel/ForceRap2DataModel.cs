using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class ForceRap2DataModel : aXdataModel
    {
        #region Fields
        /// <summary>
        /// Force Reçu UART
        /// </summary>
        private int _forceRapX, _forceRapY;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the ForceRap2DataModel class.
        /// </summary>
        /// <param name="coX"></param>
        /// <param name="coY"></param>
        public ForceRap2DataModel(int forX, int forY)
        {
            this._forceRapX = forX;
            this._forceRapY = forY;
        }

        public ForceRap2DataModel()
        {
            this._forceRapX = 0;
            this._forceRapY = 0;
        }

        #endregion

        #region Properties

        public int ForceRapX
        {
            get
            {
                return _forceRapX;
            }
            set
            {
                _forceRapX = value;
            }
        }

        public int ForceRapY
        {
            get
            {
                return _forceRapY;
            }
            set
            {
                _forceRapY = value;
            }
        }

        #endregion

        #region Methods
        // TODO : ajouter methode convertion en couple
        public double ForceToNewtonX()
        {
            return (double)this.ForceRapX / 1000.0;
        }

        public double ForceToNewtonY()
        {
            return (double)this.ForceRapY / 1000.0;
        }
        #endregion
    }
}

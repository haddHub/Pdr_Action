using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class AcosTDataModel : aXdataModel
    {
        #region Fields
        /// <summary>
        /// Force Reçu UART
        /// </summary>
        private int _aCosT, _null;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the AcosTDataModel class.
        /// </summary>
        /// <param name="coX"></param>
        /// <param name="coY"></param>
        public AcosTDataModel(int aCos, int nul)
        {
            this._aCosT = aCos;
            this._null = nul;
        }

        public AcosTDataModel()
        {
            this._aCosT = 0;
            this._null = 0;
        }

        #endregion

        #region Properties

        public int AcosT
        {
            get
            {
                return _aCosT;
            }
            set
            {
                _aCosT = value;
            }
        }

        public int Nul
        {
            get
            {
                return _null;
            }
            set
            {
                _null = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// retourne AcosT en radiant
        /// </summary>
        public double AcosTrad()
        {
            return (double)_aCosT / 1000.0;
        }

        /// <summary>
        /// retourne AcosT en degré
        /// </summary>
        /// <returns></returns>
        public double AcosDegre()
        {
            return (double)(_aCosT / 1000.0) * (180.0 / Math.PI);
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class ExerciceXDent : IExercice 
    {
        #region Fields

        private byte _angleCible;   // en ° !!!
        private byte _distCible;    // en cm !!!
        private byte _nbrsCibles;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the ExerciceXDent class.
        /// </summary>
        public ExerciceXDent()
        {
            this._angleCible = 25;
            this._distCible = 10;
            this._nbrsCibles = 3;
        }

        #endregion

        #region Properties

        public byte Anglecible
        {
            get
            {
                return _angleCible;
            }
            set
            {
                _angleCible = value;
            }
        }

        public byte DistCibles
        {
            get
            {
                return _distCible;
            }
            set
            {
                _distCible = value;
            }
        }

        public byte NbrsCibles
        {
            get
            {
                return _nbrsCibles;
            }
            set
            {
                _nbrsCibles = value;
            }
        }

        public ExerciceTypes ExerciceType
        {
            get
            {
                return ExerciceTypes.XDent;
            }
        }

        #endregion

        #region Methods

        //object ICloneable.Clone()
        //{
        //    ExerciceXDent exX = (ExerciceXDent)this.MemberwiseClone();
        //    exX.Anglecible = this.Anglecible;
        //    exX.DistCibles = this.DistCibles;
        //    exX.NbrsCibles = this.NbrsCibles;
        //    return exX;
        //}

        #endregion

        #region RelayCommand

        #endregion

        #region Actions

        #endregion
    }
}

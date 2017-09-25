using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class ExerciceBorneConfig
    {
        #region Fields

        private double _borneG;
        private double _borneD;
        private double _borneH;
        private double _borneB;
        private double _borneArc_H;
        private double _borneArc_B;
        private int _tailleBras;
        private string _tailleBorne;

       


        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the ExerciceBorneConfig class.
        /// </summary>
        public ExerciceBorneConfig()
        {
            //this._borneG = 1.0;
            //this._borneD = 73.0;
            //this._borneH = 24.0;
            //this._borneB = 70.0;
            //this._borneArc_H = 45.0;
            //this._borneArc_B = 0.0;
            this._tailleBras = 75;
        }

        public ExerciceBorneConfig(ExerciceBorneConfig ebc)
        {
            this._borneG = ebc.BorneG;
            this._borneD = ebc.BorneD;
            this._borneH = ebc.BorneH;
            this._borneB = ebc.BorneB;
            this._borneArc_H = ebc.BorneArc_H;
            this._borneArc_B = ebc.BorneArc_B;
            this._tailleBras = ebc.TailleBras;
        }

        public ExerciceBorneConfig(string taille,double bg,double bd,double bh,double bb,double bah,double bab)
        {
            this._tailleBorne = taille;
            this._borneG = bg;
            this._borneD = bd;
            this._borneH = bh;
            this._borneB = bb;
            this._borneArc_H = bah;
            this._borneArc_B = bab;
        }

        #endregion

        #region Properties

        public double BorneG
        {
            get
            {
                return _borneG;
            }
            set
            {
                //if (value < 1.0)
                //{
                //    _borneG = 1.0;
                //}
                //else if (value > 21.0)
                //{
                //    _borneG = 21.0;
                //}
                //else
                //{
                    _borneG = value;
                //}
            }
        }

        public double BorneD
        {
            get
            {
                return _borneD;
            }
            set
            {
                //if (value < 53.0)
                //{
                //    _borneD = 53.0;
                //}
                //else if (value > 73.0)
                //{
                //    _borneD = 73.0;
                //}
                //else
                //{
                    _borneD = value;
                //}
            }
        }

        public double BorneH
        {
            get
            {
                return _borneH;
            }
            set
            {
                _borneH = value;
            }
        }

        public double BorneB
        {
            get
            {
                return _borneB;
            }
            set
            {
                _borneB = value;
            }
        }

        public double BorneArc_H
        {
            get
            {
                return _borneArc_H;
            }
            set
            {
                //if (value < 18.0)
                //{
                //    _borneArc_H = 18.0;
                //}
                //else if (value > 45.0)
                //{
                //    _borneArc_H = 45.0;
                //}
                //else
                //{
                    _borneArc_H = value;
                //}
            }
        }

        public double BorneArc_B
        {
            get
            {
                return _borneArc_B;
            }
            set
            {
                _borneArc_B = value;
            }
        }
        public int TailleBras
        {
            get
            {
                return _tailleBras;
            }
            set
            {
                //if (value < 20)
                //{
                //    _borneArc_H = 20;
                //}
                //else if (value > 80)
                //{
                //    _borneArc_H = 80;
                //}
                _tailleBras = value;
            }
        }
        public string TailleBorne
        {
            get { return _tailleBorne; }
            set { _tailleBorne = value; }
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

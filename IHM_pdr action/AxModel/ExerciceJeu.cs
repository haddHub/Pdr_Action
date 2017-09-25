using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxModel.Attributes;
using AxLanguage;

namespace AxModel
{
    public enum Zoom : int
    {
        [LocalizableDescription(@"Petit", typeof(Languages))]
        Petit = 1,
        [LocalizableDescription(@"Moyen", typeof(Languages))]
        Moyen = 2,
        [LocalizableDescription(@"Grand", typeof(Languages))]
        Grand = 3
    }

    public class ExerciceJeu : ExerciceReeducation
    {
        #region Fields

        private bool _staticDyn;
        private bool _ref;
        private bool _trace;
        private Zoom _taille;
        // private ThemeModel _theme;
        private int _temps;
        private ulong _score;
        private byte _lvl;
        private byte[] _tabPosDebut;  // util si mode jeu !

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the ExerciceForme class.
        /// </summary>
        public ExerciceJeu(ExerciceBaseConfig baseConf, ExerciceBorneConfig borneConf, ThemeModel theme)
            : base(baseConf,borneConf,theme)
        {
            //valeur pour l'exo par défaut
            this._trace = false;
            this._temps = 10;
            this._score = 160U;
            this._taille = Zoom.Petit;
            this._lvl = 1;
            this._tabPosDebut = new byte[2];
            _tabPosDebut[0] = 47;   // 45
            _tabPosDebut[1] = 50;
        }

        #endregion

        #region Properties

        public bool StaticDyn
        {
            get
            {
                return _staticDyn;
            }
            set
            {
                _staticDyn = value;
            }
        }

        public bool Reference
        {
            get
            {
                return _ref;
            }
            set
            {
                _ref = value;
            }
        }

        public bool Trace
        {
            get
            {
                return _trace;
            }
            set
            {
                _trace = value;
            }
        }

        public Zoom Taille
        {
            get
            {
                return _taille;
            }
            set
            {
                _taille = value;
            }
        }

        public double TailleNbrs()
        {
            switch (Taille)
            {
                case Zoom.Petit: return 100.0;
                    break;
                case Zoom.Moyen: return 150.0;
                    break;
                case Zoom.Grand: return 200.0;
                    break;
                default: return 150.0;
                    break;
            }
        }

        //public ThemeModel Theme
        //{
        //    get
        //    {
        //        return _theme;
        //    }
        //    set
        //    {
        //        _theme = value;
        //    }
        //}

        public int Temps
        {
            get
            {
                return _temps;
            }
            set
            {
                _temps = value;
            }
        }

        public ulong Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
            }
        }

        public byte Niveau
        {
            get
            {
                return _lvl;
            }
            set
            {
                _lvl = value;
            }
        }

        public byte[] TabPosDebut
        {
            get
            {
                return _tabPosDebut;
            }
            set
            {
                _tabPosDebut = value;
            }
        }

        public ExerciceTypes ExerciceType
        {
            get
            {
                return ExerciceTypes.Jeu;
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

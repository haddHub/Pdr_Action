using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{

    public enum ModeType
    {
        Passif,
        Activo_passif,
        Actif
    }

    public class ExerciceBaseConfig
    {
        #region Fields

        private ModeType _mode;
        private byte _masse;        // masse_vir
        private byte _viscosite;    // visq_vir
        private short _raideurLat;   // k_lat
        private short _raideurLong;  // c_long
        private byte _vitesse;      // temps_desiree
        private byte _nbrRep;       // nbrs_parc
        private byte _init;         // initialisation
        private bool _auto;         // assist auto

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the ExerciceBaseConfig class.
        /// </summary>
        public ExerciceBaseConfig()
        {
            this._mode = ModeType.Activo_passif;
            this._masse = 10;
            this._viscosite = 50;
            this._raideurLat = 0;
            this._raideurLong = 0;
            this._vitesse = 0;
            this._nbrRep = 100;
            this._init = 25;
            this._auto = true;
            this._negLat = false;
            this._negLong = false;
            this._initiation = true;
            
        }

        /// <summary>
        /// Initializes a new instance of the ExerciceBaseConfig class.
        /// </summary>
        /// <param name="mas1"></param>
        /// <param name="mas2"></param>
        /// <param name="vis1"></param>
        /// <param name="vis2"></param>
        /// <param name="rLatX"></param>
        /// <param name="rLatY"></param>
        /// <param name="rLonX"></param>
        /// <param name="rLonY"></param>
        /// <param name="vit"></param>
        /// <param name="nbr"></param>
        /// <param name="pmAX"></param>
        /// <param name="pmAY"></param>
        /// <param name="pmBX"></param>
        /// <param name="pmBY"></param>
        /// <param name="ini"></param>
        /// <param name="aut"></param>
        public ExerciceBaseConfig(byte mas, byte vis, byte rLat, byte rLon, byte vit, byte nbr, byte ini, bool aut,bool nlat, bool nlong, bool init)
        {
            this._mode = ModeType.Activo_passif;
            this._masse = mas;
            this._viscosite = vis;
            this._raideurLat = rLat;
            this._raideurLong = rLon;
            this._vitesse = vit;
            this._nbrRep = nbr;
            this._init = ini;
            this._auto = aut;
            this._negLat = nlat;
            this._negLong = nlong;
            this._initiation = init;
        }

        public ExerciceBaseConfig(ExerciceBaseConfig ebc)
        {
            this._mode = ebc.ModeType;
            this._masse = ebc.Masse;
            this._viscosite = ebc.Viscosite;
            this._raideurLat = ebc.RaideurLat;
            this._raideurLong = ebc.RaideurLong;
            this._vitesse = ebc.Vitesse;
            this._nbrRep = ebc.NbrRep;
            this._init = ebc.Init;
            this._auto = ebc.Auto;
        }

        #endregion

        private const double TIME = 0.04;

        #region Properties

        public ModeType ModeType
        {
            get
            {
                return _mode;
            }
            set
            {
                _mode = value;
            }
        }

        public byte Masse
        {
            get
            {
                return this._masse;
            }
            set
            {
                this._masse = value;
            }
        }

        public byte Viscosite
        {
            get
            {
                return this._viscosite;
            }
            set
            {
                this._viscosite = value;
            }
        }

        public short RaideurLat
        {
            get
            {
                return this._raideurLat;
            }
            set
            {
                this._raideurLat = value;
            }
        }

        public short RaideurLong
        {
            get
            {
                return this._raideurLong;
            }
            set
            {
                this._raideurLong = value;
            }
        }

        public byte Vitesse
        {
            get
            {
                return this._vitesse;
            }
            set
            {
                this._vitesse = value;
            }
        }

        public byte NbrRep
        {
            get
            {
                return this._nbrRep;
            }
            set
            {
                this._nbrRep = value;
            }
        }

        public byte Init
        {
            get
            {
                return this._init;
            }
            set
            {
                this._init = value;
            }
        }

        public bool Auto
        {
            get
            {
                return this._auto;
            }
            set
            {
                this._auto = value;
            }
        }

        private bool _initiation;

        public bool Initiation
        {
            get { return _initiation; }
            set { _initiation = value; }
        }

        private bool _negLat;

        public bool NegLat
        {
            get { return _negLat; }
            set 
            {
                _negLat = value;
                if (value == false)
                {
                    if (RaideurLat < 1)
                    {
                        RaideurLat = 0;
                    }
                }
            }
        }

        private bool _negLong;

        public bool NegLong
        {
            get { return _negLong; }
            set 
            {
                _negLong = value;
                if (value == true)
                {
                    Vitesse = 5;
                }
            }
        }

        private double _initGUI;

        public double InitGUI
        {
            get 
            { 
                return this.Init * TIME; 
            }
            set 
            {
                this.Init = (byte)(value / TIME);
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

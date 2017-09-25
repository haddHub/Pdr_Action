using GalaSoft.MvvmLight;
using System;
using AxModel;
using System.Collections.Generic;
using AxError;
using GalaSoft.MvvmLight.Messaging;
namespace AxViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm/getstarted
    /// </para>
    /// </summary>
    public class ExerciceBaseConfigViewModel : ViewModelBase
    {
        #region Fields

        private SingletonReeducation ValeurReeducation = SingletonReeducation.getInstance();

        private readonly ExerciceBaseConfig exerciceBaseConfig;

        private List<double> ListeInit = new List<double>();
        private List<double> ListeKlat = new List<double>();
        private List<double> ListeKlon = new List<double>();

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the ExerciceBaseConfigViewModel class.
        /// </summary>
        public ExerciceBaseConfigViewModel(ExerciceBaseConfig exerciceBaseConfig)
        {
            try
            {
                this.exerciceBaseConfig = exerciceBaseConfig;
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        #endregion

        #region Properties

        public ModeType ModeType
        {
            get
            {
                return exerciceBaseConfig.ModeType;
            }
            set
            {
                exerciceBaseConfig.ModeType = value;
                RaisePropertyChanged("ModeType");
            }
        }

        public byte Masse
        {
            get
            {
                return exerciceBaseConfig.Masse;
            }
            set
            {
                exerciceBaseConfig.Masse = value;
                RaisePropertyChanged("Masse");
            }
        }

        public byte Viscosite
        {
            get
            {
                return exerciceBaseConfig.Viscosite;
            }
            set
            {
                exerciceBaseConfig.Viscosite = value;
                RaisePropertyChanged("Viscosite");
            }
        }

        public byte RaideurLat
        {
            get
            {
                return (byte)exerciceBaseConfig.RaideurLat;
            }
            set
            {
                exerciceBaseConfig.RaideurLat = value;
                RaisePropertyChanged("RaideurLat");
                ListeKlat.Add((double)exerciceBaseConfig.RaideurLat);
                ValeurReeducation.Klat = ListeKlat;
            }
        }

        public short RaideurLong
        {
            get
            {
                return exerciceBaseConfig.RaideurLong;
            }
            set
            {
                exerciceBaseConfig.RaideurLong = value;
                RaisePropertyChanged("RaideurLong");
                ListeKlon.Add((double)exerciceBaseConfig.RaideurLong);
                ValeurReeducation.Klong = ListeKlon;
            }
        }

        public byte Vitesse
        {
            get
            {
                return exerciceBaseConfig.Vitesse;
            }
            set
            {
                exerciceBaseConfig.Vitesse = value;
                RaisePropertyChanged("Vitesse");
            }
        }

        public byte NbrRep
        {
            get
            {
                return exerciceBaseConfig.NbrRep;
            }
            set
            {
                exerciceBaseConfig.NbrRep = value;
                RaisePropertyChanged("NbrRep");
            }
        }

        public byte Init
        {
            get
            {
                return exerciceBaseConfig.Init;
            }
            set
            {
                exerciceBaseConfig.Init = value;
                RaisePropertyChanged("Init");
            }
        }

        public bool Auto
        {
            get
            {
                return exerciceBaseConfig.Auto;
            }
            set
            {
                exerciceBaseConfig.Auto = value;
                RaisePropertyChanged("Auto");
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
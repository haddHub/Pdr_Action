using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using AxModel.Message;
using AxModel;
using AxError;
using GalaSoft.MvvmLight.Command;
using Navegar;
using GalaSoft.MvvmLight.Ioc;
namespace AxViewModel
{
    public class EvoEvalViewModel : ViewModelBase, IPageViewModel
    {
        #region Fields


        private const string ExMvtsComplexesPropertyName = "ExMvtsComplexes";
        private bool _ExMvtsComplexes = false;
        private const string ExMvtsRythmiquesPropertyName = "ExMvtsRythmiques";
        private bool _ExMvtsRythmiques = false;
        private const string ExMvtsSimplesPropertyName = "ExMvtsSimples";
        private bool _ExMvtsSimples = false;
        private const string ExMvtsCognitifsPropertyName = "ExMvtsCognitifs";
        private bool _ExMvtsCognitifs = false;

        private const string VisiChMvtsRythmiquesPropertyName = "VisiChMvtsRythmiques";
        private Visibility _VisiChMvtsRythmiques = Visibility.Visible;
        private const string VisiChMvtsSimplesPropertyName = "VisiChMvtsSimples";
        private Visibility _VisiChMvtsSimples = Visibility.Visible;
        private const string VisiChMvtsCognitifsPropertyName = "VisiChMvtsCognitifs";
        private Visibility _VisiChMvtsCognitifs = Visibility.Visible;

        private const string LabelPatientPropertyName = "LabelPatient";
        private string _labelPatient = null;

        private const string PatientValuePropertyName = "PatientValue";
        private ListePatientDataGrid _patientValue = null;

        #endregion

        #region Constructors
        public EvoEvalViewModel()
        {
            Messenger.Default.Register<PatientMessage>(this, OnRegister);
            MainViewModelCommand = new RelayCommand(NavigateToHome);

        }

        #endregion

        #region Properties
        public ListePatientDataGrid PatientValue
        {
            get { return _patientValue; }
            set
            {
                if (_patientValue == value)
                {
                    _patientValue = value;
                }

                RaisePropertyChanging(PatientValuePropertyName);
                _patientValue = value;
                RaisePropertyChanged(PatientValuePropertyName);

            }
        }


        public string LabelPatient
        {
            get { return _labelPatient; }
            set
            {
                if (_labelPatient == value)
                {
                    _labelPatient = value;
                }

                RaisePropertyChanging(LabelPatientPropertyName);
                _labelPatient = value;
                RaisePropertyChanged(LabelPatientPropertyName);
            }
        }


        public bool ExMvtsComplexes
        {
            get { return _ExMvtsComplexes; }
            set
            {
                if (_ExMvtsComplexes == value)
                {
                    _ExMvtsComplexes = value;
                }

                RaisePropertyChanging(ExMvtsComplexesPropertyName);
                _ExMvtsComplexes = value;
                RaisePropertyChanged(ExMvtsComplexesPropertyName);
                if (ExMvtsComplexes == true)
                {
                    ExMvtsRythmiques = false;
                    ExMvtsSimples = false;
                    ExMvtsCognitifs = false;
                    VisiChMvtsRythmiques = Visibility.Hidden;
                    VisiChMvtsSimples = Visibility.Hidden;
                    VisiChMvtsCognitifs = Visibility.Hidden;
                }
                else
                {
                    VisiChMvtsRythmiques = Visibility.Visible;
                    VisiChMvtsSimples = Visibility.Visible;
                    VisiChMvtsCognitifs = Visibility.Visible;
                }
            }
        }

        public bool ExMvtsRythmiques
        {
            get { return _ExMvtsRythmiques; }
            set
            {
                if (_ExMvtsRythmiques == value)
                {
                    _ExMvtsRythmiques = value;
                }

                RaisePropertyChanging(ExMvtsRythmiquesPropertyName);
                _ExMvtsRythmiques = value;
                RaisePropertyChanged(ExMvtsRythmiquesPropertyName);
                if (ExMvtsRythmiques == true)
                {
                    ExMvtsComplexes = false;
                    ExMvtsSimples = false;
                    ExMvtsCognitifs = false;
                    VisiChMvtsSimples = Visibility.Hidden;
                    VisiChMvtsCognitifs = Visibility.Hidden;
                }
                else
                {
                    VisiChMvtsSimples = Visibility.Visible;
                    VisiChMvtsCognitifs = Visibility.Visible;
                }
            }
        }

        public bool ExMvtsSimples
        {
            get { return _ExMvtsSimples; }
            set
            {
                if (_ExMvtsSimples == value)
                {
                    _ExMvtsSimples = value;
                }

                RaisePropertyChanging(ExMvtsSimplesPropertyName);
                _ExMvtsSimples = value;
                RaisePropertyChanged(ExMvtsSimplesPropertyName);
                if (ExMvtsSimples == true)
                {
                    ExMvtsComplexes = false;
                    ExMvtsRythmiques = false;
                    ExMvtsCognitifs = false;
                    VisiChMvtsCognitifs = Visibility.Hidden;
                }
                else
                {
                    VisiChMvtsCognitifs = Visibility.Visible;
                }
            }
        }

        public bool ExMvtsCognitifs
        {
            get { return _ExMvtsCognitifs; }
            set
            {
                if (_ExMvtsCognitifs == value)
                {
                    _ExMvtsCognitifs = value;
                }

                RaisePropertyChanging(ExMvtsCognitifsPropertyName);
                _ExMvtsCognitifs = value;
                RaisePropertyChanged(ExMvtsCognitifsPropertyName);
                if (ExMvtsCognitifs == true)
                {
                    ExMvtsComplexes = false;
                    ExMvtsRythmiques = false;
                    ExMvtsSimples = false;
                }
            }
        }

        public Visibility VisiChMvtsRythmiques
        {
            get { return _VisiChMvtsRythmiques; }
            set
            {
                if (_VisiChMvtsRythmiques == value)
                {
                    _VisiChMvtsRythmiques = value;
                }

                RaisePropertyChanging(VisiChMvtsRythmiquesPropertyName);
                _VisiChMvtsRythmiques = value;
                RaisePropertyChanged(VisiChMvtsRythmiquesPropertyName);
            }
        }

        public Visibility VisiChMvtsSimples
        {
            get { return _VisiChMvtsSimples; }
            set
            {
                if (_VisiChMvtsSimples == value)
                {
                    _VisiChMvtsSimples = value;
                }

                RaisePropertyChanging(VisiChMvtsSimplesPropertyName);
                _VisiChMvtsSimples = value;
                RaisePropertyChanged(VisiChMvtsSimplesPropertyName);
            }
        }

        public Visibility VisiChMvtsCognitifs
        {
            get { return _VisiChMvtsCognitifs; }
            set
            {
                if (_VisiChMvtsCognitifs == value)
                {
                    _VisiChMvtsCognitifs = value;
                }

                RaisePropertyChanging(VisiChMvtsCognitifsPropertyName);
                _VisiChMvtsCognitifs = value;
                RaisePropertyChanged(VisiChMvtsCognitifsPropertyName);
            }
        }


        #endregion

        #region Methods
        public string Name
        {
            get { return "EvolutionEvaluation"; }
        }
        private void OnRegister(PatientMessage obj)
        {
            PatientValue = obj.DataPatient;
            LabelPatient = PatientValue.Nom + " " + PatientValue.Prenom;
        }

        public void NavigateToHome()
        {
            if (MessageBox.Show(AxLanguage.Languages.REAplan_Accueil_Confirmation,AxLanguage.Languages.REAplan_Confirmation, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                SimpleIoc.Default.GetInstance<INavigation>().NavigateTo<HomeViewModel>(false); 
            }
        }

        #endregion

        #region RelayCommand

        public RelayCommand MainViewModelCommand { get; set; }

        #endregion

        #region Actions

        #endregion
    }
}

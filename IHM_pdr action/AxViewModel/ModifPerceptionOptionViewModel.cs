using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using AxModel;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Navegar;
using GalaSoft.MvvmLight.Ioc;
using System.Xml.Linq;
using System.IO;

namespace AxViewModel
{
   public class ModifPerceptionOptionViewModel : ViewModelBase 
   {
        #region Field
        private INavigation nav;
        private Singleton singleton = Singleton.getInstance();
        private Visibility isOptionsVisible;
        private Visibility isTempsVisible;
        private string pathPatient = string.Empty;
        #endregion

        #region Command
        public RelayCommand HomeViewModelCommand { get; set; }
        public RelayCommand NextViewModelCommand { get; set; }
        public RelayCommand PreviousViewModelCommand { get; set; }
        public RelayCommand<int> ChangeExerciceTypeCommand { get; set; }
        public RelayCommand<int> ChangerOptionsCommand { get; set; }
        public RelayCommand<int> ChangerZoomCommand { get; set; }
        #endregion

        public ModifPerceptionOptionViewModel()
        {
            // Service
            nav = SimpleIoc.Default.GetInstance<INavigation>(); 

            // Commands
            this.HomeViewModelCommand = new RelayCommand(this.GoHome);
            this.NextViewModelCommand = new RelayCommand(this.GoNext);
            this.PreviousViewModelCommand = new RelayCommand(this.GoBack);
            this.ChangeExerciceTypeCommand = new RelayCommand<int>(this.ChangeExerciceType);
            this.ChangerOptionsCommand = new RelayCommand<int>(this.ChangerOptions);
            this.ChangerZoomCommand = new RelayCommand<int>(this.ChangerZoom);

            // De base c'est l'exercice sans option qui est coché
            this.IsOptionsVisible = Visibility.Hidden;
            this.IsTempsVisible = Visibility.Hidden;
            // Init en fonction du précochage
            this.ChangeExerciceType(0);
            this.ChangerOptions(0);
            this.ChangerZoom(0);
            this.InitTime = 7;
            this.GeneralTime = 20;
            this.OutsideTime = 7;

            this.pathPatient = "Files/Patients/" + singleton.Patient.Nom + singleton.Patient.Prenom + singleton.Patient.DateDeNaissance.ToString().Replace("/", string.Empty);
        }

        #region Property
        public Visibility IsOptionsVisible
        {
            get { return isOptionsVisible; }
            set 
            {
                isOptionsVisible = value;
                RaisePropertyChanged("IsOptionsVisible");
            }
        }

        public Visibility IsTempsVisible
        {
            get { return isTempsVisible; }
            set
            {
                isTempsVisible = value;
                RaisePropertyChanged("IsTempsVisible");
            }
        }

        private int initTime;

        public int InitTime
        {
            get { return initTime; }
            set 
            {
                initTime = value;
                RaisePropertyChanged("InitTime");
            }
        }

        private int generalTime;

        public int GeneralTime
        {
            get { return generalTime; }
            set
            {
                generalTime = value;
                RaisePropertyChanged("GeneralTime");
            }
        }

        private int outsideTime;

        public int OutsideTime
        {
            get { return outsideTime; }
            set
            {
                outsideTime = value;
                RaisePropertyChanged("OutsideTime");
            }
        }

        #endregion

        #region Methodes
        private void ChangeExerciceType(int newType)
        {
            switch (newType)
            {
                case 0:
                    Singleton.TypeAction = PdrActionType.EgaliteLaterale;
                    this.IsOptionsVisible = Visibility.Hidden;
                    this.IsTempsVisible = Visibility.Hidden;
                    this.ChangerOptions(2);
                    break;
                case 1:
                    Singleton.TypeAction = PdrActionType.EgaliteLateraleForce;
                    this.IsOptionsVisible = Visibility.Hidden;
                    this.IsTempsVisible = Visibility.Hidden;
                    this.ChangerOptions(2);
                    break;
                case 2:
                    Singleton.TypeAction = PdrActionType.BiaisLateral;
                    this.IsOptionsVisible = Visibility.Visible;
                    this.IsTempsVisible = Visibility.Hidden;
                    break;
                case 3:
                    Singleton.TypeAction = PdrActionType.BiaisLateralForce;
                    this.IsOptionsVisible = Visibility.Visible;
                    this.IsTempsVisible = Visibility.Hidden;
                    break;
                case 4:
                    Singleton.TypeAction = PdrActionType.FacilitationLaterale;
                    this.IsOptionsVisible = Visibility.Visible;
                    this.IsTempsVisible = Visibility.Hidden;
                    break;
                case 5:
                    Singleton.TypeAction = PdrActionType.EgaliteLateraleTemps;
                    this.IsOptionsVisible = Visibility.Hidden;
                    this.IsTempsVisible = Visibility.Visible;
                    break;
                case 6:
                    Singleton.TypeAction = PdrActionType.BiaisLateralTemps;
                    this.IsOptionsVisible = Visibility.Visible;
                    this.IsTempsVisible = Visibility.Visible;
                    break;
                default:
                    Singleton.TypeAction = PdrActionType.EgaliteLaterale;
                    this.IsOptionsVisible = Visibility.Hidden;
                    this.IsTempsVisible = Visibility.Hidden;
                    this.ChangerOptions(2);
                    break;
            }
        }

        private void ChangerOptions(int newOption)
        {
            switch (newOption)
            {
                case 0:
                    Singleton.Options = PdrOptions.Option1;
                    break;
                case 1:
                    Singleton.Options = PdrOptions.Option2;
                    break;
                case 2:
                    Singleton.Options = PdrOptions.PasOptions;
                    break;
                default:
                    Singleton.Options = PdrOptions.PasOptions;
                    break;
            }
        }

        private void ChangerZoom(int newZoom)
        {
            switch (newZoom)
            {
                case 0:
                    Singleton.Zoom = PdrZoom.Petit;
                    break;
                case 1:
                    Singleton.Zoom = PdrZoom.Grand;
                    break;
                default:
                    Singleton.Zoom = PdrZoom.Petit;
                    break;
            }
        }

        /// <summary>
        /// Reset apres le goBack du zoom
        /// </summary>
        private void ResetFromBack()
        {
            this.ChangeExerciceType(0);
            this.ChangerOptions(0);
            this.ChangerZoom(0);
        }
        #endregion

        #region CanExecute
        #endregion

        #region Navigation
        /// <summary>
        /// Retour à l'accueil
        /// </summary>
        private void GoHome()
        {
            nav.NavigateTo<HomeViewModel>("SetIsRetour", new object[] { true });
        }

        private void GoNext()
        {
            // Sauvegarde des options
            XDocument doc;

            if (File.Exists(this.pathPatient + "/InfoPatient.xml"))
            {
                doc = XDocument.Load(this.pathPatient + "/InfoPatient.xml");
                var date = DateTime.Now.ToShortDateString().Replace('/','-');
                var heure = DateTime.Now.ToShortTimeString().Replace(':','h');

                var option = (Singleton.TypeAction == PdrActionType.EgaliteLaterale || Singleton.TypeAction == PdrActionType.EgaliteLateraleForce || Singleton.TypeAction == PdrActionType.EgaliteLateraleTemps) ? PdrOptions.PasOptions.ToString() : Singleton.Options.ToString();

                doc.Root.Add(
                    new XElement("Settings_" + date + "_" + heure,
                        new XElement("Exercice", Singleton.TypeAction.ToString()),
                        new XElement("Option", option),
                        new XElement("Zoom", Singleton.Zoom.ToString())));

                doc.Save(this.pathPatient + "/InfoPatient.xml");
            }

            Singleton.InitTime = this.InitTime;
            Singleton.GeneralTime = this.GeneralTime;
            Singleton.OutsideTime = this.OutsideTime;

            this.nav.NavigateTo<ModifActionZoomViewModel>(this, null, true);
        }

        private void GoBack()
        {
            nav.GoBack();
        }
        #endregion
    }
}

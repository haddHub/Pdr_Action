using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using AxModel;
using Navegar;
using GalaSoft.MvvmLight.Ioc;
using System.Windows;
using System.Xml.Linq;
using System.IO;

namespace AxViewModel
{
    public class ModifActionOptionViewModel : ViewModelBase
    {
        #region Field
        private INavigation nav;
        private Singleton singleton = Singleton.getInstance();
        private Visibility isOptionsVisible;
        private int viscositeValue;
        private string pathPatient = string.Empty;
        private const byte VISCMIN = 15;
        #endregion

        #region Command
        public RelayCommand HomeViewModelCommand { get; set; }
        public RelayCommand NextViewModelCommand { get; set; }
        public RelayCommand PreviousViewModelCommand { get; set; }
        public RelayCommand<int> ChangeExerciceTypeCommand { get; set; }
        public RelayCommand<int> ChangerOptionsCommand { get; set; }
        public RelayCommand<int> ChangerZoomCommand { get; set; }
        #endregion

        public ModifActionOptionViewModel()
        {
            // Service
            nav = SimpleIoc.Default.GetInstance<INavigation>(); 

            // Commands'
            this.HomeViewModelCommand = new RelayCommand(this.GoHome);
            this.NextViewModelCommand = new RelayCommand(this.GoNext);
            this.PreviousViewModelCommand = new RelayCommand(this.GoBack);
            this.ChangeExerciceTypeCommand = new RelayCommand<int>(this.ChangeExerciceType);
            this.ChangerOptionsCommand = new RelayCommand<int>(this.ChangerOptions);
            this.ChangerZoomCommand = new RelayCommand<int>(this.ChangerZoom);

            // Init en fonction du précochage
            this.ChangeExerciceType(0);
            this.ChangerOptions(0);
            this.ChangerZoom(0);
            this.ViscFromFile();
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

        public int ViscositeValue
        {
            get { return viscositeValue; }
            set 
            {
                viscositeValue = value;
                RaisePropertyChanged("ViscositeValue");
                Singleton.Viscosite = viscositeValue;
            }
        }
        #endregion

        #region Methodes
        private void ChangeExerciceType(int newType)
        {
            switch (newType)
            {
                case 0:
                    Singleton.TypeAction = PdrActionType.InhibitionHemicorps;
                    this.IsOptionsVisible = Visibility.Visible;
                    break;
                case 1:
                    Singleton.TypeAction = PdrActionType.InhibitionHemichamp;
                    this.IsOptionsVisible = Visibility.Visible;
                    break;
                case 2:
                    Singleton.TypeAction = PdrActionType.FacilitationAction;
                    this.IsOptionsVisible = Visibility.Hidden;
                    this.ChangerOptions(2);
                    break;
                case 3:
                    Singleton.TypeAction = PdrActionType.PasDeModificationAction;
                    this.IsOptionsVisible = Visibility.Hidden;
                    this.ChangerOptions(2);
                    break;
                default:
                    Singleton.TypeAction = PdrActionType.PasDeModificationAction;
                    this.IsOptionsVisible = Visibility.Hidden;
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

        private void ViscFromFile()
        {
            this.pathPatient = "Files/Patients/" + singleton.Patient.Nom + singleton.Patient.Prenom + singleton.Patient.DateDeNaissance.ToString().Replace("/", string.Empty);
            XDocument doc;

            if (File.Exists(this.pathPatient + "/InfoPatient.xml"))
            {
                doc = XDocument.Load(this.pathPatient + "/InfoPatient.xml");

                var force75string = doc.Root.Element("Force75").Value;
                force75string = force75string.Replace('.', ',');
                var force75 = Convert.ToDouble(force75string);

                if ((int)force75 < VISCMIN)
                {
                    this.ViscositeValue = VISCMIN;
                }
                else
                {
                    this.ViscositeValue = (int)force75;
                }                
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
            this.ViscFromFile();
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
                var date = DateTime.Now.ToShortDateString().Replace('/', '-');
                var heure = DateTime.Now.ToShortTimeString().Replace(':', 'h');

                var visco = (Singleton.TypeAction == PdrActionType.FacilitationAction || Singleton.TypeAction == PdrActionType.PasDeModificationAction) ? "Pas d'inhibition" : Singleton.Viscosite.ToString();

                doc.Root.Add(
                    new XElement("Settings_" + date + "_" + heure,
                        new XElement("Exercice", Singleton.TypeAction.ToString()),
                        new XElement("Option", Singleton.Options.ToString()),
                        new XElement("Zoom", Singleton.Zoom.ToString()),
                        new XElement("Viscosite", visco)));

                doc.Save(this.pathPatient + "/InfoPatient.xml");
            }

            this.nav.NavigateTo<ModifActionZoomViewModel>(this, null, true);
        }

        private void GoBack()
        {
            nav.GoBack();
        }
        #endregion 
    }
}

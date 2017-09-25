using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using Navegar;
using AxModel;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.IO;
using System.Xml.Linq;

namespace AxViewModel
{
    public class ModifActionZoomViewModel : ViewModelBase
    {
        #region Field
        private INavigation nav;
        private IMessageBoxService msg;
        private Singleton singleton = Singleton.getInstance();
        private PointCollection ciblesPossible;
        private int repetitionGauche;
        private int repetitionDroit;
        #endregion

        #region Command
        public RelayCommand HomeViewModelCommand { get; set; }
        public RelayCommand NextViewModelCommand { get; set; }
        public RelayCommand PreviousViewModelCommand { get; set; }
        public RelayCommand<int> ChangerChoixCommand { get; set; }
        #endregion

        public ModifActionZoomViewModel()
        {
            // Service
            nav = SimpleIoc.Default.GetInstance<INavigation>();
            msg = SimpleIoc.Default.GetInstance<IMessageBoxService>();

            // Commands
            this.HomeViewModelCommand = new RelayCommand(this.GoHome);
            this.NextViewModelCommand = new RelayCommand(this.GoNext);
            this.PreviousViewModelCommand = new RelayCommand(this.GoBack);
            this.ChangerChoixCommand = new RelayCommand<int>(this.ChangerChoix);

            this.InitPossible();
            this.CreationCibleAmplitudeArticulaireAide();
            this.InitSlider();
        }

        #region Property
        public ObservableCollection<CibleCheckBox> CiblesGauche { get; set; }
        public ObservableCollection<CibleCheckBox> CiblesDroite { get; set; }

        public int RepetitionGauche
        {
            get { return repetitionGauche; }
            set 
            {
                repetitionGauche = value;
                RaisePropertyChanged("RepetitionGauche");
            }
        }

        public int RepetitionDroit
        {
            get { return repetitionDroit; }
            set 
            {
                repetitionDroit = value;
                RaisePropertyChanged("RepetitionDroit");
            }
        }
        #endregion

        #region Methodes
        /// <summary>
        /// Création de la liste des cibles possible
        /// </summary>
        private void InitPossible()
        {
            this.ciblesPossible = new PointCollection();
            ciblesPossible.Add(new System.Windows.Point(192.0, 1008.0));    // 1
            ciblesPossible.Add(new System.Windows.Point(192.0, 864.0));     // 2
            ciblesPossible.Add(new System.Windows.Point(192.0, 720.0));     // 3
            ciblesPossible.Add(new System.Windows.Point(192.0, 576.0));     // 4
            ciblesPossible.Add(new System.Windows.Point(192.0, 432.0));     // 5
            ciblesPossible.Add(new System.Windows.Point(192.0, 288.0));     // 6
            ciblesPossible.Add(new System.Windows.Point(576.0, 1008.0));    // 7
            ciblesPossible.Add(new System.Windows.Point(576.0, 864.0));     // 8
            ciblesPossible.Add(new System.Windows.Point(576.0, 720.0));     // 9
            ciblesPossible.Add(new System.Windows.Point(576.0, 576.0));     // 10
            ciblesPossible.Add(new System.Windows.Point(576.0, 432.0));     // 11
            ciblesPossible.Add(new System.Windows.Point(576.0, 288.0));     // 12
            ciblesPossible.Add(new System.Windows.Point(1344.0, 1008.0));   // 19
            ciblesPossible.Add(new System.Windows.Point(1344.0, 864.0));    // 20
            ciblesPossible.Add(new System.Windows.Point(1344.0, 720.0));    // 21
            ciblesPossible.Add(new System.Windows.Point(1344.0, 576.0));    // 22
            ciblesPossible.Add(new System.Windows.Point(1344.0, 432.0));    // 23
            ciblesPossible.Add(new System.Windows.Point(1344.0, 288.0));    // 24
            ciblesPossible.Add(new System.Windows.Point(1728.0, 1008.0));   // 25
            ciblesPossible.Add(new System.Windows.Point(1728.0, 864.0));    // 26
            ciblesPossible.Add(new System.Windows.Point(1728.0, 720.0));    // 27
            ciblesPossible.Add(new System.Windows.Point(1728.0, 576.0));    // 28
            ciblesPossible.Add(new System.Windows.Point(1728.0, 432.0));    // 29
            ciblesPossible.Add(new System.Windows.Point(1728.0, 288.0));    // 30
        }

        private void CreationCiblesDefaut()
        {
            this.CiblesGauche = new ObservableCollection<CibleCheckBox>();
            this.CiblesDroite = new ObservableCollection<CibleCheckBox>();

            int numCible = 1;
            foreach (var cible in ciblesPossible)
            {
                // Mise à l'echelle des coordonnées des cible pour l'affichage dans l'interface thérapeute où l'image n'est pas en 1920x1080
                var xAff = (cible.X / 1920) * 1228;
                var yAff = ((1080 - cible.Y) / 1080) * 400;

                bool ischecked = true;
                if (Singleton.Zoom == PdrZoom.Petit)
                {
                    if (numCible == 1 || numCible == 2 || numCible == 3 || numCible == 7 || numCible == 8 || numCible == 9 || numCible == 19 || numCible == 20 || numCible == 21 || numCible ==25 || numCible == 26 || numCible == 27)
                    {
                        ischecked = false;
                    }
                }

                var check = new CibleCheckBox((int)xAff, (int)yAff, ischecked, numCible);
                if (numCible < 13)
                {
                    this.CiblesGauche.Add(check);
                }
                else
                {
                    this.CiblesDroite.Add(check);
                }
                

                numCible++;
                if (numCible == 13)
                {
                    numCible = 19;
                }
            }

            RaisePropertyChanged("CiblesGauche");
            RaisePropertyChanged("CiblesDroite");
        }

        private void CreationCibleAmplitudeArticulaire()
        {
            string pathPatient = "Files/Patients/" + singleton.Patient.Nom + singleton.Patient.Prenom + singleton.Patient.DateDeNaissance.ToString().Replace("/", string.Empty);
            if (File.Exists(pathPatient + "/InfoPatient.xml"))
            {
                try
                {
                    this.CiblesGauche = new ObservableCollection<CibleCheckBox>();
                    this.CiblesDroite = new ObservableCollection<CibleCheckBox>();

                    // Lecture du fichier
                    XDocument doc = XDocument.Load(pathPatient + "/InfoPatient.xml");
                    var cibles = doc.Root.Element("CibleAmpliArt").Value;
                    string[] ciblesString = cibles.Split(new char[] { ',' });


                    int numCible = 1;
                    foreach (var cible in ciblesPossible)
                    {
                        // Mise à l'echelle des coordonnées des cible pour l'affichage dans l'interface thérapeute où l'image n'est pas en 1920x1080
                        var xAff = (cible.X / 1920) * 1228;
                        var yAff = ((1080 - cible.Y) / 1080) * 400;

                        bool ischecked = false;

                        var check = new CibleCheckBox((int)xAff, (int)yAff, ischecked, numCible);
                        if (numCible < 13)
                        {
                            this.CiblesGauche.Add(check);
                        }
                        else
                        {
                            this.CiblesDroite.Add(check);
                        }


                        numCible++;
                        if (numCible == 13)
                        {
                            numCible = 19;
                        }
                    }

                    foreach (var cible in ciblesString)
                    {
                        int num = Convert.ToInt32(cible);
                        if (num <= 12)
                        {
                            // gauche
                            var lacbile = (from c in this.CiblesGauche
                                           where c.Number == num
                                           select c).First();
                            lacbile.Selected = true;
                        }
                        else
                        {
                            num += 6;
                            var lacbile = (from c in this.CiblesDroite
                                           where c.Number == num
                                           select c).First();
                            lacbile.Selected = true;
                        }
                    }
                }
                catch (Exception)
                {
                    this.CreationCiblesDefaut();
                }
            }
            else
            {
                this.CreationCiblesDefaut();
            }

            RaisePropertyChanged("CiblesGauche");
            RaisePropertyChanged("CiblesDroite");
        }

        private void CreationCibleAmplitudeArticulaireAide()
        {
            string pathPatient = "Files/Patients/" + singleton.Patient.Nom + singleton.Patient.Prenom + singleton.Patient.DateDeNaissance.ToString().Replace("/", string.Empty);
            if (File.Exists(pathPatient + "/InfoPatient.xml"))
            {
                try
                {
                    this.CiblesGauche = new ObservableCollection<CibleCheckBox>();
                    this.CiblesDroite = new ObservableCollection<CibleCheckBox>();

                    // Lecture du fichier
                    XDocument doc = XDocument.Load(pathPatient + "/InfoPatient.xml");
                    var cibles = doc.Root.Element("CibleAmpliArtAide").Value;
                    string[] ciblesString = cibles.Split(new char[] { ',' });


                    int numCible = 1;
                    foreach (var cible in ciblesPossible)
                    {
                        // Mise à l'echelle des coordonnées des cible pour l'affichage dans l'interface thérapeute où l'image n'est pas en 1920x1080
                        var xAff = (cible.X / 1920) * 1228;
                        var yAff = ((1080 - cible.Y) / 1080) * 400;

                        bool ischecked = false;

                        var check = new CibleCheckBox((int)xAff, (int)yAff, ischecked, numCible);
                        if (numCible < 13)
                        {
                            this.CiblesGauche.Add(check);
                        }
                        else
                        {
                            this.CiblesDroite.Add(check);
                        }


                        numCible++;
                        if (numCible == 13)
                        {
                            numCible = 19;
                        }
                    }

                    foreach (var cible in ciblesString)
                    {
                        int num = Convert.ToInt32(cible);
                        if (num <= 12)
                        {
                            // gauche
                            var lacbile = (from c in this.CiblesGauche
                                           where c.Number == num
                                           select c).First();
                            lacbile.Selected = true;
                        }
                        else
                        {
                            num += 6;
                            var lacbile = (from c in this.CiblesDroite
                                           where c.Number == num
                                           select c).First();
                            lacbile.Selected = true;
                        }
                    }
                }
                catch (Exception)
                {
                    this.CreationCiblesDefaut();
                }
            }
            else
            {
                this.CreationCiblesDefaut();
            }

            RaisePropertyChanged("CiblesGauche");
            RaisePropertyChanged("CiblesDroite");
        }

        private void InitSlider()
        {
            switch (Singleton.TypeAction)
            {
                case PdrActionType.InhibitionHemicorps:
                    if (Singleton.Zoom == PdrZoom.Petit)
                    {
                        this.RepetitionGauche = 4;
                        this.RepetitionDroit = 4;
                    }
                    else
                    {
                        this.RepetitionGauche = 2;
                        this.RepetitionDroit = 2;
                    }
                    break;
                case PdrActionType.InhibitionHemichamp:
                    if (Singleton.Zoom == PdrZoom.Petit)
                    {
                        this.RepetitionGauche = 4;
                        this.RepetitionDroit = 4;
                    }
                    else
                    {
                        this.RepetitionGauche = 2;
                        this.RepetitionDroit = 2;
                    }
                    break;
                case PdrActionType.FacilitationAction:
                    if (Singleton.Zoom == PdrZoom.Petit)
                    {
                        this.RepetitionGauche = 4;
                        this.RepetitionDroit = 4;
                    }
                    else
                    {
                        this.RepetitionGauche = 2;
                        this.RepetitionDroit = 2;
                    }
                    break;
                case PdrActionType.PasDeModificationAction:
                    if (Singleton.Zoom == PdrZoom.Petit)
                    {
                        this.RepetitionGauche = 4;
                        this.RepetitionDroit = 4;
                    }
                    else
                    {
                        this.RepetitionGauche = 2;
                        this.RepetitionDroit = 2;
                    }
                    break;
                case PdrActionType.EgaliteLaterale:
                    if (Singleton.Zoom == PdrZoom.Petit)
                    {
                        this.RepetitionGauche = 4;
                        this.RepetitionDroit = 4;
                    }
                    else
                    {
                        this.RepetitionGauche = 2;
                        this.RepetitionDroit = 2;
                    }
                    break;
                case PdrActionType.EgaliteLateraleForce:
                    if (Singleton.Zoom == PdrZoom.Petit)
                    {
                        this.RepetitionGauche = 4;
                        this.RepetitionDroit = 4;
                    }
                    else
                    {
                        this.RepetitionGauche = 2;
                        this.RepetitionDroit = 2;
                    }
                    break;
                case PdrActionType.EgaliteLateraleTemps:
                    if (Singleton.Zoom == PdrZoom.Petit)
                    {
                        this.RepetitionGauche = 4;
                        this.RepetitionDroit = 4;
                    }
                    else
                    {
                        this.RepetitionGauche = 2;
                        this.RepetitionDroit = 2;
                    }
                    break;
                case PdrActionType.BiaisLateral:
                    if (Singleton.Zoom == PdrZoom.Petit)
                    {
                        if (Singleton.Options == PdrOptions.Option1)
                        {
                            this.RepetitionGauche = 6;
                            this.RepetitionDroit = 2;
                        }
                        else if (Singleton.Options == PdrOptions.Option2)
                        {
                            this.RepetitionGauche = 2;
                            this.RepetitionDroit = 6;
                        }
                    }
                    else
                    {
                        if (Singleton.Options == PdrOptions.Option1)
                        {
                            this.RepetitionGauche = 3;
                            this.RepetitionDroit = 1;
                        }
                        else if (Singleton.Options == PdrOptions.Option2)
                        {
                            this.RepetitionGauche = 1;
                            this.RepetitionDroit = 3;
                        }
                    }
                    break;
                case PdrActionType.BiaisLateralForce:
                    if (Singleton.Zoom == PdrZoom.Petit)
                    {
                        if (Singleton.Options == PdrOptions.Option1)
                        {
                            this.RepetitionGauche = 6;
                            this.RepetitionDroit = 2;
                        }
                        else if (Singleton.Options == PdrOptions.Option2)
                        {
                            this.RepetitionGauche = 2;
                            this.RepetitionDroit = 6;
                        }
                    }
                    else
                    {
                        if (Singleton.Options == PdrOptions.Option1)
                        {
                            this.RepetitionGauche = 3;
                            this.RepetitionDroit = 1;
                        }
                        else if (Singleton.Options == PdrOptions.Option2)
                        {
                            this.RepetitionGauche = 1;
                            this.RepetitionDroit = 3;
                        }
                    }
                    break;
                case PdrActionType.BiaisLateralTemps:
                    if (Singleton.Zoom == PdrZoom.Petit)
                    {
                        if (Singleton.Options == PdrOptions.Option1)
                        {
                            this.RepetitionGauche = 6;
                            this.RepetitionDroit = 2;
                        }
                        else if (Singleton.Options == PdrOptions.Option2)
                        {
                            this.RepetitionGauche = 2;
                            this.RepetitionDroit = 6;
                        }
                    }
                    else
                    {
                        if (Singleton.Options == PdrOptions.Option1)
                        {
                            this.RepetitionGauche = 3;
                            this.RepetitionDroit = 1;
                        }
                        else if (Singleton.Options == PdrOptions.Option2)
                        {
                            this.RepetitionGauche = 1;
                            this.RepetitionDroit = 3;
                        }
                    }
                    break;
                case PdrActionType.FacilitationLaterale:
                    if (Singleton.Zoom == PdrZoom.Petit)
                    {
                        if (Singleton.Options == PdrOptions.Option1)
                        {
                            this.RepetitionGauche = 6;
                            this.RepetitionDroit = 2;
                        }
                        else if (Singleton.Options == PdrOptions.Option2)
                        {
                            this.RepetitionGauche = 2;
                            this.RepetitionDroit = 6;
                        }
                    }
                    else
                    {
                        if (Singleton.Options == PdrOptions.Option1)
                        {
                            this.RepetitionGauche = 3;
                            this.RepetitionDroit = 1;
                        }
                        else if (Singleton.Options == PdrOptions.Option2)
                        {
                            this.RepetitionGauche = 1;
                            this.RepetitionDroit = 3;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void ChangerChoix(int choix)
        {
            if (choix == 0)
            {
                this.CreationCibleAmplitudeArticulaireAide();
            }
            else if (choix == 1)
            {
                this.CreationCibleAmplitudeArticulaire();
            }
            else
            {
                this.CreationCiblesDefaut();
            }
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
            Singleton.RepetitionGauche = this.RepetitionGauche;
            Singleton.RepetitionDroit = this.RepetitionDroit;
            Singleton.CiblesGauche = new List<int>();
            Singleton.CiblesDroite = new List<int>();
            int nbrSelectedGauche = 0;
            int nbrSelectedDroit = 0;
            // Trouve toutes les cibles cochées et les  donne au singleton
            foreach (var cible in this.CiblesGauche)
            {
                if (cible.Selected)
                {
                    Singleton.CiblesGauche.Add(cible.Number);
                    nbrSelectedGauche++;
                }
            }

            foreach (var cible in this.CiblesDroite)
            {
                if (cible.Selected)
                {
                    Singleton.CiblesDroite.Add(cible.Number);
                    nbrSelectedDroit++;
                }
            }

            // Regarde si le nombre de cible des deux coté est bien paire
            var nbrCibleGauche = this.RepetitionGauche * nbrSelectedGauche;
            var nbrCibleDroit = this.RepetitionDroit * nbrSelectedDroit;

            if (nbrCibleGauche % 2 == 0 && nbrCibleDroit % 2 == 0)
            {
                this.nav.NavigateTo<ModifActionVisualisationViewModel>(this, null, true);
            }
            else
            {
                msg.ShowWarning("Le nombre de cible doit être pair !");
            }
        }

        private void GoBack()
        {
            nav.GoBack("ResetFromBack", new object[] { });
        }
        #endregion 
    }
}

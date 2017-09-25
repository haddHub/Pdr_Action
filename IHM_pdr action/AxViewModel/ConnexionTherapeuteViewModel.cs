using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using Navegar;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Controls;
using AxModel;
using GalaSoft.MvvmLight.Ioc;
using AxData;
using AxError;
using Common.Logging;
using System.ComponentModel;
namespace AxViewModel
{
    public class ConnexionTherapeuteViewModel : BlankViewModelBase, IDataErrorInfo
    {
        //Fields
        private List<ViewModelBase> _pagesInternes;
        private PasswordBox pwBox;
        private ILog logger = LogManager.GetCurrentClassLogger();
        private bool CanUseBoutton;
        private bool FirstTime;
        private bool _isEnConencted;

        public bool IsConnected
        {
            get { return _isEnConencted; }
            set { _isEnConencted = value; }
        }


        //Constructor
        public ConnexionTherapeuteViewModel()
        {
            try
            {
                CanUseBoutton = false;
                FirstTime = true;
                InitNavigation();
                CreateCommands();
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }
        #region properties
        //Properties
        public RelayCommand<object> ConnexionCommand { get; set; }
        public RelayCommand<string> ChangeInternNavigation { get; set; }
        public RelayCommand<object> InscriptionCommand { get; set; }
        public RelayCommand<object> MoveToInscriptionCommand { get; set; }

        private ViewModelBase _internView;
        public ViewModelBase InternView
        {
            get { return _internView; }
            set
            {
                if (_internView != value)
                {
                    _internView = value;
                    RaisePropertyChanged("InternView");
                }
            }
        }

        private string _nomUtilisateur;

        public string NomUtilisateur
        {
            get { return _nomUtilisateur; }
            set
            {
                if (_nomUtilisateur != value)
                {
                    _nomUtilisateur = value;
                    RaisePropertyChanging(NomUtilisateur);
                    FirstTime = false;
                }
            }
        }

        private string _prenom;

        public string Prenom
        {
            get { return _prenom; }
            set
            {
                if (_prenom != value)
                {
                    _prenom = value;
                    RaisePropertyChanged("Prenom");
                    FirstTime = false;
                }
            }
        }

        private string _nom;

        public string Nom
        {
            get { return _nom; }
            set
            {
                if (_nom != value)
                {
                    _nom = value;
                    RaisePropertyChanged("Nom");
                    FirstTime = false;
                }
            }
        }

        private string _mdp;

        public string Mdp
        {
            get { return _mdp; }
            set 
            { 
                _mdp = value;
                RaisePropertyChanged("Mdp");
                RaisePropertyChanged("MdpConfirm");
                FirstTime = false;
            }
        }

        private string _mdpConfirm;

        public string MdpConfirm
        {
            get { return _mdpConfirm; }
            set
            {
                _mdpConfirm = value;
                RaisePropertyChanged("MdpConfirm");
                FirstTime = false;
            }
        }

        public List<ViewModelBase> PagesInternes
        {
            get
            {
                if (_pagesInternes == null)
                    _pagesInternes = new List<ViewModelBase>();

                return _pagesInternes;
            }
        }
        #endregion

        //Methods
        private void CreateCommands()
        {
            MoveToInscriptionCommand = new RelayCommand<object>(MoveToInscription);
            ChangeInternNavigation = new RelayCommand<string>((p) => changeInternView(p));
            InscriptionCommand = new RelayCommand<object>(Inscription, canExecuteInscription);
            ConnexionCommand = new RelayCommand<object>(Connexion);
        }

        private bool canExecuteInscription(Object o)
        {
            if (CanUseBoutton == true)
                return true;
            else
                return false;
        }

        private void MoveToInscription(object obj)
        {
            try
            {
                pwBox = obj as PasswordBox;
                TherapeuteDB therapeute = AdminData.Connexion(NomUtilisateur, pwBox.Password);

                if (therapeute != null)
                {
                    if (AdminData.IsAdministrateur(NomUtilisateur, pwBox.Password))
                    {
                        NomUtilisateur = null;
                        //Nom = null;
                        //Prenom = null;
                        FirstTime = true;
                        InternView = PagesInternes[1];
                    }
                    else
                        MessageBox.Show(AxLanguage.Languages.REAplan_Impossible_Enregistrer_Utilisateur, AxLanguage.Languages.REAplan_Inscription_Erreur, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    MessageBox.Show(AxLanguage.Languages.REAplan_Connexion_Erreur, AxLanguage.Languages.REAplan_Inscription_Erreur, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        private void Connexion(object obj)
        {
            try
            {
                pwBox = obj as PasswordBox;

                TherapeuteDB therapeute = AdminData.Connexion(NomUtilisateur, pwBox.Password);

                if (therapeute != null)
                {
                    Singleton.identificationAdmin();
                    Singleton singletonAdmin = Singleton.getInstance();
                    singletonAdmin.Admin = (new Admin(therapeute.Nom, therapeute.Prenom, therapeute.Login, therapeute.MotDePasse));
                    NomUtilisateur = null;
                    SimpleIoc.Default.GetInstance<INavigation>().NavigateTo<HomeViewModel>(true);
                    //log le thérapeut connecté
                    logger.Info("Connexion de " + therapeute.Prenom + " " + therapeute.Nom + "  Login : " + therapeute.Login);
                }
                else
                    MessageBox.Show(AxLanguage.Languages.REAplan_Connexion_Erreur2, AxLanguage.Languages.REAplan_Inscription_Erreur, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        private void changeInternView(string p)
        {
            try
            {
                InternView = PagesInternes[Convert.ToInt32(p, 10)];
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        private void InitNavigation()
        {
            try
            {
                PagesInternes.Add(new FormulaireTherapeuteViewModel());
                PagesInternes.Add(new FormulaireInscriptionTherapeuteViewModel());

                InternView = PagesInternes[0];
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }


        public void Inscription(object obj)
        {
            try
            {
                pwBox = obj as PasswordBox;
                AdminData.InscriptionTherapeute(Nom, Prenom, NomUtilisateur, pwBox.Password,false);

                MessageBox.Show(AxLanguage.Languages.REAplan_Utilisateur_Enregistre, AxLanguage.Languages.REAplan_Enregistrement_Therapeute, MessageBoxButton.OK, MessageBoxImage.Asterisk);
                InternView = PagesInternes[0];
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        #region DataValidation
        public string Error
        {
            get { throw new NotImplementedException(); }
        }
        public string this[string columnName]
        {
            get
            {
                string ErrorMessage = "";

                try
                {
                    if (FirstTime == false)
                    {
                        switch (columnName)
                        {
                            case "Nom":
                                ErrorMessage = ValidationData.ValidationNomPrenom(Nom, 0);
                                break;
                            case "Prenom":
                                ErrorMessage = ValidationData.ValidationNomPrenom(Prenom, 1);
                                break;
                            case "NomUtilisateur":
                                ErrorMessage = ValidationData.ValidationNomUtilisateur(NomUtilisateur);
                                break;
                            case "Mdp":
                                string mdpConfirmFake;
                                if (MdpConfirm == null) mdpConfirmFake = "";
                                else mdpConfirmFake = MdpConfirm;
                                ErrorMessage = ValidationData.ValidationMdp(Mdp);
                                break;
                            case "MdpConfirm":
                                string mdpFake;
                                if (Mdp == null) mdpFake = "";
                                else mdpFake = Mdp;
                                ErrorMessage = ValidationData.ValidationMdpConfirm(mdpFake,MdpConfirm);
                                break;
                        }
                        Therapeute t = new Therapeute(Nom, Prenom, NomUtilisateur, Mdp);
                        t.MdpConfirm = MdpConfirm;
                        CanUseBoutton = ValidationData.IsTherapeuteValid(t);
                    }
                }
                catch (Exception ex)
                {
                    GestionErreur.GerrerErreur(ex);
                }
                return ErrorMessage;
            }
        #endregion
        }
    }
}

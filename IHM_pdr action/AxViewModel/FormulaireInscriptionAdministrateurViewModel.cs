using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using System.ComponentModel;
using AxError;
using GalaSoft.MvvmLight.Command;
using AxData;
using System.Windows;
using AxModel;
using Navegar;
using GalaSoft.MvvmLight.Ioc;

namespace AxViewModel
{
    public class FormulaireInscriptionAdministrateurViewModel : BlankViewModelBase, IDataErrorInfo
    {
        private bool FirstTime;
        private bool CanUseButton;
        public INavigation _nav;
        #region Proprietes
        public RelayCommand InscriptionCommand { get; set; }
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
        #endregion

        public FormulaireInscriptionAdministrateurViewModel()
        {
            try
            {
                _nav = SimpleIoc.Default.GetInstance<INavigation>();
                FirstTime = true;
                CanUseButton = false;
                InscriptionCommand = new RelayCommand(Inscription, canExecuteInscription);
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        private void Inscription()
        {
            try
            {
                AdminData.InscriptionTherapeute(Nom, Prenom, NomUtilisateur, Mdp, true);
                MessageBox.Show(AxLanguage.Languages.REAplan_Utilisateur_Enregistre, AxLanguage.Languages.REAplan_Enregistrement_Therapeute, MessageBoxButton.OK, MessageBoxImage.Asterisk);
                _nav.NavigateTo<ConnexionTherapeuteViewModel>();
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }
        private bool canExecuteInscription()
        {
            if (CanUseButton == true)
                return true;
            else
                return false;
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
                                ErrorMessage = ValidationData.ValidationMdpConfirm(mdpFake, MdpConfirm);
                                break;
                        }
                        Therapeute t = new Therapeute(Nom, Prenom, NomUtilisateur, Mdp);
                        t.MdpConfirm = MdpConfirm;
                        CanUseButton = ValidationData.IsTherapeuteValid(t);
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

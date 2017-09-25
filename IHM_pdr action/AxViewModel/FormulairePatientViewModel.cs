using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using Navegar;
using AxModel;
using GalaSoft.MvvmLight.Ioc;
using System.IO;
using System.Windows;
using AxData;
using System.Collections.ObjectModel;
using System.Data.Objects;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using AxError;
using Common.Logging;
namespace AxViewModel
{
    public class FormulairePatientViewModel : ViewModelBase
    {
        #region constructor
        public FormulairePatientViewModel()
        {
            try
            {
                singleUser = Singleton.getInstance();
                _nav = SimpleIoc.Default.GetInstance<INavigation>();
                CreateCommands();
                ListeNom = PatientData.GetListeNom();
                ListePrenom = PatientData.GetListePrenom();
                Messenger.Default.Register<Singleton>(this, "Singleton", OnConnected);
                Messenger.Default.Register<bool>(this, "ConnInsc", ConnInsc);
                Messenger.Default.Register<bool>(this, "ConnSupp", ConnSupp);
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }
        #endregion

        #region var
        Singleton singleUser;

        private bool tryCo = false;
            private Singleton User = Singleton.getInstance();

            private const string NomPatientPropertyName = "NomPatient";
            private string _nomPatient;

            private const string PrenomPatientPropertyName = "PrenomPatient";
            private string _prenomPatient;

            private const string SelectNPropertyName = "SelectN";
            private string _selectN;

            private const string SelectPPropertyName = "SelectP";
            private string _selectP;

            private const string ResultRechPropertyName = "ResultRech";
            private ObservableCollection<ListePatientDataGrid> _resultRech = null;

            private const string ResultRech2PropertyName = "ResultRech2";
            private ObservableCollection<PatientDB> _resultRech2 = null;

            private const string SelectResultRechPropertyName = "SelectResultRech";
            private ListePatientDataGrid _selectResultRech = null;

            private const string ListeNomPropertyName = "ListeNom";
            private ObservableCollection<string> _listeNom = null;

            private const string ListePrenomPropertyName = "ListePrenom";
            private ObservableCollection<string> _listePrenom = null;

            private const string IsEnConnPropertyName = "IsEnConn";
            public INavigation _nav;

            private ILog logger = LogManager.GetCurrentClassLogger(); 
       //     private bool _isEnConn = false;




        #endregion

        #region property

            public string NomPatient
            {
                get { return _nomPatient; }
                set
                {
                    if (_nomPatient == value)
                    {
                        return;
                    }

                    RaisePropertyChanging(NomPatientPropertyName);
                    _nomPatient = value;
                    RaisePropertyChanged(NomPatientPropertyName);

                }
            }

            public string PrenomPatient
            {
                get { return _prenomPatient; }
                set
                {
                    if (_prenomPatient == value)
                    {
                        return;
                    }

                    RaisePropertyChanging(PrenomPatientPropertyName);
                    _prenomPatient = value;
                    RaisePropertyChanged(PrenomPatientPropertyName);
                }
            }

            public string SelectN
            {
                get { return _selectN; }
                set
                {
                    if (_selectN == value)
                    {
                        return;
                    }

                    RaisePropertyChanging(SelectNPropertyName);
                    _selectN = value;
                    RaisePropertyChanged(SelectNPropertyName);
                    NomPatient = SelectN;
                    RecherchePatient(); 
                }
            }

            public string SelectP
            {
                get { return _selectP; }
                set
                {
                    if (_selectP == value)
                    {
                        return;
                    }

                    RaisePropertyChanging(SelectPPropertyName);
                    _selectP = value;
                    RaisePropertyChanged(SelectPPropertyName);
                    PrenomPatient = SelectP;
                    RecherchePatient();
                }
            }

            public ObservableCollection<ListePatientDataGrid> ResultRech
            {
                get { return _resultRech; }
                set
                {
                    if (_resultRech == value)
                    {
                        _resultRech = value;
                    }

                    RaisePropertyChanging(ResultRechPropertyName);
                    _resultRech = value;
                    RaisePropertyChanged(ResultRechPropertyName);
                }
            }

            private ListePatientDataGrid saveSelect;
            public ListePatientDataGrid SelectResultRech
            {
                get { return _selectResultRech; }
                set
                {
                    if (_selectResultRech == value)
                    {
                        _selectResultRech = value;
                    }

                    RaisePropertyChanging(SelectResultRechPropertyName);
                    _selectResultRech = value;
                    RaisePropertyChanged(SelectResultRechPropertyName);

                }
            }
            private bool _isSelected;

            public bool IsSelected
            {
                get
                {
                    return _isSelected;
                }
                set
                {
                    if (_isSelected != value)
                    {
                        _isSelected = value;
                        RaisePropertyChanged("IsSelected");
                    }
                }
            }

            private bool _isEnConn;

            public bool IsEnConn
            {
                get
                {
                    return _isEnConn;
                }
                set
                {
                    if (_isEnConn != value)
                    {
                        _isEnConn = value;
                        RaisePropertyChanged("IsEnConn");
                    }
                }
            }

            public ObservableCollection<string> ListeNom
            {
                get { return _listeNom; }
                set
                {
                    if (_listeNom == value)
                    {
                        _listeNom = value;
                    }

                    RaisePropertyChanging(ListeNomPropertyName);
                    _listeNom = value;
                    RaisePropertyChanged(ListeNomPropertyName);
                }
            }

            public ObservableCollection<string> ListePrenom
            {
                get { return _listePrenom; }
                set
                {
                    if (_listePrenom == value)
                    {
                        _listePrenom = value;
                    }

                    RaisePropertyChanging(ListePrenomPropertyName);
                    _listePrenom = value;
                    RaisePropertyChanged(ListePrenomPropertyName);
                }
            }

        #endregion

        #region command

            public RelayCommand CmdSup
            {
                get { return new RelayCommand(SupPatient, canExecute); }
            }

            public RelayCommand ConnexionCommand
            {
                get;
                private set;
            }

        #endregion

        #region methode

        private void CreateCommands()
            {
                ConnexionCommand = new RelayCommand(() => Connexion(), canExecute);
            }

        private void OnConnected(Singleton obj)
        {
            try
            {
                IsEnConn = true;
                IsSelected = false;
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        public ObservableCollection<string> GetListeNom()
        {
            ObservableCollection<string> result = new ObservableCollection<string>();

            using (ReaPlanDBEntities context = new ReaPlanDBEntities())
            {
                ObjectSet<PatientDB> query = context.PatientDBs;

                ObjectResult<PatientDB> queryResult = query.Execute(MergeOption.AppendOnly);
                foreach (PatientDB patient in queryResult)
                {
                    result.Add(patient.Nom);
                }
            }

            ObservableCollection<string> ListSansDuplication = new ObservableCollection<string>();
            foreach (string s in result)
                if (!ListSansDuplication.Contains(s))
                    ListSansDuplication.Add(s);

            return ListSansDuplication;
        }

        public ObservableCollection<string> GetListePrenom()
        {
            ObservableCollection<string> result = new ObservableCollection<string>();

            using  (ReaPlanDBEntities context = new  ReaPlanDBEntities())
            {
                ObjectSet<PatientDB> query = context.PatientDBs;

                ObjectResult<PatientDB> queryResult = query.Execute(MergeOption.AppendOnly);
                foreach (PatientDB patient in queryResult)
                {
                    result.Add(patient.Prenom);
                }
            }

            ObservableCollection<string> ListSansDuplication = new ObservableCollection<string>();
            foreach (string s in result)
                if (!ListSansDuplication.Contains(s))
                    ListSansDuplication.Add(s);

            return ListSansDuplication;
        }

        private void Connexion()
        {
            try
            {
                tryCo = true;
                Singleton.logOffPatient();
                IsEnConn = true;
                Singleton.identification();
                singleUser.Patient = SelectResultRech;

                PatientDB patient = PatientData.RecherchePatient(singleUser.Patient.Nom, singleUser.Patient.Prenom, Convert.ToDateTime(SelectResultRech.DateDeNaissance));


                if (patient != null)
                {
                    singleUser.PatientSingleton = new Patient(patient.Nom, patient.Prenom, patient.DateNaissance, patient.Sexe, (int)patient.Taille, (double)patient.Poids, (int)patient.Id1, (int)patient.Id2);
                }
                //log le patient qui à été connecté et la thérapeut qui l'a connecté
                logger.Info("Le patient " + patient.Prenom + " " + patient.Nom + " a été connecté par : " + Singleton.getInstance().Admin.Prenom + " " + Singleton.getInstance().Admin.Nom);
                MessageBox.Show(AxLanguage.Languages.REAplan_Connexion_Patient, AxLanguage.Languages.REAplan_Connexion, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Asterisk);
                _nav.NavigateTo<HomeViewModel>(true);

                IsSelected = false;
                RecherchePatient();
                Messenger.Default.Send<Singleton>(singleUser, "Singleton"); //Message envoyé à HomeViewModel avec le singleton
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        private void RecherchePatient()
        {
            try
            {
                ResultRech = new ObservableCollection<ListePatientDataGrid>();
                ObjectResult<PatientDB> queryResult = PatientData.RecherchePateint2();

                using (ReaPlanDBEntities context = new ReaPlanDBEntities())
                {
                    ObjectSet<PatientDB> query = context.PatientDBs;
                    queryResult = query.Execute(MergeOption.AppendOnly);

                    foreach (PatientDB result in queryResult)
                    {
                        if (result.Nom == SelectN || result.Prenom == SelectP)
                        {
                            if (singleUser.PatientSingleton == null || (result.Nom != singleUser.PatientSingleton.Nom || result.Prenom != singleUser.PatientSingleton.Prenom || result.DateNaissance != singleUser.PatientSingleton.DateNaiss))
                            {
                                ListePatientDataGrid p = new ListePatientDataGrid();
                                p.Nom = result.Nom;
                                p.Prenom = result.Prenom;
                                p.DateDeNaissance = result.DateNaissance.ToShortDateString();
                                ResultRech.Add(p);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        public void SupPatient()
        {
            try
            {
                if (MessageBox.Show(AxLanguage.Languages.REAplan_Supprimer_Confirmation, AxLanguage.Languages.REAplan_Confirmation, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    Singleton single = Singleton.getInstance();
                    DateTime DateDeNaissance = Convert.ToDateTime(SelectResultRech.DateDeNaissance);
                    PatientData.SupPatient(SelectResultRech.Nom, SelectResultRech.Prenom, DateDeNaissance);

                    MessageBox.Show(AxLanguage.Languages.REAplan_Supprimer_Patient, AxLanguage.Languages.REAplan_Suppression, MessageBoxButton.OK, MessageBoxImage.Exclamation);

                    ResultRech = new ObservableCollection<ListePatientDataGrid>();
                    using (ReaPlanDBEntities context = new ReaPlanDBEntities())
                    {
                        ObjectSet<PatientDB> query = context.PatientDBs;
                        ObjectResult<PatientDB> queryResult = query.Execute(MergeOption.AppendOnly);
                        foreach (PatientDB result in queryResult)
                            if (result.Nom == SelectN || result.Prenom == SelectP)
                            {
                                if (single.PatientSingleton == null || (result.Nom != single.PatientSingleton.Nom || result.Prenom != single.PatientSingleton.Prenom || result.DateNaissance != single.PatientSingleton.DateNaiss))
                                {
                                    ListePatientDataGrid p = new ListePatientDataGrid();
                                    p.Nom = result.Nom;
                                    p.Prenom = result.Prenom;
                                    p.DateDeNaissance = result.DateNaissance.ToShortDateString();
                                    ResultRech.Add(p);
                                }
                            }
                    }
                    ListeNom = GetListeNom();
                    ListePrenom = GetListePrenom();
                    NomPatient = null;
                    PrenomPatient = null;
                }
                SimpleIoc.Default.GetInstance<INavigation>().NavigateTo<HomeViewModel>(null, "PostTraitementSupression", null, false);
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        #endregion    


    
        public void ConnInsc(bool y)
        {
            IsEnConn = true;
        }

        public void ConnSupp(bool y)
        {
            try
            {
                Singleton single = Singleton.getInstance();
                IsEnConn = false;
                IsSelected = false;

                if (single.PatientSingleton != null)
                    IsEnConn = true;
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        public bool canExecute()
        {
                if (SelectResultRech != null)
                {
                    IsEnConn = false;
                    return true;
                }
                else
                {
                    if (User.PatientSingleton == null)
                        IsEnConn = false;
                    else
                        IsEnConn = true;

                    return false;
                }
            }
        }
    }

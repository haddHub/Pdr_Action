using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using Navegar;
using System.ComponentModel;
using GalaSoft.MvvmLight.Messaging;
using AxModel.Message;
using AxModel;
using AxError;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using AxData;
using GalaSoft.MvvmLight.Ioc;

namespace AxViewModel
{
    public class FormulaireInscriptionPatientViewModel : ViewModelBase,IDataErrorInfo
    {
        #region ctor
        public FormulaireInscriptionPatientViewModel(string p)
            {
                try
                {
                    _nav = SimpleIoc.Default.GetInstance<INavigation>();
                    DateMax = DateTime.Now.AddYears(-3);
                    DateMinimum = DateTime.Now.AddYears(-110);
                    DateNaiss = DateMax;
                    CanUseBoutton = false;
                    InscOuModif = Convert.ToInt32(p, 10);
                    if (InscOuModif == 2)
                    {
                        NomBoutton = AxLanguage.Languages.REAplan_Enregistrer_Modif;
                        Messenger.Default.Register<Singleton>(this, "Singleton", OnRegister2);
                    }
                    else
                        NomBoutton = AxLanguage.Languages.REAplan_Inscription;
                }
                catch (Exception ex)
                {
                    GestionErreur.GerrerErreur(ex);
                }
            }
        #endregion

        #region var
            private const string MasculinPropertyName = "Masculin";
            private bool _masculin;

            private const string FemininPropertyName = "Feminin";
            private bool _feminin;

            private const string NomPatientPropertyName = "NomPatient";
            private string _nomPatient;

            private const string PrenomPatientPropertyName = "PrenomPatient";
            private string _prenomPatient;

            private const string TaillePatientPropertyName = "TaillePatient";
            private int _taillePatient;

            private const string PoidPatientPropertyName = "PoidPatient";
            private double _poidPatient;

            private const string BrasPatientPropertyName = "BrasPatient";
            private int _brasPatient;
           
            private string _id1;

            private string _id2;

            private const string PatientValuePropertyName = "PatientValue";
            private ListePatientDataGrid _patientValue = null;

            private DateTime dateNaiss;

            private const string NomBouttonPropertyName = "NomBoutton";
            private const string CanUseBouttonPropertyName = "CanUseBoutton";
            private bool firstTime = true;
            private bool canUseButton;
            public INavigation _nav;
        #endregion

        #region property

            public DateTime DateMax { get; set; }
            public DateTime DateMinimum { get; set; }
            public DateTime DateNaiss
            {
                get { return dateNaiss; }
                set
                {
                    if (dateNaiss == value)
                    {
                        return;
                    }

                    RaisePropertyChanging("DateNaiss");
                    dateNaiss = value;
                    RaisePropertyChanged("DateNaiss");
                    IdDate = ConvertirDateToIdDate();
                    RaisePropertyChanged("ID2");
                    firstTime = true;
                }
            }

            public bool Masculin
            {
                get { return _masculin; }
                set
                {
                    if (_masculin == value)
                    {
                        return;
                    }
                    _masculin = value;
                    RaisePropertyChanged(MasculinPropertyName);
                    RaisePropertyChanged("ID1");
                }
            }

            public bool Feminin
            {
                get { return _feminin; }
                set
                {
                    if (_feminin == value)
                    {
                        return;
                    }
                    _feminin = value;
                    RaisePropertyChanged(FemininPropertyName);
                    RaisePropertyChanged("ID1");
                }
            }

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
                    firstTime = false;
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
                    firstTime = false;
                }
            }

            public int TaillePatient
            {
                get { return _taillePatient; }
                set
                {
                    if (_taillePatient == value)
                    {
                        return;
                    }

                    RaisePropertyChanging(TaillePatientPropertyName);
                    _taillePatient = value;
                    RaisePropertyChanged(TaillePatientPropertyName);
                    firstTime = false;
                }
            }

            private string _idDate;
            public string IdDate
            {
                get { return _idDate; }
                set
                {
                    if (_idDate == value)
                    {
                        return;
                    }

                    RaisePropertyChanging("IdDate");
                    _idDate = value;
                    RaisePropertyChanged("IdDate");
                    firstTime = false;
                }
            }

            public string ID1
            {
                get { return _id1; }
                set
                {
                    if (_id1 == value)
                    {
                        return;
                    }

                    RaisePropertyChanging("ID1");
                    _id1 = value;
                    RaisePropertyChanged("ID1");
                    firstTime = false;
                    RaisePropertyChanged("ID2");
                    if (ID1.Equals("") == false)
                    {
                        bool isChar = ID1.Any(i => char.IsLetter(i));
                        if (isChar == false)
                        {
                            if (Convert.ToInt32(ID1) % 2 == 0)//paire
                            {
                                Feminin = true;
                            }
                            else//impaire
                            {
                                Masculin = true;
                            }
                            RaisePropertyChanged("Masculin");
                            RaisePropertyChanged("Feminin");  
                        }
                    }
                }
            }

            public string ID2
            {
                get { return _id2; }
                set
                {
                    if (_id2 == value)
                    {
                        return;
                    }

                    RaisePropertyChanging("ID2");
                    _id2 = value;
                    RaisePropertyChanged("ID2");
                    firstTime = false;
                }
            }

            public double PoidPatient
            {
                get { return _poidPatient; }
                set
                {
                    if (_poidPatient == value)
                    {
                        return;
                    }

                    RaisePropertyChanging(PoidPatientPropertyName);
                    _poidPatient = value;
                    RaisePropertyChanged(PoidPatientPropertyName);
                    firstTime = false;
                }
            }

            public int BrasPatient
            {
                get { return _brasPatient; }
                set
                {
                    if (_brasPatient == value)
                    {
                        return;
                    }

                    RaisePropertyChanging(BrasPatientPropertyName);
                    _brasPatient = value;
                    RaisePropertyChanged(BrasPatientPropertyName);
                }
            }

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

            private string _nomBoutton;

            public string NomBoutton
            {
                get { return _nomBoutton; }
                set
                {
                    if (_nomBoutton == value)
                    {
                        return;
                    }

                    RaisePropertyChanging(NomBouttonPropertyName);
                    _nomBoutton = value;
                    RaisePropertyChanged(NomBouttonPropertyName);
                }
            }
            public bool CanUseBoutton
            {
                get { return canUseButton; }
                set
                {
                    if (canUseButton == value)
                    {
                        return;
                    }

                    RaisePropertyChanging(CanUseBouttonPropertyName);
                    canUseButton = value;
                    RaisePropertyChanged(CanUseBouttonPropertyName);
                }
            }

            public int InscOuModif { get; set; }
            #endregion

        #region command
            public RelayCommand CmdSavePatient
            {
                get { return new RelayCommand(SavePatient); }
            }
            #endregion

        #region methode

            private void SavePatient()
            {
                try
                {
                    string sexe;
                    if (Masculin == true)
                    {
                        sexe = "M";
                    }
                    else
                    {
                        sexe = "F";
                    }
                    PrenomPatient = PrenomPatient.Substring(0, 1).ToUpper() + PrenomPatient.Substring(1, PrenomPatient.Length - 1).ToLower();
                    NomPatient = NomPatient.ToUpper();
                    int id1Int = Convert.ToInt32(ID1);
                    int id2Int = Convert.ToInt32(ID2);
                    Singleton singleUser = Singleton.getInstance();
                    Patient patient;
                    if (singleUser.PatientSingleton != null)
                    {
                        patient = new Patient(singleUser.PatientSingleton.Nom, singleUser.PatientSingleton.Prenom, singleUser.PatientSingleton.DateNaiss, sexe, TaillePatient, PoidPatient, 0, 0);
                    }
                    else
                    {
                        patient = new Patient(NomPatient, PrenomPatient, DateNaiss, sexe, TaillePatient, PoidPatient, 0, 0);
                    }
                    Patient newPatient = new Patient(NomPatient, PrenomPatient, DateNaiss, sexe, TaillePatient, PoidPatient, 0, 0);
                    singleUser.Patient = new ListePatientDataGrid();
                    singleUser.Patient.Nom = NomPatient;
                    singleUser.Patient.Prenom = PrenomPatient;
                    singleUser.Patient.DateDeNaissance = DateNaiss.ToShortDateString();
                    if (InscOuModif == 2)//Modification
                    {
                        if (MessageBox.Show(AxLanguage.Languages.REAplan_Modification_Demande_Confirmation,AxLanguage.Languages.REAplan_Confirmation, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            PatientData.ModificationPatient(patient, newPatient);
                            MessageBox.Show(AxLanguage.Languages.REAplan_Modification_Confirmation);
                        }
                    }
                    else
                    {

                        PatientData.InscriptionPatient(patient);
                        MessageBox.Show(AxLanguage.Languages.REAplan_Patient_Enregistre);
                        NomPatient = null;
                        PrenomPatient = null;
                        DateNaiss = DateMax;
                        sexe = null;
                        TaillePatient = 0;
                        PoidPatient = 0;
                        firstTime = true;
                    }
                    Singleton singleUser2 = Singleton.getInstance();
                    singleUser2.PatientSingleton = patient;
                    Messenger.Default.Send<Singleton>(singleUser2, "Singleton");
                    _nav.NavigateTo<HomeViewModel>(null, "ToLoad", null, false);
                }
                catch (Exception ex)
                {
                    GestionErreur.GerrerErreur(ex);
                }
            }

            private void OnRegister2(Singleton obj)
            {
                try
                {
                    NomPatient = obj.PatientSingleton.Nom;
                    PrenomPatient = obj.PatientSingleton.Prenom;
                    DateNaiss = obj.PatientSingleton.DateNaiss;
                    TaillePatient = obj.PatientSingleton.Taille;
                    PoidPatient = obj.PatientSingleton.Poid;
                    if (obj.PatientSingleton.Sexe == "M" || obj.PatientSingleton.Sexe == "m")
                        Masculin = true;
                    else
                        Feminin = true;

                    ID1 = obj.PatientSingleton.ID1.ToString();
                    ID2 = obj.PatientSingleton.ID2.ToString();
                    if (ID1.Length == 2)
                        ID1 = "0" + ID1;
                    if (ID1.Length == 1)
                        ID1 = "00" + ID1;
                    if (ID2.Length == 1)
                        ID2 = "0" + ID2;
                }
                catch (Exception ex)
                {
                    GestionErreur.GerrerErreur(ex);
                }
            }

            private string ConvertirDateToIdDate()
            {
                string dateId,day,month;
                if (DateNaiss.Month.ToString().Length == 1)
                    month = "0" + DateNaiss.Month.ToString();
                else
                    month = DateNaiss.Month.ToString();
                if (DateNaiss.Day.ToString().Length == 1)
                    day = "0" + DateNaiss.Day.ToString();
                else
                    day = DateNaiss.Day.ToString();
                dateId = DateNaiss.Year.ToString().Substring(2, 2) + "" + month + "" + day;
                return dateId;
            }

            public string Name
            {
                get { return "Inscription"; }
            }
            #endregion

            #region DataValidation
            public string Error
            {
                get { throw new NotImplementedException(); }
            }
            //Erreurs sur les champs du formulaire d'inscription/modification
            //Le message d'erreur est fournit par le moduel AxError
            public string this[string columnName]
            {
                get
                {
                    string ErrorMessage = "";

                    try
                    {
                        if (firstTime == false)
                        {
                            switch (columnName)
                            {
                                case "NomPatient":
                                    ErrorMessage = ValidationData.ValidationNomPrenom(NomPatient, 0);
                                    break;
                                case "PrenomPatient":
                                    ErrorMessage = ValidationData.ValidationNomPrenom(PrenomPatient, 1);
                                    break;
                                case "TaillePatient":
                                    ErrorMessage = ValidationData.ValidationTaille(TaillePatient);
                                    break;
                                case "PoidPatient":
                                    ErrorMessage = ValidationData.ValidationPoids(PoidPatient);
                                    break;
                            }
                            Patient p = new Patient(NomPatient, PrenomPatient, TaillePatient, PoidPatient);
                            p.DateNaiss = DateNaiss;
                            if (Masculin == true)
                                p.Sexe = "m";
                            else
                                p.Sexe = "f";
                            CanUseBoutton = ValidationData.IsPatientValid(p);
                        }
                    }
                    catch (Exception ex)
                    {
                        GestionErreur.GerrerErreur(ex);
                    }
                    return ErrorMessage;
                }
            }
            #endregion
        }
}

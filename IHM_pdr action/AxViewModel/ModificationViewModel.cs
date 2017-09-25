using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using AxModel.Message;
using AxModel;
using System.Windows;
using AxError.Exceptions;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.IO;
using AxData;

namespace AxViewModel
{
    public class ModificationViewModel : ViewModelBase, IPageViewModel
    {
 
        #region ctor
        public ModificationViewModel()
        {
            Messenger.Default.Register<PatientMessage>(this, OnRegister);
            JourNaiss = new ObservableCollection<int>();
            for (int i = 1; i <= 31; i++)
            {
                JourNaiss.Add(i);
            }

            MoisNaiss = new ObservableCollection<string>();
            MoisNaiss.Add("Janvier");
            MoisNaiss.Add("Février");
            MoisNaiss.Add("Mars");
            MoisNaiss.Add("Avril");
            MoisNaiss.Add("Mai");
            MoisNaiss.Add("Juin");
            MoisNaiss.Add("Juillet");
            MoisNaiss.Add("Aout");
            MoisNaiss.Add("Septembre");
            MoisNaiss.Add("Octobre");
            MoisNaiss.Add("Novembre");
            MoisNaiss.Add("Décembre");

            AnneeNaiss = new ObservableCollection<int>();
            for (int i = DateTime.Now.Year; i >= 1950; i--)
            {
                AnneeNaiss.Add(i);
            }

        }
        #endregion

        #region var

        private const string PatientValuePropertyName = "PatientValue";
        private ListePatientDataGrid _patientValue = null;

            private const string JourNaissPropertyName = "JourNaiss";
            private ObservableCollection<int> _jourNaiss = null;

            private const string MoisNaissPropertyName = "MoisNaiss";
            private ObservableCollection<string> _moisNaiss = null;

            private const string AnneeNaissPropertyName = "AnneeNaiss";
            private ObservableCollection<int> _anneeNaiss = null;

            private const string MasculinPropertyName = "Masculin";
            private bool _masculin = true;

            private const string FemininPropertyName = "Feminin";
            private bool _feminin;

            private const string NomPatientPropertyName = "NomPatient";
            private string _nomPatient;

            private const string PrenomPatientPropertyName = "PrenomPatient";
            private string _prenomPatient;

            private const string SelJourNaissPropertyName = "SelJourNaiss";
            private int _selJourNaiss;

            private const string SelMoisNaissPropertyName = "SelMoisNaiss";
            private string _selMoisNaiss;

            private const string SelAnneeNaissPropertyName = "SelAnneeNaiss";
            private int _selAnneeNaiss;

            private const string TaillePatientPropertyName = "TaillePatient";
            private int _taillePatient;

            private const string PoidPatientPropertyName = "PoidPatient";
            private double _poidPatient;

            private const string BrasPatientPropertyName = "BrasPatient";
            private int _brasPatient;




        #endregion

        #region property
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


            public ObservableCollection<int> JourNaiss
            {
                get { return _jourNaiss; }
                set
                {
                    if (_jourNaiss == value)
                    {
                        return;
                    }

                    RaisePropertyChanging(JourNaissPropertyName);
                    _jourNaiss = value;
                    RaisePropertyChanged(JourNaissPropertyName);
                }
            }

            public ObservableCollection<string> MoisNaiss
            {
                get { return _moisNaiss; }
                set
                {
                    if (_moisNaiss == value)
                    {
                        return;
                    }

                    RaisePropertyChanging(MoisNaissPropertyName);
                    _moisNaiss = value;
                    RaisePropertyChanged(MoisNaissPropertyName);
                }
            }

            public ObservableCollection<int> AnneeNaiss
            {
                get { return _anneeNaiss; }
                set
                {
                    if (_anneeNaiss == value)
                    {
                        return;
                    }

                    RaisePropertyChanging(AnneeNaissPropertyName);
                    _anneeNaiss = value;
                    RaisePropertyChanged(AnneeNaissPropertyName);
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

                    RaisePropertyChanging(MasculinPropertyName);
                    _masculin = value;
                    RaisePropertyChanged(MasculinPropertyName);
                    RaisePropertyChanging(FemininPropertyName);
                    _feminin = false;
                    RaisePropertyChanged(FemininPropertyName);
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

                    RaisePropertyChanging(FemininPropertyName);
                    _feminin = value;
                    RaisePropertyChanged(FemininPropertyName);
                    RaisePropertyChanging(MasculinPropertyName);
                    _masculin = false;
                    RaisePropertyChanged(MasculinPropertyName);
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

            public int SelJourNaiss
            {
                get { return _selJourNaiss; }
                set
                {
                    if (_selJourNaiss == value)
                    {
                        return;
                    }

                    RaisePropertyChanging(SelJourNaissPropertyName);
                    _selJourNaiss = value;
                    RaisePropertyChanged(SelJourNaissPropertyName);
                }
            }

            public string SelMoisNaiss
            {
                get { return _selMoisNaiss; }
                set
                {
                    if (_selMoisNaiss == value)
                    {
                        return;
                    }

                    RaisePropertyChanging(SelMoisNaissPropertyName);
                    _selMoisNaiss = value;
                    RaisePropertyChanged(SelMoisNaissPropertyName);
                }
            }

            public int SelAnneeNaiss
            {
                get { return _selAnneeNaiss; }
                set
                {
                    if (_selAnneeNaiss == value)
                    {
                        return;
                    }

                    RaisePropertyChanging(SelAnneeNaissPropertyName);
                    _selAnneeNaiss = value;
                    RaisePropertyChanged(SelAnneeNaissPropertyName);
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
                    if (NomPatient == null)
                    { throw new MessException("Entrez un nom de patient svp"); }
                    if (PrenomPatient == null)
                    { throw new MessException("Entrez un prénom de patient svp"); }
                    if (SelJourNaiss == 0)
                    { throw new MessException("Selctionez un jour de naissance svp"); }
                    if (SelMoisNaiss == null)
                    { throw new MessException("Selctionez un mois de naissance svp"); }
                    if (SelAnneeNaiss == 0)
                    { throw new MessException("Selctionez uue année de naissance svp"); }
                    if (TaillePatient == 0)
                    { throw new MessException("Entrez une taille pour le patient svp"); }
                    if (PoidPatient == 0)
                    { throw new MessException("Entrez un poid pour le patient svp"); }

                    int mois = 0;
                    string sexe;
                    mois = NumMois();
                    DateTime dateNaiss = new DateTime(SelAnneeNaiss, mois, SelJourNaiss);
                    if (Masculin == true)
                    {
                        sexe = "M";
                    }
                    else
                    {
                        sexe = "F";
                    }
                    if (MessageBox.Show("Voulez-vous vraiment modifier le patient?",
                    "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        PrenomPatient = PrenomPatient.Substring(0, 1).ToUpper() + PrenomPatient.Substring(1, PrenomPatient.Length - 1).ToLower();
                        NomPatient = NomPatient.ToUpper();
                        if (PatientValue.Nom != NomPatient || PatientValue.Prenom != PrenomPatient || PatientValue.DateDeNaissance != String.Format("{0:00}", SelJourNaiss) + "/" + String.Format("{0:00}", mois) + "/" + SelAnneeNaiss)
                        {
                            string dossier1 = PatientValue.Nom + PatientValue.Prenom + PatientValue.DateDeNaissance.Substring(0, 2) + PatientValue.DateDeNaissance.Substring(3, 2) + PatientValue.DateDeNaissance.Substring(6, 4);
                            string dossier2 = NomPatient + PrenomPatient + String.Format("{0:00}", SelJourNaiss) + String.Format("{0:00}", mois) + SelAnneeNaiss;
                            Patient p1 = new Patient(dossier1, dossier2, PatientValue.Nom, NomPatient, PatientValue.Prenom, PrenomPatient, PatientValue.DateDeNaissance, String.Format("{0:00}", SelJourNaiss) + "/" + String.Format("{0:00}", mois) + "/" + SelAnneeNaiss);
                            PatientData.ChangeDirectory(p1);
                            PatientValue.Nom = NomPatient;
                            PatientValue.Prenom = PrenomPatient;
                            PatientValue.DateDeNaissance = String.Format("{0:00}", SelJourNaiss) + "/" + String.Format("{0:00}", mois) + "/" + SelAnneeNaiss;
                        }
                        string dossier = NomPatient + PrenomPatient + String.Format("{0:00}", SelJourNaiss) + String.Format("{0:00}", mois) + SelAnneeNaiss;
                        Patient p = new Patient(dossier, TaillePatient, PoidPatient, BrasPatient, sexe);
                        PatientData.ModifPatientInfo(p);
                        MessageBox.Show("Le patient a bien été modifié");
                        Messenger.Default.Send("0", "InscriptionViewModel");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }

            private void OnRegister(PatientMessage obj)
            {
                PatientValue = obj.DataPatient;
                NomPatient = (string)PatientValue.Nom;
                PrenomPatient = PatientValue.Prenom;
                SelJourNaiss = int.Parse(PatientValue.DateDeNaissance.Substring(0, 2));
                SelMoisNaiss = NomMois();
                SelAnneeNaiss = int.Parse(PatientValue.DateDeNaissance.Substring(6, 4));
                string dossier;
                dossier = PatientValue.Nom + PatientValue.Prenom + PatientValue.DateDeNaissance.Substring(0, 2) + PatientValue.DateDeNaissance.Substring(3, 2) + PatientValue.DateDeNaissance.Substring(6, 4);
                Patient p1 = PatientData.RecupInfoPatient(dossier);
                TaillePatient =p1.Taille;
                PoidPatient = p1.Poid;
                BrasPatient = p1.Bras;
                if (p1.Sexe.Equals("M"))
                    Masculin = true;
                else
                    Feminin = true;
            }    

            private void checkExeption()
            {
                throw new NotImplementedException();
            }

            private int NumMois()
            {
                int mois=0;
                switch (SelMoisNaiss)
                {
                    case "Janvier":
                        mois = 01;
                        break;
                    case "Février":
                        mois = 02;
                        break;
                    case "Mars":
                        mois = 03;
                        break;
                    case "Avril":
                        mois = 04;
                        break;
                    case "Mai":
                        mois = 05;
                        break;
                    case "Juin":
                        mois = 06;
                        break;
                    case "Juillet":
                        mois = 07;
                        break;
                    case "Aout":
                        mois = 08;
                        break;
                    case "Septembre":
                        mois = 09;
                        break;
                    case "Octobre":
                        mois = 10;
                        break;
                    case "Novembre":
                        mois = 11;
                        break;
                    case "Décembre":
                        mois = 12;
                        break;
                }
                return mois;
            }

            private string NomMois  ()
            {
                string mois;
                switch (PatientValue.DateDeNaissance.Substring(3, 2))
                {
                    case "01":
                        mois = "Janvier";
                        break;
                    case "02":
                        mois = "Février";
                        break;
                    case "03":
                        mois = "Mars";
                        break;
                    case "04":
                        mois = "Avril";
                        break;
                    case "05":
                        mois = "Mai";
                        break;
                    case "06":
                        mois = "Juin";
                        break;
                    case "07":
                        mois = "Juillet";
                        break;
                    case "08":
                        mois = "Aout";
                        break;
                    case "09":
                        mois = "Septembre";
                        break;
                    case "10":
                        mois = "Octobre";
                        break;
                    case "11":
                        mois = "Novembre";
                        break;
                    default:
                        mois = "Décembre";
                        break;
                }
                return mois;
            }

            #endregion    


        public string Name
        {
            get { return "Modification"; }
        }
    }
}

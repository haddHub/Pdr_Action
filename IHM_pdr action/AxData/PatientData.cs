using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxModel;
using System.Xml.Linq;
using System.IO;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Data.Objects;
using System.Data.EntityClient;

namespace AxData
{
    public static class PatientData
    {
        public static EntityConnection conn;
        private static ReaPlanDBEntities context;

        #region Supprimer
        public static void SupPatient(string nom, string prenom,DateTime dateNaissance)
        {
            //permet au TU de donner la fausse BD
            if (conn == null)
            {
                context = new ReaPlanDBEntities();
                string dossier = "Files/Patients/" + nom + prenom + dateNaissance.ToShortDateString().ToString().Replace("/", string.Empty);
                if (Directory.Exists(dossier))
                {
                    DirectoryInfo directory = new DirectoryInfo(dossier);
                    directory.Delete(true);
                }
            }
            else
                context = new ReaPlanDBEntities(conn);

            using (context)
            {
                var requete = from c in context.PatientDBs
                              where c.Nom == nom
                              && c.Prenom == prenom
                              && c.DateNaissance == dateNaissance
                              select c;

                var patient = requete.FirstOrDefault();

                var requeteEx = from c in context.ExerciceDBs
                                where c.IdPatient == patient.IdPatient
                                select c;

                
                //requeteEx.ToList().ForEach((ExerciceDB ex) =>
                //        {
                //            var requeteParamEx = from c in context.ParametreExDBs
                //                                where c.IdExercice == ex.IdExercice
                //                                select c;
                //            requeteParamEx.ToList().ForEach((ParametreExDB ParamEx) =>
                //            {
                //                context.DeleteObject(ParamEx);
                //                context.SaveChanges();
                //            });
                //            context.DeleteObject(ex);
                //            context.SaveChanges();
                //        }); 

                if (patient != null)
                {
                    context.DeleteObject(patient);
                    context.SaveChanges();
                }
            }
         }
 
        #endregion

        #region Connexion
        public static ObservableCollection<string> GetListeNom()
        {
            ObservableCollection<string> result = new ObservableCollection<string>();

            if (conn == null)
                context = new ReaPlanDBEntities();
            else
                context = new ReaPlanDBEntities(conn);

            using (context)
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
        public static ObservableCollection<string> GetListePrenom()
        {
            ObservableCollection<string> result = new ObservableCollection<string>();

            if (conn == null)
                context = new ReaPlanDBEntities();
            else
                context = new ReaPlanDBEntities(conn);

            using (context)
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
        public static PatientDB RecherchePatient(string nom, string prenom,DateTime dateNaissance)
        {
            if (conn == null)
                context = new ReaPlanDBEntities();
            else
                context = new ReaPlanDBEntities(conn);

            using (context)
            {
                var requete = from c in context.PatientDBs
                              where c.Nom == nom
                              && c.Prenom == prenom
                              && c.DateNaissance == dateNaissance
                              select c;

                PatientDB patient = requete.FirstOrDefault();
                return patient;
            }
        }
        public static ObjectResult<PatientDB> RecherchePateint2()//Recherche de tout les patients sauf celui connecter
        {
            if (conn == null)
                context = new ReaPlanDBEntities();
            else
                context = new ReaPlanDBEntities(conn);

            using (context)
            {
                ObjectSet<PatientDB> query = context.PatientDBs;
                ObjectResult<PatientDB> queryResult = query.Execute(MergeOption.AppendOnly);
                return queryResult;
            }
        }
        #endregion

        #region Inscription
        public static void InscriptionPatient(Patient pat)
        {
            if (conn == null)
            {
                context = new ReaPlanDBEntities();
                pat.CreeDossier();
            }
            else
                context = new ReaPlanDBEntities(conn);

            
            using (context)
            {
                PatientDB patient = PatientDB.CreatePatientDB(pat.Nom,pat.Prenom,pat.DateNaiss,pat.Sexe,(decimal)pat.Taille,(decimal)pat.Poid, pat.ID1, pat.ID2);
                context.AddToPatientDBs(patient);
                context.SaveChanges();
            }
        }
        #endregion

        #region Modification
        public static void ModificationPatient(Patient oldPat, Patient newPat)
        {
            if (conn == null)
                context = new ReaPlanDBEntities();
            else
                context = new ReaPlanDBEntities(conn);

            Singleton singlePatient = Singleton.getInstance();
            string newDossier = "Files/Patients/" + newPat.Nom + newPat.Prenom + newPat.DateNaiss.ToShortDateString().ToString().Replace("/", string.Empty);
            string dossier = "Files/Patients/" + oldPat.Nom + oldPat.Prenom + oldPat.DateNaiss.ToShortDateString().ToString().Replace("/", string.Empty);

            if (dossier != newDossier)
            {
                if (Directory.Exists(dossier))
                {
                    System.IO.Directory.Move(dossier, newDossier);
                }
            }
                

            using (context)
            {

                PatientDB patient = (from c in context.PatientDBs
                                     where c.Nom == singlePatient.PatientSingleton.Nom
                                   && c.Prenom == singlePatient.PatientSingleton.Prenom
                                   && c.DateNaissance == singlePatient.PatientSingleton.DateNaiss.Date
                                     select c).FirstOrDefault();
                if (patient != null)
                {
                    patient.Nom = newPat.Nom;
                    patient.Prenom = newPat.Prenom;
                    patient.DateNaissance = newPat.DateNaiss;
                    patient.Poids = (decimal)newPat.Poid;
                    patient.Taille = newPat.Taille;
                    patient.Sexe = newPat.Sexe.ToString();
                    patient.Id1 = newPat.ID1;
                    patient.Id2 = newPat.ID2;
                    context.SaveChanges();
                }
            }
        }
        #endregion

        #region Ressort
        public static bool GetRessort()
        {
            Singleton singlePatient = Singleton.getInstance();
            string dossier = singlePatient.Patient.Nom + singlePatient.Patient.Prenom + singlePatient.Patient.DateDeNaissance.ToString().Replace("/", string.Empty);
            XDocument doc = XDocument.Load("Files/Patients/" + dossier + "/infoPatient.xml");
            return (bool)doc.Root.Element("Ressort");
        }
        #endregion
    }
}

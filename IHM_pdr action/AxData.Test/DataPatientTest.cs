using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using AxData;
using AxModel;
using Moq;
using System.Data.EntityClient;
using Effort;
using System.Data;
using AxViewModel;
using AxError;
namespace AxData.Test
{
    [TestFixture]
    public class DataPatientTest
    {
        private ReaPlanDBEntities context;
        DateTime dateNaissance;
        [SetUp]
        public void Init()
        {
            dateNaissance = DateTime.Now;
            EntityConnection connection = Effort.EntityConnectionFactory.CreateTransient("name=ReaPlanDBEntities");
            context = new ReaPlanDBEntities(connection);
            //creation d'un patient test dans la fausse BD
            PatientDB patient = PatientDB.CreatePatientDB("pat", "paul", dateNaissance, "M", (decimal)172, (decimal)66, 123, 26);
            context.AddToPatientDBs(patient);
            context.SaveChanges();

            PatientData.conn = connection;

            
        }

        //Tests sur l'inscription d'un patient

        [Test]
        [ExpectedException(typeof(ConstraintException))]
        public void Patient_Nom_Obligatoire()
        {
            Patient p = new Patient(null,"prenom",dateNaissance,"M",172,66,123,23);
            PatientData.InscriptionPatient(p);
        }
        [Test]
        [ExpectedException(typeof(ConstraintException))]
        public void Patient_Prenom_Obligatoire()
        {
            Patient p = new Patient("nom", null, dateNaissance, "M", 172, 66, 123, 23);
            PatientData.InscriptionPatient(p);
        }
        [Test]
        [ExpectedException(typeof(ConstraintException))]
        public void Patient_Sexe_Obligatoire()
        {
            Patient p = new Patient("nom", "prenom", dateNaissance, null, 172, 66, 123, 23);
            PatientData.InscriptionPatient(p);
        }
        [Test]
        [ExpectedException(typeof(ConstraintException))]
        public void Patient_DateNaissance_Obligatoire()
        {
            Patient p = new Patient();
            p.Nom = "nom";
            p.Prenom = "prenom";
            p.DateNaiss = DateTime.MaxValue;
            p.Sexe = "M";
            p.Taille = 0;
            p.Poid = 66;
            p.ID1 = 123;
            p.ID2 = 23;
            PatientData.InscriptionPatient(p);
        }
        [Test]
        [ExpectedException(typeof(ConstraintException))]
        public void Patient_Taille_Obligatoire()
        {
            Patient p = new Patient();
            p.Nom = "nom";
            p.Prenom = "prenom";
            p.Sexe = "M";
            p.Taille = 0;
            p.Poid = 66;
            p.ID1 = 123;
            p.ID2 = 23;
            PatientData.InscriptionPatient(p);
        }
        [Test]
        [ExpectedException(typeof(ConstraintException))]
        public void Patient_Poids_Obligatoire()
        {
            Patient p = new Patient();
            p.Nom = "nom";
            p.Prenom = "prenom";
            p.Sexe = "M";
            p.Taille = 172;
            p.Poid = 0;
            p.ID1 = 123;
            p.ID2 = 23;
            PatientData.InscriptionPatient(p);
        }
        [Test]
        [ExpectedException(typeof(ConstraintException))]
        public void Patient_ID1_Obligatoire()
        {
            Patient p = new Patient();
            p.Nom = "nom";
            p.Prenom = "prenom";
            p.Sexe = "M";
            p.Taille = 172;
            p.Poid = 66;
            p.ID1 = 0;
            p.ID2 = 23;
            PatientData.InscriptionPatient(p);
        }
        [Test]
        [ExpectedException(typeof(ConstraintException))]
        public void Patient_ID2_Obligatoire()
        {
            Patient p = new Patient();
            p.Nom = "nom";
            p.Prenom = "prenom";
            p.Sexe = "M";
            p.Taille = 172;
            p.Poid = 66;
            p.ID1 = 123;
            p.ID2 = 0;
            PatientData.InscriptionPatient(p);
        }

        //Test sur la suppression d'un patient

        [Test]
        public void Patient_Bien_Supprime()
        {
            PatientData.SupPatient("pat","paul",dateNaissance);
            Assert.IsNull(PatientData.RecherchePatient("pat", "paul", dateNaissance));
        }

        //Test sur la connexion d'un patient

        [Test]
        public void Bon_Nom_Patient_Trouve()
        {
            Assert.AreEqual("pat", PatientData.RecherchePatient("pat", "paul", dateNaissance).Nom);
        }
        [Test]
        public void Bon_Prenom_Patient_Trouve()
        {
            Assert.AreEqual("paul", PatientData.RecherchePatient("pat", "paul", dateNaissance).Prenom);
        }
        [Test]
        public void Bonne_Date_Patient_Trouve()
        {
            Assert.AreEqual(dateNaissance, PatientData.RecherchePatient("pat", "paul", dateNaissance).DateNaissance);
        }
    }
}

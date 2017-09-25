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
    public class DataTherapeuteTest
    {
        private ReaPlanDBEntities context;
        private string pass;
        private TherapeuteDB thera;
        [SetUp]
        public void Init()
        {
            EntityConnection connection = Effort.EntityConnectionFactory.CreateTransient("name=ReaPlanDBEntities");
            context = new ReaPlanDBEntities(connection);

            //creation d'un faux therapeute adminstrateur
            pass = "dSxLCjvQDN7t7ArMtZj8aSi68VopD/OL5Oc3S587mFVZOehubIGTHAzo4azslOrQ";// = admin
            thera = TherapeuteDB.CreateTherapeuteDB("pedro", "drope", "admin", pass);
            thera.Administrateur = true;
            context.AddToTherapeuteDBs(thera);

            //creation d'un faux therapeute non adminstrateur
            pass = "35YbbsmJTwvM0Eonh3qLT5cv6DKi5k3ARspeWfiiVB9X8mmjjFsJeTGqVI5ejzEz";// = 123456
            thera = TherapeuteDB.CreateTherapeuteDB("jacko", "michel", "jackmich", pass);
            thera.Administrateur = false;
            context.AddToTherapeuteDBs(thera);

            context.SaveChanges();
            AdminData.conn = connection;
        }

        //Tests sur la connexion d'un thérapeute

        [Test]
        public void Therapeute_Connection_Login_Bon_Mdp_Bon()
        {
            Assert.IsNotNull(AdminData.Connexion("admin", "admin"));
        }
        [Test]
        public void Therapeute_Connection_Login_Faux_Mdp_Bon()
        {
            Assert.IsNull(AdminData.Connexion("admon", "admin"));
        }
        public void Therapeute_Connection_Login_Bon_Mdp_Faux()
        {
            Assert.IsNull(AdminData.Connexion("admin", "12345"));
        }
        public void Therapeute_Connection_Login_Faux_Mdp_Faux()
        {
            Assert.IsNull(AdminData.Connexion("admon", "12345"));
        }
        [Test]
        public void Therapeute_Login_Mdp_Doivent_Etre_Rempli()
        {
            Assert.IsNull(AdminData.Connexion("", ""));
        }
        //Si la méthode IsAdministrateur return true le therapeute est vraiment admin et peut acceder a l'inscription
        //Sinon non
        [Test]
        public void Therapeute_Doit_Etre_Admin()
        {
            Assert.IsTrue(AdminData.IsAdministrateur("admin", "admin"));
        }
        [Test]
        public void Therapeute_Ne_Doit_Pas_Etre_Admin()
        {
            Assert.IsFalse(AdminData.IsAdministrateur("jackmich", "123456"));
        }

        //Si la methode AdminInBd return true il y a bien un admin dans la bd
        [Test]
        public void Admin_Dans_La_Bd()
        {
            Assert.IsTrue(AdminData.AdminInBd());
        }
        [Test]
        public void Admin_Pas_Dans_La_Bd()
        {
            var requete = from c in context.TherapeuteDBs
                          where c.Administrateur == true
                          select c;
            var admin = requete.FirstOrDefault();
            context.DeleteObject(admin);
            context.SaveChanges();
            Assert.IsFalse(AdminData.AdminInBd());
        }

        //Test sur l'enregistrement d'un administrateur
        [Test]
        public void Inscription_Administrateur()
        {
            AdminData.InscriptionTherapeute("Monsieur", "Michel", "michadmin", "12345", true);
            var requete = from c in context.TherapeuteDBs
                          where c.Administrateur == true &&
                          c.Login == "michadmin"
                          select c;
            var admin = requete.FirstOrDefault();
            Assert.IsNotNull(admin);
        }

        //Tests sur l'inscription d'un thérapeute

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void Therapeute_Nom_Obligatoire()
        {
            AdminData.InscriptionTherapeute(null, "Michel", "mich", "12345", false);
        }
        [Test]
        [ExpectedException(typeof(ConstraintException))]
        public void Therapeute_Prenom_Obligatoire()
        {
            AdminData.InscriptionTherapeute("Denis", null, "mich", "12345",false);
        }
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void Therapeute_Nom_Utilisateur_Obligatoire()
        {
            AdminData.InscriptionTherapeute("Denis", "Michel", null, "12345", false);
        }
        [Test]
        [ExpectedException(typeof(ConstraintException))]
        public void Therapeute_Mot_De_Passe_Obligatoire()
        {
            AdminData.InscriptionTherapeute("Denis", "Michel", "mich", null, false);
        }

        //Tests sur le IDataErrorInfo
        [Test]
        public void Therapeute_Nom_Valide()
        {
            Assert.IsNotEmpty(ValidationData.ValidationNomPrenom("Deni236s",0));
        }
        [Test]
        public void Therapeute_Prenom_Valide()
        {
            Assert.IsNotEmpty(ValidationData.ValidationNomPrenom("Mich5899", 1));
        }
        [Test]
        public void Therapeute_Nom_Utilisateur_Longeur_Valide()
        {
            Assert.IsNotEmpty(ValidationData.ValidationNomUtilisateur(""));
        }
        [Test]
        public void Therapeute_Mot_De_Passe_Longeur_Valide()
        {
            Assert.IsNotEmpty(ValidationData.ValidationMdp("1234"));
        }
        [Test]
        public void Therapeute_Confirmation_Mot_De_Passe()
        {
            Assert.IsNotEmpty(ValidationData.ValidationMdpConfirm("123456", "abcdef"));
        }
        //Verifie que les deux mdp doivent être identique
        [Test]
        public void Therapeute_Confirmation_Mot_De_Passe_Valide()
        {
            Assert.IsEmpty(ValidationData.ValidationMdpConfirm("123456","123456"));
        }
    }
}

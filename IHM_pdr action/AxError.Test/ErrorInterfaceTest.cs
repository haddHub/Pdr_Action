using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxError;
using AxModel;
using NUnit.Framework;

namespace AxError.Test
{
    [TestFixture]
    public class ErrorInterfaceTest
    {
        string motValide, motVide, motChiffre;
        double poidsValide, poidsVide, poidsTropPetit, poidsTropGrand;
        int tailleValide, tailleVide, tailleTropPetite, tailleTropGrande;

        [SetUp]
        public void InitTest()
        {
            motValide = "Michel";
            motChiffre = "Patrick1998";
            motVide = "";
            poidsValide = 69.6;
            poidsTropPetit = 1;
            poidsTropGrand = 856;
            tailleValide = 185;
            tailleTropPetite = 12;
            tailleTropGrande = 965;
        }

        #region Test sur le prénom
        [Test]
        public void Patient_Prenom_Est_Valide()
        {
            Assert.IsEmpty(ValidationData.ValidationNomPrenom(motValide, 0));
        }
        [Test]
        public void Patient_Prenom_Avec_Chiffre_Non_Valdie()
        {
            Assert.IsNotEmpty(ValidationData.ValidationNomPrenom(motVide, 0));
        }
        #endregion
        #region Test sur le nom
        [Test]
        public void Nom_Est_Valide()
        {
            Assert.IsEmpty(ValidationData.ValidationNomPrenom(motValide, 1));
        }
        [Test]
        public void Nom_Avec_Chiffre_Non_Valdie()
        {
            Assert.IsNotEmpty(ValidationData.ValidationNomPrenom(motVide, 1));
        }
        #endregion
        #region Test sur le poids
        [Test]
        public void Poids_Est_Valide()
        {
            Assert.IsEmpty(ValidationData.ValidationPoids(poidsValide));
        }
        [Test]
        public void Poids_Trop_Petit()
        {
            Assert.IsNotEmpty(ValidationData.ValidationPoids(poidsTropPetit));
        }
        [Test]
        public void Poids_Trop_Grand()
        {
            Assert.IsNotEmpty(ValidationData.ValidationPoids(poidsTropGrand));
        }
        #endregion
        #region Test sur la taille
        [Test]
        public void Taille_Est_Valide()
        {
            Assert.IsEmpty(ValidationData.ValidationTaille(tailleValide));
        }
        [Test]
        public void Taille_Trop_Petite()
        {
            Assert.IsNotEmpty(ValidationData.ValidationTaille(tailleTropPetite));
        }
        [Test]
        public void Taille_Trop_Grande()
        {
            Assert.IsNotEmpty(ValidationData.ValidationTaille(tailleTropGrande));
        }
        #endregion
        #region Test Sur le registre national
        [Test]
        public void Digit_Avant_Janvier_2000_Correct()
        {
            DateTime date = new DateTime(1991, 03, 22);
            string digit = "63";
            string cptNaiss = "433";
            string idate = "910322";
            Assert.IsEmpty(ValidationData.ValidationIdNationalDigit(digit, date, cptNaiss, idate));
        }
        [Test]
        public void Digit_Avant_Janvier_2000_Mauvais()
        {
            DateTime date = new DateTime(1991, 03, 22);
            string digit = "26";
            string cptNaiss = "433";
            string idate = "910322";
            Assert.IsNotEmpty(ValidationData.ValidationIdNationalDigit(digit, date, cptNaiss, idate));
        }
        [Test]
        public void Digit_Apres_Decembre_1999_Mauvais()
        {
            DateTime date = new DateTime(2005, 05, 09);
            string digit = "26";
            string cptNaiss = "185";
            string idate = "050509";
            Assert.IsNotEmpty(ValidationData.ValidationIdNationalDigit(digit, date, cptNaiss, idate));
        }
        [Test]
        public void Digit_Apres_Decembre_1999_Correct()
        {
            DateTime date = new DateTime(2005, 05, 09);
            string digit = "5";
            string cptNaiss = "185";
            string idate = "050509";
            Assert.IsEmpty(ValidationData.ValidationIdNationalDigit(digit, date, cptNaiss, idate));
        }
        #endregion
    }
}

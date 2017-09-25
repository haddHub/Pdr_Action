using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxModel;
namespace AxError
{
    public static class ValidationData
    {
        private static string errorMessage = "";
        private static DateTime dateAnterieur = new DateTime(2000,01,01);
        private static DateTime dateUlterieur = new DateTime(1999,12,31);
        public static string ValidationNomPrenom(string nomPre,int i)
        {
            if (string.IsNullOrEmpty(nomPre))//test si le champ est vide
            {
                if(i == 0)
                    errorMessage = AxLanguage.Languages.REAplan_Validation_Nom;
                else
                    errorMessage = AxLanguage.Languages.REAplan_Validation_Prenom;
            }
            else
            {
                bool isNumeric = nomPre.Any(c => char.IsDigit(c));//test si il y a des nombres
                if (isNumeric == true)
                {
                    if(i == 0)
                        errorMessage = AxLanguage.Languages.REAplan_Validation_Nom_Chiffre;
                    else
                        errorMessage = AxLanguage.Languages.REAplan_Validation_Prenom_Chiffre;
                }
                else
                {
                    if (nomPre.Length > 100 || nomPre.Length < 1)
                        errorMessage = AxLanguage.Languages.REAplan_Validation_Nom_Taille;
                    else
                        errorMessage = "";
                }
            }
            return errorMessage;
        }

        public static string ValidationNomUtilisateur(string nomUti)
        {
            if (string.IsNullOrEmpty(nomUti))//test si le champ est vide
                errorMessage = AxLanguage.Languages.REAplan_Validation_Nom_Utilisateur;
            else
            {
                if (nomUti.Length > 100 || nomUti.Length < 1)
                    errorMessage = AxLanguage.Languages.REAplan_Validation_Nom_Taille;
                else
                    errorMessage = "";
            }
            return errorMessage;
        }

        public static string ValidationMdp(string mdp)
        {
            if (string.IsNullOrEmpty(mdp))//test si le champ est vide
                errorMessage = AxLanguage.Languages.REAplan_Validation_Mot_De_Passe;
            else
            {
                if (mdp.Length > 20 || mdp.Length < 5)
                    errorMessage = AxLanguage.Languages.REAplan_Validation_Mot_De_Passe_Taille;
                else
                        errorMessage = "";
            }
            return errorMessage;
        }
        public static string ValidationMdpConfirm(string mdp, string mdpConfirm)
        {
            if (string.IsNullOrEmpty(mdpConfirm))//test si le champ est vide
                errorMessage = AxLanguage.Languages.REAplan_Validation_Mot_De_Passe;
            else
            {
                if (mdpConfirm.Length > 20 || mdpConfirm.Length < 5)
                    errorMessage = AxLanguage.Languages.REAplan_Validation_Mot_De_Passe_Taille;
                else
                {
                    if (!mdp.Equals(mdpConfirm))
                        errorMessage = AxLanguage.Languages.REAplan_Validation_Mot_De_Passe_Confirmation;
                    else
                        errorMessage = "";
                }
            }
            return errorMessage;
        }
        public static string ValidationTaille(int taille)
        {
            if (string.IsNullOrEmpty(taille.ToString()))
                errorMessage = AxLanguage.Languages.REAplan_Validation_Taille;
            else
            {
                if (IsChar(taille.ToString()) == true)
                {
                    errorMessage = AxLanguage.Languages.REAplan_Validation_Taille_Lettre;
                }
                else
                {
                    if (taille < 0)
                        errorMessage = AxLanguage.Languages.REAplan_Validation_Taille_Negative;
                    else
                    {
                        if (taille < 50 ||taille > 300)
                            errorMessage = AxLanguage.Languages.REAplan_Validation_Taille_Plausible;
                        else
                            errorMessage = "";
                    }
                }
            }
            return errorMessage;
        }

        public static string ValidationPoids(double poids)
        {
            if (string.IsNullOrEmpty(poids.ToString()))
                errorMessage = AxLanguage.Languages.REAplan_Validation_Poids;
            else
            {
                if (IsChar(poids.ToString()) == true)
                {
                    errorMessage = AxLanguage.Languages.REAplan_Validation_Poids_Lettre;
                }
                else
                {
                    if (poids < 0)
                        errorMessage = AxLanguage.Languages.REAplan_Validation_Poids_Negatif;
                    else
                    {
                        if (poids < 10 || poids > 400)
                            errorMessage = AxLanguage.Languages.REAplan_Validation_Poids_Plausible;
                        else
                            errorMessage = "";
                    }
                }
            }
            return errorMessage;
        }

        public static string ValidationIdNationalCompteurNaissance(string cptNaiss, bool m,bool f)
        {
            if (string.IsNullOrEmpty(cptNaiss.ToString()))
                errorMessage = AxLanguage.Languages.REAplan_Validation_IdNat;
            else
            {
                if (IsChar(cptNaiss.ToString()) == true)
                {
                    errorMessage = AxLanguage.Languages.REAplan_Validation_IdNat_Lettre;
                }
                else
                {
                    if (Convert.ToInt32(cptNaiss) < 0)
                        errorMessage = AxLanguage.Languages.REAplan_Validation_IdNat_Negatif;
                    else
                    {
                        if (cptNaiss.ToString().Length < 3)
                            errorMessage = AxLanguage.Languages.REAplan_Validation_IdNat_Chiffre;
                        else
                        {
                            if (((Convert.ToInt32(cptNaiss) % 2) == 0) && f == false)//est pair
                                errorMessage = AxLanguage.Languages.REAplan_Validation_IdNat_Femme;
                            else if (((Convert.ToInt32(cptNaiss) % 2) != 0) && m == false)//est impair
                                errorMessage = AxLanguage.Languages.REAplan_Validation_IdNat_Homme;
                            else
                                errorMessage = "";
                        }
                    }
                }
            }
            return errorMessage;
        }

        public static string ValidationIdNationalDigit(string digit,DateTime dateNaissance, string cptNaiss,string idDate)
        {
            if (string.IsNullOrEmpty(digit.ToString()))
                errorMessage = AxLanguage.Languages.REAplan_Validation_IdNat;
            else
            {
                if (IsChar(digit.ToString()) == true)
                {
                    errorMessage = AxLanguage.Languages.REAplan_Validation_IdNat_Lettre;
                }
                else
                {
                    if (Convert.ToInt32(digit) < 0)
                        errorMessage = AxLanguage.Languages.REAplan_Validation_IdNat_Negatif;
                    else
                    {
                        if(dateNaissance.CompareTo(dateAnterieur) < 0)//avant 01/01/2000
                        {
                            int digitTest = 97 - ((Convert.ToInt32(idDate + "" + cptNaiss)) % 97);
                            if (Convert.ToInt32(digit) != digitTest)
                                errorMessage = AxLanguage.Languages.REAplan_Validation_IdNat_Digit;
                            else
                                errorMessage = "";
                        }
                        else if (dateNaissance.CompareTo(dateUlterieur) > 0)//apres 31/12/1999
                        {
                            int digitTest = 97 - ((Convert.ToInt32("2"+idDate + "" + cptNaiss)) % 97);
                            if (Convert.ToInt32(digit) != digitTest)
                                errorMessage = AxLanguage.Languages.REAplan_Validation_IdNat_Digit;
                            else
                                errorMessage = "";
                        }
                        else
                            errorMessage = "";
                    }
                }
            }
            return errorMessage;
        }

        private static bool IsChar(string str)
        {
            bool isChar = str.Any(i => char.IsLetter(i));//test si il y a des lettres dans le str
            return isChar;
        }

        public static bool IsPatientValid(Patient p)
        {
            bool isValid = true;
            bool m = false; 
            bool f = false;
            if (p.Sexe == "m")
                m = true;
            else
                f = true;
           
            string idDate = ConvertirDateToIdDate(p.DateNaiss);
            if (ValidationNomPrenom(p.Nom, 0) != "")
                isValid = false;
            else if (ValidationNomPrenom(p.Prenom, 1) != "")
                isValid = false;
            else if (ValidationPoids(p.Poid) != "")
                isValid = false;
            else if (ValidationTaille(p.Taille) != "")
                isValid = false;
            return isValid;
        }
        public static bool IsTherapeuteValid(Therapeute t)
        {
            bool isValid = true;

            if (ValidationNomPrenom(t.Nom, 0) != "")
                isValid = false;
            else if (ValidationNomPrenom(t.Prenom, 1) != "")
                isValid = false;
            else if (ValidationNomUtilisateur(t.NomUtilisateur) != "")
                isValid = false;
            else if (ValidationMdp(t.Mdp) != "")
                isValid = false;
            else if (ValidationMdpConfirm(t.Mdp,t.MdpConfirm) != "")
                isValid = false;
            return isValid;
        }
        private static string ConvertirDateToIdDate(DateTime DateNaiss)
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
    }
}

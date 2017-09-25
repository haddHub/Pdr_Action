using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Data.EntityClient;
using AxModel;

namespace AxData
{
    public static class AdminData
    {
        public static EntityConnection conn;
        private static ReaPlanDBEntities bdd;
        public static TherapeuteDB Connexion(string login,string pass)
        {
            //permet au TU de donner la fausse BD
            if (conn == null)
            {
                bdd = new ReaPlanDBEntities();
                UiServices.SetBusyState();
            }
            else
                bdd = new ReaPlanDBEntities(conn);

            //Cryptage du mot de passe entré pour le comparer avec le mdp crypté de la bd
            if (pass != null && login != null)
            {
                string password = login.ToLower() + pass;
                UTF8Encoding textConverter = new UTF8Encoding();
                byte[] passBytes = textConverter.GetBytes(password);
                pass = Convert.ToBase64String(new SHA384Managed().ComputeHash(passBytes));
            }

            using (bdd)
            {
                var requete = from c in bdd.TherapeuteDBs
                              where c.Login == login
                              && c.MotDePasse == pass
                              select c;
                TherapeuteDB therapeute = requete.FirstOrDefault();
                return therapeute;
            }
        }

        public static void InscriptionTherapeute(string nom,string prenom,string login,string pass,bool isAdmin)
        {
            //permet au TU de donner la fausse BD
            if (conn == null)
                bdd = new ReaPlanDBEntities();
            else
                bdd = new ReaPlanDBEntities(conn);

            nom = nom.ToUpper();
            using (bdd)
            {

                //Cryptage du mot de passe
                if (pass !=null)
                {
                    string passwordTmp = login.ToLower() + pass;
                    UTF8Encoding textConverter = new UTF8Encoding();
                    byte[] passBytes = textConverter.GetBytes(passwordTmp);
                    pass = Convert.ToBase64String(new SHA384Managed().ComputeHash(passBytes)); 
                }

                TherapeuteDB thera = TherapeuteDB.CreateTherapeuteDB(nom, prenom, login, pass);
                if (isAdmin == true)
                    thera.Administrateur = true;
                bdd.AddToTherapeuteDBs(thera);
                bdd.SaveChanges();
            }
        }
        public static bool IsAdministrateur(string login, string pass)
        {
            TherapeuteDB thera = Connexion(login, pass);
            if (thera != null)
            {
                return thera.Administrateur;
            }
            else
                return false;
        }
        //Permet de savoir si un admin est présent dans la bd
        public static bool AdminInBd()
        {
            //permet au TU de donner la fausse BD
            if (conn == null)
                bdd = new ReaPlanDBEntities();
            else
                bdd = new ReaPlanDBEntities(conn);

            using (bdd)
            {
                var requete = from c in bdd.TherapeuteDBs
                              where c.Administrateur == true
                              select c;
                TherapeuteDB admin = requete.FirstOrDefault();
                if (admin == null)
                    return false;
                else
                    return true;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class Admin
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string NomUtilisateur { get; set; }
        public string MotDePasse { get; set; }

        public Admin(string n, string p, string nomUti, string mdp)
        {
            Nom = n;
            Prenom = p;
            NomUtilisateur = nomUti;
            MotDePasse = mdp;
        }

        public override string ToString()
        {
            return Nom.Trim() + " " + Prenom.Trim();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class Therapeute
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Mdp { get; set; }
        public string MdpConfirm { get; set; }
        public string NomUtilisateur { get; set; }

        public Therapeute(string nom, string prenom, string nomUti, string mdp)
        {
            Nom = nom;
            Prenom = prenom;
            NomUtilisateur = nomUti;
            Mdp = mdp;
            MdpConfirm = "";
        }
    }
}

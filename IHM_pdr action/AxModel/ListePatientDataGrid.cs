using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class ListePatientDataGrid
    {
        public String Nom { get; set; }
        public String Prenom { get; set; }
        public String DateDeNaissance { get; set;}

        public ListePatientDataGrid()
        {

        }

        public ListePatientDataGrid(string nom, string prenom, string daten)
        {
            Nom = nom;
            Prenom = prenom;
            DateDeNaissance = daten;
        }
    }
}

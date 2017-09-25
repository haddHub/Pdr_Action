using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class ThemeEvaluationModel
    {
        #region Proprietes
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _cible;

        public string Cible
        {
            get { return _cible; }
            set { _cible = value; }
        }
        private string _chasseur;

        public string Chasseur
        {
            get { return _chasseur; }
            set { _chasseur = value; }
        }
        private string _chasseur2;

        public string Chasseur2
        {
            get { return _chasseur2; }
            set { _chasseur2 = value; }
        }

        private string _fond;

        public string Fond
        {
            get { return _fond; }
            set { _fond = value; }
        } 
        #endregion

        public ThemeEvaluationModel(string nom,string cible,string chasseur,string fond)
        {
            Name = nom;
            Cible = cible;
            Chasseur = chasseur;
            Fond = fond;
        }
    }
}

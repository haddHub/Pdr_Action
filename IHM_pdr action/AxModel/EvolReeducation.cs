using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
namespace AxModel
{
    public class EvolReeducation
    {
        public string Dossier { get; set; }
        public List<XElement> ListeDate { get; set; }

        public EvolReeducation(string dossier)
        {
            Dossier = dossier;
        }

        private List<XElement> Exercice(XDocument file)
        {
            var recherche =
                (from el in file.Root.Elements("Ex")
                 select el).ToList();
            return recherche;
        }
    }
}

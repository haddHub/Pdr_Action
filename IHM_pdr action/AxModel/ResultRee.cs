using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows;

namespace AxModel
{
    public class ResultRee
    {
        public string dateJour = String.Format("{0:00}", DateTime.Now.Day) + String.Format("{0:00}", DateTime.Now.Month) + DateTime.Now.Year;
        XDocument fileResult;
        public string Dossier { get; set; }
        public ResultRee(string dossier)
        {
            fileResult = XDocument.Load(@"../../Files/Patients/" + dossier + "/Reeducation/fiTemps.xml");
            Dossier = dossier;
        }

        private List<XElement> Exercice()
        {
            var recherche =
                (from el in fileResult.Root.Elements("Ex")
                 select el).ToList();
            return recherche;
        }

        public List<PointsDSReed> returnTemp()
        {
            List<PointsDSReed> value = new List<PointsDSReed>();

            List<XElement> rechExo = Exercice();
            foreach (var el in rechExo)
            {
                double p;
                p = ConvertDouble(el.Element("Temps"));
                PointsDSReed point = new PointsDSReed(p, 0);
                value.Add(point);
            }
            return value;
        }

        public List<double> returnMouv()
        {
            List<double> value = new List<double>();

            List<XElement> rechExo = Exercice();
            foreach (var el in rechExo)
            {
                double p;
                p = ConvertDouble(el.Element("Cible"));
                value.Add(p);
            }
            return value;
        }

        public List<PointsDSReed> returnInit()
        {
            List<PointsDSReed> value = new List<PointsDSReed>();

            List<XElement> rechExo = Exercice();
            foreach (var el in rechExo)
            {
                XElement Initial = el.Element("Initial");
                List<XElement> ListVal = (from el2 in Initial.Elements("Valeur")
                                          select el2).ToList();
                int i = 0;
                double moy = 0;
                foreach (var el3 in ListVal)
                {
                    moy += ConvertDouble(el3);
                    i++;
                }
                moy /= i;
                double somme = 0;
                foreach (var el3 in ListVal)
                {
                    somme += Math.Pow((ConvertDouble(el3) - moy), 2);
                }
                double dvStand = Math.Sqrt(somme / i);
                PointsDSReed p = new PointsDSReed(moy, dvStand);
                value.Add(p);
            }
            return value;
        }

        public List<PointsDSReed> returnAssistLat()
        {
            List<PointsDSReed> value = new List<PointsDSReed>();

            List<XElement> rechExo = Exercice();
            foreach (var el in rechExo)
            {
                XElement Initial = el.Element("AssistLat");
                List<XElement> ListVal = (from el2 in Initial.Elements("Valeur")
                                          select el2).ToList();
                int i = 0;
                double moy = 0;
                foreach (var el3 in ListVal)
                {
                    moy += ConvertDouble(el3);
                    i++;
                }
                moy /= i;
                double somme = 0;
                foreach (var el3 in ListVal)
                {
                    somme += Math.Pow((ConvertDouble(el3) - moy), 2);
                }
                double dvStand = Math.Sqrt(somme / i);
                PointsDSReed p = new PointsDSReed(moy, dvStand);
                value.Add(p);
            }
            return value;
        }

        public List<PointsDSReed> returnAssistLong()
        {
            List<PointsDSReed> value = new List<PointsDSReed>();

            List<XElement> rechExo = Exercice();
            foreach (var el in rechExo)
            {
                XElement Initial = el.Element("AssistLong");
                List<XElement> ListVal = (from el2 in Initial.Elements("Valeur")
                                          select el2).ToList();
                int i = 0;
                double moy = 0;
                foreach (var el3 in ListVal)
                {
                    moy += ConvertDouble(el3);
                    i++;
                }
                moy /= i;
                double somme = 0;
                foreach (var el3 in ListVal)
                {
                    somme += Math.Pow((ConvertDouble(el3) - moy), 2);
                }
                double dvStand = Math.Sqrt(somme / i);
                PointsDSReed p = new PointsDSReed(moy, dvStand);
                value.Add(p);
            }
            return value;
        }

        private static double ConvertDouble(XElement el)
        {
            int j = 0;
            for (int ind = 0; (ind < el.Value.Count() - 1 && el.Value.Substring(ind, 1) != ",") || (ind < el.Value.Count() - 1 && el.Value.Substring(ind, 1) != "."); ind++)
            {
                j++;
            }
            double cyclerecup = 0.0;
            if (j != el.Value.Count() - 1)
            {
                cyclerecup = int.Parse(el.Value.Substring(0, j));
                int lgdeci = el.Value.Substring(j + 1, el.Value.Count() - (j + 1)).Count();
                double deci = double.Parse(el.Value.Substring(j + 1, el.Value.Count() - (j + 1))) / Math.Pow(10, lgdeci);
                cyclerecup += deci;
            }
            else
            {
                cyclerecup = double.Parse(el.Value);
            }
            return cyclerecup;
        }
    }
}
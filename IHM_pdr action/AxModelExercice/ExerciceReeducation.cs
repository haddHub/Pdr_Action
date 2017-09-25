using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxModel;
using System.IO;
using System.Diagnostics;

namespace AxModelExercice
{
    public class ExerciceReeducation : ExerciceGeneric
    {
        public string NomExercice {get; set;}
        public string ImageDifficulte { get; set; }  
        public byte Niveau { get; set; }
        public int Score { get; set; }
        public ThemeModel Theme { get; set; }

        //proprietes pour l'enregistrement d'une reeducation
        public double NbrMouvement { get; set; }
        public double RaideurLatMoyenne { get; set; }
        public double CVRaideurLat { get; set; }
        public double RaideurLongMoyenne { get; set; }
        public double CVRaideurLong { get; set; }
        public double InitMoyen { get; set; }
        public double CVInit { get; set; }
        public double VitesseMoyenne { get; set; }
        public double CVVitesseMoyenne { get; set; }

        private List<double> listeKlat;
        private List<double> listeKlong;
        private List<double> listeInit;
        private List<double> listeVitesse;

        public ExerciceReeducation(ExerciceBaseConfig baseConf, ExerciceBorneConfig borneConf, ThemeModel theme)
            : base(baseConf,borneConf)
        {
            Theme = theme;
            this.TypeExercice = ExerciceTypes.Jeu;
        }

        public ExerciceReeducation()
        {

        }

        public void SaveMouvement(string dossier)
        {
            string pathString;
            string pathStringMouvComplex;
            CreerPathFichier(dossier, out pathString, out pathStringMouvComplex);

            if (!System.IO.File.Exists(pathStringMouvComplex))
            {
                System.IO.Directory.CreateDirectory(pathString);
                using (System.IO.FileStream fs = System.IO.File.Create(pathStringMouvComplex))
                {
                    fs.Close();
                }
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathStringMouvComplex, true))
            {
                file.WriteLine(DataItem());
                file.Close();
            }
        }

        private static void CreerPathFichier(string dossier, out string pathString, out string pathStringMouvComplex)
        {
            string dateJour = String.Format("{0:00}", DateTime.Now.Day) + "_" + String.Format("{0:00}", DateTime.Now.Month) + "_" + DateTime.Now.Year;
            pathString = Directory.GetCurrentDirectory() + "\\Files\\Patients\\" + dossier + "\\Rééducation\\Données brutes\\Mouvements complexes";
            string fileNameMouvComplex = dateJour + ".txt";
            pathStringMouvComplex = System.IO.Path.Combine(pathString, fileNameMouvComplex);
        }

        private string DataItem()
        {
            string data;
            data = String.Format("{0,0:N2} {1,0:N2} {2,0:N2} {3,0:N2}", RaideurLatMoyenne, RaideurLongMoyenne, VitesseMoyenne, InitMoyen);
            data = data.ToString().Replace(".", string.Empty);
            return data.ToString().Replace(',', '.');
        }

        public void CalculerMoyenneEtCV(string dossier)
        {
            string pathString;
            string pathStringMouvComplex;
            listeInit = new List<double>();
            listeKlat = new List<double>();
            listeKlong = new List<double>();
            listeVitesse = new List<double>();

            CreerPathFichier(dossier, out pathString, out pathStringMouvComplex);
            // si le fichier existe
            if (System.IO.File.Exists(pathStringMouvComplex))
            {
                var allLines = System.IO.File.ReadAllLines(pathStringMouvComplex);
                foreach (string line in allLines)
                {
                    //une ligne = klat klong vitesse init
                    string l = line.Replace('.',',');
                    string[] split = l.Split(' ');
                    listeKlat.Add(Convert.ToDouble(split[0]));
                    listeKlong.Add(Convert.ToDouble(split[1]));
                    listeVitesse.Add(Convert.ToDouble(split[2]));
                    listeInit.Add(Convert.ToDouble(split[3]));
                    NbrMouvement++;
                }
                CalculerMoyenne();
                CalculerCV();
            }
        }
        private void CalculerMoyenne()
        {
            RaideurLatMoyenne = Moyenne(listeKlat);
            RaideurLongMoyenne = Moyenne(listeKlong);
            VitesseMoyenne = Moyenne(listeVitesse);
            InitMoyen = Moyenne(listeInit);
        }
        private double Moyenne(List<double> liste)
        {
            double moy = 0.0;
            for (int i = 0; i < liste.Count(); i++)
            {
                moy += liste[i];
            }
            moy /= liste.Count();
            return moy;
        }
        private void CalculerCV()
        {
            CVRaideurLat = (EcartType(listeKlat) / RaideurLatMoyenne)*100;
            CVRaideurLong = (EcartType(listeKlong) / RaideurLongMoyenne)*100;
            CVVitesseMoyenne = (EcartType(listeVitesse) / VitesseMoyenne)*100;
            CVInit = (EcartType(listeInit) / InitMoyen) * 100;
        }
        private double EcartType(List<double> liste)
        {
            double moyenne = Moyenne(liste);
            double somme = 0.0;
            for (int i = 0; i < liste.Count(); i++)
            {
                double delta = liste[i] - moyenne;
                somme += delta * delta;
            }
            return Math.Sqrt(somme / (liste.Count() - 1));
        }
    }
}

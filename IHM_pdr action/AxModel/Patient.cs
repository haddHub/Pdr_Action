using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.IO;
using GalaSoft.MvvmLight.Messaging;
using System.Data;

namespace AxModel
{
    public class Patient
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public DateTime DateNaiss { get; set; }
        public string Sexe { get; set; }
        public int Taille { get; set; }
        public double Poid { get; set; }
        public int Bras { get; set; }
        public string NomDoss { get; set; }
        public string Dossier1 { get; set; }
        public string Dossier2 { get; set; }
        public string Nom1 { get; set; }
        public string Nom2 { get; set; }
        public string Prenom1 { get; set; }
        public string Prenom2 { get; set; }
        public string Date1 { get; set; }
        public string Date2 { get; set; }
        public int ID1 { get; set; }
        public int ID2 { get; set; }
        public ListePatientDataGrid PatSup { get; set; }

        public Patient(string nom, string prenom, DateTime dateNaiss, string sexe, int taille, double poid)
        {
            Nom = nom;
            Prenom = prenom;
            DateNaiss = dateNaiss;
            Sexe = sexe;
            Taille = taille;
            Poid = poid;
        }

        public Patient(string nom, string prenom, DateTime dateNaiss, string sexe, int taille, double poid, int id1, int id2)
        {
            Nom = nom;
            Prenom = prenom;
            DateNaiss = dateNaiss;
            Sexe = sexe;
            Taille = taille;
            Poid = poid;
            ID1 = id1;
            ID2 = id2;
        }

        public Patient(string nom, string prenom)
        {
            Nom = nom;
            Prenom = prenom;
        }

        public Patient(ListePatientDataGrid patSup)
        {
            PatSup = patSup;
        }


        public Patient(string dossier)
        {
            NomDoss = dossier;
        }

        public Patient(string dossier1, string dossier2, string nom1, string nom2, string prenom1, string prenom2, string date1, string date2)
        {
            Dossier1 = dossier1;
            Dossier2 = dossier2;
            Nom1 = nom1;
            Nom2 = nom2;
            Prenom1 = prenom1;
            Prenom2 = prenom2;
            Date1 = date1;
            Date2 = date2;
        }

        public Patient(string dossier, int taille, double poid, int bras, string sexe)
        {
            NomDoss = dossier;
            Taille = taille;
            Poid = poid;
            Bras = bras;
            Sexe = sexe;
        }

        public Patient(string nom,string prenom,int taille,double poids)
        {
            Nom = nom;
            Prenom = prenom;
            Taille = taille;
            Poid = poids;
        }

        //ctor pour les tests uniatres
        public Patient()
        {
            if(Taille == 0)
                throw new ConstraintException();
            else if (Poid == 0)
                throw new ConstraintException();
            else if (ID1 == 0)
                throw new ConstraintException();
            else if (ID2 ==0)
                throw new ConstraintException();
            else if (DateNaiss == DateTime.MaxValue)
                throw new ConstraintException();
        }

        public void CreeDossier()
        {
            string activeDir = @"../../Files/Patients";
            string newPath = System.IO.Path.Combine(activeDir, Nom + "_" + Prenom + "_" + DateNaiss.Day + "_" + DateNaiss.Month + "_" + DateNaiss.Year);
            System.IO.Directory.CreateDirectory(newPath);
            activeDir = @"../../Files/Patients/" + Nom + "_" + Prenom + "_" + DateNaiss.Day + "_" + DateNaiss.Month + "_" + DateNaiss.Year;
            newPath = System.IO.Path.Combine(activeDir, "Evaluation");
            System.IO.Directory.CreateDirectory(newPath);
            newPath = System.IO.Path.Combine(activeDir, "Rééducation");
            System.IO.Directory.CreateDirectory(newPath);
            activeDir = @"../../Files/Patients/" + Nom + "_" + Prenom + "_" + DateNaiss.Day + "_" + DateNaiss.Month + "_" + DateNaiss.Year + "/Evaluation";
            newPath = System.IO.Path.Combine(activeDir, "Résultats");
            System.IO.Directory.CreateDirectory(newPath);
            newPath = System.IO.Path.Combine(activeDir, "Données brutes");
            System.IO.Directory.CreateDirectory(newPath);
            activeDir = @"../../Files/Patients/" + Nom + "_" + Prenom + "_" + DateNaiss.Day + "_" + DateNaiss.Month + "_" + DateNaiss.Year + "/Evaluation/Données brutes";
            newPath = System.IO.Path.Combine(activeDir, "Grande amplitude");
            System.IO.Directory.CreateDirectory(newPath);
            newPath = System.IO.Path.Combine(activeDir, "Cible");
            System.IO.Directory.CreateDirectory(newPath);
            newPath = System.IO.Path.Combine(activeDir, "Cercle");
            System.IO.Directory.CreateDirectory(newPath);
            newPath = System.IO.Path.Combine(activeDir, "Carré");
            System.IO.Directory.CreateDirectory(newPath);
            activeDir = @"../../Files/Patients/" + Nom + "_" + Prenom + "_" + DateNaiss.Day + "_" + DateNaiss.Month + "_" + DateNaiss.Year + "/Rééducation";
            newPath = System.IO.Path.Combine(activeDir, "Résultats");
            System.IO.Directory.CreateDirectory(newPath);
            newPath = System.IO.Path.Combine(activeDir, "Données brutes");
            System.IO.Directory.CreateDirectory(newPath);
            activeDir = @"../../Files/Patients/" + Nom + "_" + Prenom + "_" + DateNaiss.Day + "_" + DateNaiss.Month + "_" + DateNaiss.Year + "/Rééducation/Données brutes";
            newPath = System.IO.Path.Combine(activeDir, "Mouvements simples");
            System.IO.Directory.CreateDirectory(newPath);
            newPath = System.IO.Path.Combine(activeDir, "Mouvements complexes");
            System.IO.Directory.CreateDirectory(newPath);
            newPath = System.IO.Path.Combine(activeDir, "Mouvements rythmiques");
            System.IO.Directory.CreateDirectory(newPath);
            AddFileEva();
        }

        private void AddFileEva()
        {
            string pathString = @"../../Files/Patients/" + Nom + "_" + Prenom + "_" + DateNaiss.Day + "_" + DateNaiss.Month + "_" + DateNaiss.Year + "/Evaluation/Résultats";
            string fileNameCercle = "Cercle.txt";
            string fileNameGA = "Grande Amplitude.txt";
            string fileNameCible = "Cible.txt";
            string fileNameCarre = "Carre.txt";
            string pathStringCercle = System.IO.Path.Combine(pathString, fileNameCercle);
            string pathStringGA = System.IO.Path.Combine(pathString, fileNameGA);
            string pathStringCible = System.IO.Path.Combine(pathString, fileNameCible);
            string pathStringCarre = System.IO.Path.Combine(pathString, fileNameCarre);

            if (!System.IO.File.Exists(pathStringCercle))
            {
                System.IO.File.Create(pathStringCercle);
            }
            else
            {
                Console.WriteLine("File \"{0}\" already exists.", fileNameCercle);
                return;
            }

            if (!System.IO.File.Exists(pathStringGA))
            {
                System.IO.File.Create(pathStringGA);
            }
            else
            {
                Console.WriteLine("File \"{0}\" already exists.", fileNameGA);
                return;
            }

            if (!System.IO.File.Exists(pathStringCible))
            {
                System.IO.File.Create(pathStringCible);
            }
            else
            {
                Console.WriteLine("File \"{0}\" already exists.", fileNameCible);
                return;
            }

            if (!System.IO.File.Exists(pathStringCarre))
            {
                System.IO.File.Create(pathStringCarre);
            }
            else
            {
                Console.WriteLine("File \"{0}\" already exists.", fileNameCarre);
                return;
            }

            string pathStringReed = @"../../Files/Patients/" + Nom + "_" + Prenom + "_" + DateNaiss.Day + "_" + DateNaiss.Month + "_" + DateNaiss.Year + "/Rééducation/Résultats";
            string fileNameSimples = "Mouvements simples.txt";
            string fileNameComplexes = "Mouvements complexes.txt";
            string fileNameRythmiques = "Mouvements rythmiques.txt";
            string pathStringSimples = System.IO.Path.Combine(pathStringReed, fileNameSimples);
            string pathStringComplexes = System.IO.Path.Combine(pathStringReed, fileNameComplexes);
            string pathStringRythmiques = System.IO.Path.Combine(pathStringReed, fileNameRythmiques);

            if (!System.IO.File.Exists(pathStringSimples))
            {
                System.IO.File.Create(pathStringSimples);
            }
            else
            {
                Console.WriteLine("File \"{0}\" already exists.", fileNameSimples);
                return;
            }

            if (!System.IO.File.Exists(pathStringComplexes))
            {
                System.IO.File.Create(pathStringComplexes);
            }
            else
            {
                Console.WriteLine("File \"{0}\" already exists.", fileNameComplexes);
                return;
            }

            if (!System.IO.File.Exists(pathStringRythmiques))
            {
                System.IO.File.Create(pathStringRythmiques);
            }
            else
            {
                Console.WriteLine("File \"{0}\" already exists.", fileNameRythmiques);
                return;
            }
        }
    }
}
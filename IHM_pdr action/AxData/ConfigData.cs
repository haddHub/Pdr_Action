using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxModel;
using System.Xml.Linq;
namespace AxData
{
    public static class ConfigData
    {
        private static XDocument fileConfig;
        public static ConfigProgramme LoadConfigProgramme()
        {
            fileConfig = XDocument.Load(@"../../Config/ConfigProgramme.xml");
            bool debug = (bool)fileConfig.Root.Element("Debug");
            double version = (double)fileConfig.Root.Element("Version");
            bool fermeture;
            if (fileConfig.Root.Element("BonneFermeture").Value == "")
            {
                fermeture = true;
                ChangerBonneFermeture(true);
            }
            else
            {
                fermeture = (bool)fileConfig.Root.Element("BonneFermeture");
                if (fermeture == true)
                    ChangerBonneFermeture(false);
            }
            string date;
            //si pas de date d'installation dans le fichier de config
            //la date du jour est ajoutée
            if (fileConfig.Root.Element("DateInstallation").Value == "")
            {
                date = DateTime.Now.ToShortDateString();
                fileConfig.Root.Element("DateInstallation").Remove();
                fileConfig.Root.Add(new XElement("DateInstallation",date));
                fileConfig.Save(@"../../Config/ConfigProgramme.xml");
            }
            else
                date = fileConfig.Root.Element("DateInstallation").Value;

            //si pas de culture dans le fichier la culture de l'os est ajoutée
            string culture;
            if (fileConfig.Root.Element("Culture").Value == "")
            {
                culture = System.Globalization.CultureInfo.CurrentCulture.Name;
                ChangerCulture(culture);
            }
            else
                culture = fileConfig.Root.Element("Culture").Value;
            return new ConfigProgramme(version, debug, date, fermeture,culture);
        }

        public static void ChangerBonneFermeture(bool statu)
        {
            fileConfig.Root.Element("BonneFermeture").Remove();
            fileConfig.Root.Add(new XElement("BonneFermeture", statu));
            fileConfig.Save(@"../../Config/ConfigProgramme.xml");
        }

        public static void ChangerCulture(string culture)
        {
            fileConfig.Root.Element("Culture").Remove();
            fileConfig.Root.Add(new XElement("Culture", culture));
            fileConfig.Save(@"../../Config/ConfigProgramme.xml");
        }

        public static List<ExerciceBorneConfig> LoadAllBorneConfig()
        {
            XDocument fileConfig = XDocument.Load(@"../../Config/Borne/configBorne.xml");
            var bornes = from borne in fileConfig.Descendants("Borne") select borne;
            List<ExerciceBorneConfig> listeBorne = new List<ExerciceBorneConfig>();
            foreach (var borne in bornes)
            {
                string taille = (string)borne.Element("Taille");
                double borneG = (double)borne.Element("BorneGauche");
                double borneD = (double)borne.Element("BorneDroite");
                double borneH = (double)borne.Element("BorneHaut");
                double borneB = (double)borne.Element("BorneBas");
                double borneArcH = (double)borne.Element("BorneArcHaut");
                double borneArcB = (double)borne.Element("BorneArcBas");
                ExerciceBorneConfig borneConf = new ExerciceBorneConfig(taille, borneG, borneD, borneH, borneB, borneArcH, borneArcB);
                listeBorne.Add(borneConf);
            }
            return listeBorne;
        }

        public static ExerciceBaseConfig LoadExerciceBaseConfig()
        {
            XDocument fileConfig = XDocument.Load(@"../../Config/Base/configBase.xml");
            byte masse = (byte)((int)fileConfig.Root.Element("Masse"));
            byte viscosite = (byte)((int)fileConfig.Root.Element("Viscosite"));
            byte raideurLat = (byte)((int)fileConfig.Root.Element("RaideurLat"));
            byte raideurLong = (byte)((int)fileConfig.Root.Element("RaideurLong"));
            byte vitesse = (byte)((int)fileConfig.Root.Element("Vitesse"));
            byte nbrRep = (byte)((int)fileConfig.Root.Element("NbrRep"));
            byte init = (byte)((int)fileConfig.Root.Element("Init"));
            bool auto = (bool)fileConfig.Root.Element("Auto");
            //return new ExerciceBaseConfig(masse, viscosite, raideurLat, raideurLong, vitesse, nbrRep, init, auto);
            return new ExerciceBaseConfig();
        }

        public static ExerciceBaseConfig LoadLastConfigPourExercice(string nomExo)
        {
            ExerciceBaseConfig config = LoadExerciceBaseConfig();
            var patientSingleton = Singleton.getInstance().PatientSingleton;

            using (ReaPlanDBEntities db = new ReaPlanDBEntities())
            {
                PatientDB pat = (from p in db.PatientDBs
                                  where p.Id1 == patientSingleton.ID1 &&
                                  p.Id2 == patientSingleton.ID2 &&
                                  p.DateNaissance == patientSingleton.DateNaiss
                                  select p).FirstOrDefault();
                if (pat !=null)
                {
                    JeuDB jeu = (from j in db.JeuDBs
                                 where j.NomJeu == nomExo
                                 select j).FirstOrDefault();

                    if (jeu !=null)
                    {
                        ExerciceDB exo = (from e in db.ExerciceDBs
                                          where e.IdPatient == pat.IdPatient &&
                                          e.IdJeu == jeu.IdJeu
                                          orderby e.Date
                                          orderby e.Heure descending
                                          select e).FirstOrDefault();

                        if (exo != null)
                        {
                            ConfigJeuDB conf = (from c in db.ConfigJeuDBs
                                                where c.IdExercice == exo.IdExercice
                                                select c).FirstOrDefault();

                            if (conf != null)
                            {
                                config.Init = (byte)conf.Initialisation;
                                config.RaideurLat = (byte)conf.RaideurLat;
                                config.RaideurLong = (byte)conf.RaideurLong;
                                config.Vitesse = (byte)conf.Vitesse;   
                            }
                        }
                    }
                }
                return config;
            }
        }

        public static void SaveConfigJeu()
        {
            
        }
    }
}

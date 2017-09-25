using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using AxModel;
using AxModelExercice;

namespace AxData
{
    public static class ExerciceJeuData
    {
        public static void AjoutPartieDataBaseJeu(ExerciceBaseConfig config)
        {
            Singleton singlePatient = Singleton.getInstance();
            string dossier = singlePatient.Patient.Nom + singlePatient.Patient.Prenom + singlePatient.Patient.DateDeNaissance.ToString().Replace("/", string.Empty);
            ExerciceReeducation exo = new ExerciceReeducation();
            exo.CalculerMoyenneEtCV(dossier);

            using (ReaPlanDBEntities bdd = new ReaPlanDBEntities())
            {
                var requete = from c in bdd.JeuDBs
                              where c.NomJeu == "MvtsComplexes"
                              select c;

                var jeuBD = requete.FirstOrDefault();

                var requeteP = from c in bdd.PatientDBs
                               where c.Nom == singlePatient.PatientSingleton.Nom
                               && c.Prenom == singlePatient.PatientSingleton.Prenom
                               && c.DateNaissance == singlePatient.PatientSingleton.DateNaiss.Date
                               select c;
                var patient = requeteP.FirstOrDefault();

                var requeteA = from c in bdd.TherapeuteDBs
                               where c.Nom == singlePatient.Admin.Nom
                               && c.Prenom == singlePatient.Admin.Prenom
                               && c.Login == singlePatient.Admin.NomUtilisateur
                               select c;
                var thera = requeteA.FirstOrDefault();

                ExerciceDB ex = new ExerciceDB()
                {
                    Date = DateTime.Now,
                    Heure = DateTime.Now.TimeOfDay,
                    IdPatient = patient.IdPatient,
                    IdUtilisateur = thera.IdUtilisateur,
                    IdJeu = jeuBD.IdJeu
                };

                bdd.AddToExerciceDBs(ex);
                bdd.SaveChanges();

                //Recherche de l'id du dernier ex fait par le patient
                var requeteExDB = from c in bdd.ExerciceDBs
                                  select c;

                var exDB = requeteExDB.AsEnumerable().LastOrDefault();

                //Enregistrement de la config
                ConfigJeuDB configJeu = new ConfigJeuDB()
                { 
                    IdExercice = exDB.IdExercice,
                    RaideurLat = config.RaideurLat,
                    RaideurLong = config.RaideurLong,
                    Vitesse = config.Vitesse,
                    Initialisation = config.Init
                };

                bdd.AddToConfigJeuDBs(configJeu);
                bdd.SaveChanges();

                //Ajout pour vitesse
                var requeteParamAmplitude = from c in bdd.ParametreJeuDBs
                                            where c.LibelleParametre == "Vitesse"
                                            && c.IdJeu == jeuBD.IdJeu
                                            select c;
                var paramJeuAmplitude = requeteParamAmplitude.FirstOrDefault();

                ParametreExDB paramExDBAmplitude = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramJeuAmplitude.IdParametreJeuDB,
                    Resultat = (decimal)exo.VitesseMoyenne,
                    CoefficientVariation = (decimal)exo.CVVitesseMoyenne,
                };

                bdd.AddToParametreExDBs(paramExDBAmplitude);
                bdd.SaveChanges();

                ////Ajout pour init
                var requeteParamVitM = from c in bdd.ParametreJeuDBs
                                       where c.LibelleParametre == "Init"
                                       && c.IdJeu == jeuBD.IdJeu
                                       select c;
                var paramjeuVitM = requeteParamVitM.FirstOrDefault();

                ParametreExDB paramExDBVitM = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramjeuVitM.IdParametreJeuDB,
                    Resultat = (decimal)exo.InitMoyen,
                    EcartType = (decimal)exo.CVInit
                };

                bdd.AddToParametreExDBs(paramExDBVitM);
                bdd.SaveChanges();

                //Ajout pour klat
                var requeteParamVitMax = from c in bdd.ParametreJeuDBs
                                         where c.LibelleParametre == "AssisLat"
                                         && c.IdJeu == jeuBD.IdJeu
                                         select c;
                var paramjeuVitMax = requeteParamVitMax.FirstOrDefault();

                ParametreExDB paramExDBVitMax = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramjeuVitMax.IdParametreJeuDB,
                    Resultat = (decimal)exo.RaideurLatMoyenne,
                    CoefficientVariation = (decimal)exo.CVRaideurLat
                };

                bdd.AddToParametreExDBs(paramExDBVitMax);
                bdd.SaveChanges();

                //Ajout pour klong
                var requeteParamSt = from c in bdd.ParametreJeuDBs
                                     where c.LibelleParametre == "AssisLong"
                                         && c.IdJeu == jeuBD.IdJeu
                                     select c;
                var paramjeuSt = requeteParamSt.FirstOrDefault();

                ParametreExDB paramExDBSt = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramjeuSt.IdParametreJeuDB,
                    Resultat = (decimal)exo.RaideurLongMoyenne,
                    CoefficientVariation = (decimal)exo.CVRaideurLong
                };

                bdd.AddToParametreExDBs(paramExDBSt);
                bdd.SaveChanges();

                //Ajout pour nbrMouvement
                var requeteParamSM = from c in bdd.ParametreJeuDBs
                                     where c.LibelleParametre == "NbMouvements"
                                         && c.IdJeu == jeuBD.IdJeu
                                     select c;
                var paramjeuSM = requeteParamSM.FirstOrDefault();

                ParametreExDB paramExDBSM = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramjeuSM.IdParametreJeuDB,
                    Resultat = (decimal)exo.NbrMouvement,
                };

                bdd.AddToParametreExDBs(paramExDBSM);
                bdd.SaveChanges();
            }
        }
    }
}

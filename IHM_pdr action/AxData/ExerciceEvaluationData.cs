using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxModel;
using System.Xml.Linq;
using AxModelExercice;
namespace AxData
{
    public static class ExerciceEvaluationData
    {
        public static void AjoutPartie(string dossier, ExerciceEvaluation exoEval)
        {
            string fileName = FindTypeOfExercice(exoEval);
            
            string dateJour = String.Format("{0:00}", DateTime.Now.Day) + "/" + String.Format("{0:00}", DateTime.Now.Month) + "/" + DateTime.Now.Year;
            XDocument file = XDocument.Load(@"../../Files/Patients/" + dossier + "/Evaluation/" + fileName + ".xml");
            XElement newDonneeCommun = new XElement("Partie", new XElement("Date", dateJour),
                                                        new XElement("Speed",
                                                            new XElement("ResultatMoyen", exoEval.VitesseMoy.ToString()),
                                                            new XElement("EcartType", exoEval.EcartTypeVMoy.ToString()),
                                                            new XElement("CoefficientVariation", exoEval.CVVitesseMoy.ToString())),
                                                        new XElement("PeakSpeed",
                                                            new XElement("ResultatMoyen", exoEval.VitesseMax.ToString()),
                                                            new XElement("EcartType", exoEval.EcartTypeVMax.ToString()),
                                                            new XElement("CoefficientVariation", exoEval.CVVitesseMax.ToString())),
                                                        new XElement("Jerk",
                                                            new XElement("ResultatMoyen", exoEval.JerkMetric.ToString()),
                                                            new XElement("EcartType", exoEval.EcartTypeJM.ToString()),
                                                            new XElement("CoefficientVariation", exoEval.CVJerkMetric.ToString())),
                                                        new XElement("SpeedMetric",
                                                            new XElement("ResultatMoyen", exoEval.SpeedMetric.ToString()),
                                                            new XElement("EcartType", exoEval.EcartTypeSM.ToString()),
                                                            new XElement("CoefficientVariation", exoEval.CVSpeedMetric.ToString())));
            XElement newDonnee;
            if (fileName == "Square" || fileName == "Circle")
            {
                ExerciceForme exoForme = (ExerciceForme)exoEval;
                newDonnee = new XElement("Precision",
                                new XElement("ResultatMoyen", exoForme.ShapeAccuracy.ToString()),
                                new XElement("EcartType", exoForme.EcartTypeSA.ToString()),
                                new XElement("CoefficientVariation", exoForme.CVShapeAccuracy.ToString()));
            }
            else
            {
                ExerciceMouvement exoMouv = (ExerciceMouvement)exoEval;
                newDonnee = new XElement("Straightness",
                                new XElement("ResultatMoyen", exoMouv.Linearite.ToString()),
                                new XElement("EcartType", exoMouv.EcartTypeLin.ToString()),
                                new XElement("CoefficientVariation", exoMouv.CVLinearite.ToString()));
                XElement newDonneeMouv;
                if (exoMouv.TypeDroite==DroiteType.Vertical)
                {
                    Target target = (Target)exoMouv;
                    newDonneeMouv = new XElement("Accuracy",
                                        new XElement("ResultatMoyen", target.Precision.ToString()),
                                        new XElement("EcartType", target.EcartTypePre.ToString()),
                                        new XElement("CoefficientVariation", target.CVPrecision.ToString()));
                }
                else
                {
                    FreeAmplitude freeAmpl = (FreeAmplitude)exoMouv;
                    newDonneeMouv = new XElement("Amplitude",
                                        new XElement("ResultatMoyen", freeAmpl.Amplitude.ToString()),
                                        new XElement("EcartType", freeAmpl.EcartTypeAmp.ToString()),
                                        new XElement("CoefficientVariation", freeAmpl.CVAmplitude.ToString()));
                }
                newDonnee.Add(newDonneeMouv);
            }
            newDonneeCommun.Add(newDonnee);
            file.Root.Add(newDonneeCommun);
            file.Save(@"../../Files/Patients/" + dossier + "/Evaluation/" + fileName + ".xml");
        }

        private static string FindTypeOfExercice(ExerciceEvaluation exoEval)
        {
            Singleton singlePatient = Singleton.getInstance();
            string fileName;

            if (exoEval.TypeEval == ExerciceEvalTypes.Forme)
            {
                ExerciceForme exoForme = (ExerciceForme)exoEval;
                if (exoForme.TypeForme == FormeType.Carré)
                    fileName = "Square";
                else
                    fileName = "Circle";
            }
            else
            {
                ExerciceMouvement exoMouv = (ExerciceMouvement)exoEval;
                if (exoMouv.TypeDroite == DroiteType.Vertical)
                    fileName = "Target";
                else
                    fileName = "FreeAmplitude";
            }
            return fileName;
        }

        public static void AjoutPartieDataBase(ExerciceEvaluation exo)
        {
            Singleton singlePatient = Singleton.getInstance();

            if (exo is Circle)
            {
                AjoutPartieDataBaseCircle(exo as Circle);
            }
            if (exo is Square)
            {
                AjoutPartieDataBaseSquare(exo as Square);
            }
            if (exo is Target)
            {
                AjoutPartieDataBaseTarget(exo as Target);
            }
            if (exo is FreeAmplitude)
            {
                AjoutPartieDataBaseFreeAmplitude(exo as FreeAmplitude);
            }
        }

        private static void AjoutPartieDataBaseFreeAmplitude(FreeAmplitude exo)
        {
            Singleton singlePatient = Singleton.getInstance();

            using (ReaPlanDBEntities bdd = new ReaPlanDBEntities())
            {
                var requete = from c in bdd.JeuDBs
                              where c.NomJeu == "FreeAmplitude"
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

                //Ajout pour amplitude
                var requeteParamAmplitude = from c in bdd.ParametreJeuDBs
                                   where c.LibelleParametre == "Amplitude"
                                   && c.IdJeu == jeuBD.IdJeu
                                   select c;
                var paramJeuAmplitude = requeteParamAmplitude.FirstOrDefault();

                ParametreExDB paramExDBAmplitude = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramJeuAmplitude.IdParametreJeuDB,
                    Resultat = (decimal)exo.Amplitude,
                    CoefficientVariation = (decimal)exo.CVAmplitude,
                    EcartType = (decimal)exo.EcartTypeAmp
                };

                bdd.AddToParametreExDBs(paramExDBAmplitude);
                bdd.SaveChanges();

                //Ajout pour vitesse moyenne
                var requeteParamVitM = from c in bdd.ParametreJeuDBs
                                   where c.LibelleParametre == "VitesseMoyenne"
                                   && c.IdJeu == jeuBD.IdJeu
                                   select c;
                var paramjeuVitM = requeteParamVitM.FirstOrDefault();

                ParametreExDB paramExDBVitM = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramjeuVitM.IdParametreJeuDB,
                    Resultat = (decimal)exo.VitesseMoy,
                    CoefficientVariation = (decimal)exo.CVVitesseMoy,
                    EcartType = (decimal)exo.EcartTypeVMoy
                };

                bdd.AddToParametreExDBs(paramExDBVitM);
                bdd.SaveChanges();

                //Ajout pour vitesse max
                var requeteParamVitMax = from c in bdd.ParametreJeuDBs
                                       where c.LibelleParametre == "VitesseMax"
                                       && c.IdJeu == jeuBD.IdJeu
                                       select c;
                var paramjeuVitMax = requeteParamVitMax.FirstOrDefault();

                ParametreExDB paramExDBVitMax = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramjeuVitMax.IdParametreJeuDB,
                    Resultat = (decimal)exo.VitesseMax,
                    CoefficientVariation = (decimal)exo.CVVitesseMax,
                    EcartType = (decimal)exo.EcartTypeVMax
                };

                bdd.AddToParametreExDBs(paramExDBVitMax);
                bdd.SaveChanges();

                //Ajout pour Straightness
                var requeteParamSt = from c in bdd.ParametreJeuDBs
                                     where c.LibelleParametre == "Straigthness"
                                         && c.IdJeu == jeuBD.IdJeu
                                         select c;
                var paramjeuSt = requeteParamSt.FirstOrDefault();

                ParametreExDB paramExDBSt = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramjeuSt.IdParametreJeuDB,
                    Resultat = (decimal)exo.Linearite,
                    CoefficientVariation = (decimal)exo.CVLinearite,
                    EcartType = (decimal)exo.EcartTypeLin
                };

                bdd.AddToParametreExDBs(paramExDBSt);
                bdd.SaveChanges();

                //Ajout pour speedMetric
                var requeteParamSM = from c in bdd.ParametreJeuDBs
                                     where c.LibelleParametre == "SpeedMetric"
                                         && c.IdJeu == jeuBD.IdJeu
                                     select c;
                var paramjeuSM = requeteParamSM.FirstOrDefault();

                ParametreExDB paramExDBSM = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramjeuSM.IdParametreJeuDB,
                    Resultat = (decimal)exo.SpeedMetric,
                    CoefficientVariation = (decimal)exo.CVSpeedMetric,
                    EcartType = (decimal)exo.EcartTypeSM
                };

                bdd.AddToParametreExDBs(paramExDBSM);
                bdd.SaveChanges();

                //Ajout pour speedMetric
                var requeteParamJM = from c in bdd.ParametreJeuDBs
                                     where c.LibelleParametre == "JerkMetric"
                                         && c.IdJeu == jeuBD.IdJeu
                                     select c;
                var paramjeuJM = requeteParamJM.FirstOrDefault();

                ParametreExDB paramExDBJM = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramjeuJM.IdParametreJeuDB,
                    Resultat = (decimal)exo.JerkMetric,
                    CoefficientVariation = (decimal)exo.CVJerkMetric,
                    EcartType = (decimal)exo.EcartTypeJM
                };

                bdd.AddToParametreExDBs(paramExDBJM);
                bdd.SaveChanges();
            }
        }

        private static void AjoutPartieDataBaseTarget(Target exo)
        {
            Singleton singlePatient = Singleton.getInstance();

            using (ReaPlanDBEntities bdd = new ReaPlanDBEntities())
            {
                var requete = from c in bdd.JeuDBs
                              where c.NomJeu == "Target"
                              select c;

                var jeuBD = requete.FirstOrDefault();

                var requeteP = from c in bdd.PatientDBs
                               where c.Nom == singlePatient.PatientSingleton.Nom
                               && c.Prenom == singlePatient.PatientSingleton.Prenom
                               && c.DateNaissance == singlePatient.PatientSingleton.DateNaiss
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

                //Ajout pour amplitude
                var requeteParamPrecision = from c in bdd.ParametreJeuDBs
                                            where c.LibelleParametre == "Accuracy"
                                            && c.IdJeu == jeuBD.IdJeu
                                            select c;
                var paramJeuPrecision = requeteParamPrecision.FirstOrDefault();

                ParametreExDB paramExDBPrecision = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramJeuPrecision.IdParametreJeuDB,
                    Resultat = (decimal)exo.Precision,
                    CoefficientVariation = (decimal)exo.CVPrecision,
                    EcartType = (decimal)exo.EcartTypePre
                };

                bdd.AddToParametreExDBs(paramExDBPrecision);
                bdd.SaveChanges();

                //Ajout pour vitesse moyenne
                var requeteParamVitM = from c in bdd.ParametreJeuDBs
                                       where c.LibelleParametre == "Speed"
                                       && c.IdJeu == jeuBD.IdJeu
                                       select c;
                var paramjeuVitM = requeteParamVitM.FirstOrDefault();

                ParametreExDB paramExDBVitM = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramjeuVitM.IdParametreJeuDB,
                    Resultat = (decimal)exo.VitesseMoy,
                    CoefficientVariation = (decimal)exo.CVVitesseMoy,
                    EcartType = (decimal)exo.EcartTypeVMoy
                };

                bdd.AddToParametreExDBs(paramExDBVitM);
                bdd.SaveChanges();

                //Ajout pour vitesse max
                var requeteParamVitMax = from c in bdd.ParametreJeuDBs
                                         where c.LibelleParametre == "PeakSpeed"
                                         && c.IdJeu == jeuBD.IdJeu
                                         select c;
                var paramjeuVitMax = requeteParamVitMax.FirstOrDefault();

                ParametreExDB paramExDBVitMax = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramjeuVitMax.IdParametreJeuDB,
                    Resultat = (decimal)exo.VitesseMax,
                    CoefficientVariation = (decimal)exo.CVVitesseMax,
                    EcartType = (decimal)exo.EcartTypeVMax
                };

                bdd.AddToParametreExDBs(paramExDBVitMax);
                bdd.SaveChanges();

                //Ajout pour Straightness
                var requeteParamSt = from c in bdd.ParametreJeuDBs
                                     where c.LibelleParametre == "Straightness"
                                         && c.IdJeu == jeuBD.IdJeu
                                     select c;
                var paramjeuSt = requeteParamSt.FirstOrDefault();

                ParametreExDB paramExDBSt = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramjeuSt.IdParametreJeuDB,
                    Resultat = (decimal)exo.Linearite,
                    CoefficientVariation = (decimal)exo.CVLinearite,
                    EcartType = (decimal)exo.EcartTypeLin
                };

                bdd.AddToParametreExDBs(paramExDBSt);
                bdd.SaveChanges();

                //Ajout pour speedMetric
                var requeteParamSM = from c in bdd.ParametreJeuDBs
                                     where c.LibelleParametre == "SpeedMetric"
                                         && c.IdJeu == jeuBD.IdJeu
                                     select c;
                var paramjeuSM = requeteParamSM.FirstOrDefault();

                ParametreExDB paramExDBSM = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramjeuSM.IdParametreJeuDB,
                    Resultat = (decimal)exo.SpeedMetric,
                    CoefficientVariation = (decimal)exo.CVSpeedMetric,
                    EcartType = (decimal)exo.EcartTypeSM
                };

                bdd.AddToParametreExDBs(paramExDBSM);
                bdd.SaveChanges();

                //Ajout pour jerkMetric
                var requeteParamJM = from c in bdd.ParametreJeuDBs
                                     where c.LibelleParametre == "Jerk"
                                         && c.IdJeu == jeuBD.IdJeu
                                     select c;
                var paramjeuJM = requeteParamJM.FirstOrDefault();

                ParametreExDB paramExDBJM = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramjeuJM.IdParametreJeuDB,
                    Resultat = (decimal)exo.JerkMetric,
                    CoefficientVariation = (decimal)exo.CVJerkMetric,
                    EcartType = (decimal)exo.EcartTypeJM
                };

                bdd.AddToParametreExDBs(paramExDBJM);
                bdd.SaveChanges();
            }
        }

        private static void AjoutPartieDataBaseSquare(Square exo)
        {
            Singleton singlePatient = Singleton.getInstance();

            using (ReaPlanDBEntities bdd = new ReaPlanDBEntities())
            {
                var requete = from c in bdd.JeuDBs
                              where c.NomJeu == "Square"
                              select c;

                var jeuBD = requete.FirstOrDefault();

                var requeteP = from c in bdd.PatientDBs
                               where c.Nom == singlePatient.PatientSingleton.Nom
                               && c.Prenom == singlePatient.PatientSingleton.Prenom
                               && c.DateNaissance == singlePatient.PatientSingleton.DateNaiss
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

                //Ajout pour amplitude
                var requeteParamPrecision = from c in bdd.ParametreJeuDBs
                                            where c.LibelleParametre == "ShapeAccuracy"
                                            && c.IdJeu == jeuBD.IdJeu
                                            select c;
                var paramJeuPrecision = requeteParamPrecision.FirstOrDefault();

                ParametreExDB paramExDBPrecision = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramJeuPrecision.IdParametreJeuDB,
                    Resultat = (decimal)exo.ShapeAccuracy,
                    CoefficientVariation = (decimal)exo.CVShapeAccuracy,
                    EcartType = (decimal)exo.EcartTypeSA
                };

                bdd.AddToParametreExDBs(paramExDBPrecision);
                bdd.SaveChanges();

                //Ajout pour vitesse moyenne
                var requeteParamVitM = from c in bdd.ParametreJeuDBs
                                       where c.LibelleParametre == "Speed"
                                       && c.IdJeu == jeuBD.IdJeu
                                       select c;
                var paramjeuVitM = requeteParamVitM.FirstOrDefault();

                ParametreExDB paramExDBVitM = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramjeuVitM.IdParametreJeuDB,
                    Resultat = (decimal)exo.VitesseMoy,
                    CoefficientVariation = (decimal)exo.CVVitesseMoy,
                    EcartType = (decimal)exo.EcartTypeVMoy
                };

                bdd.AddToParametreExDBs(paramExDBVitM);
                bdd.SaveChanges();

                //Ajout pour vitesse max
                var requeteParamVitMax = from c in bdd.ParametreJeuDBs
                                         where c.LibelleParametre == "PeakSpeed"
                                         && c.IdJeu == jeuBD.IdJeu
                                         select c;
                var paramjeuVitMax = requeteParamVitMax.FirstOrDefault();

                ParametreExDB paramExDBVitMax = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramjeuVitMax.IdParametreJeuDB,
                    Resultat = (decimal)exo.VitesseMax,
                    CoefficientVariation = (decimal)exo.CVVitesseMax,
                    EcartType = (decimal)exo.EcartTypeVMax
                };

                bdd.AddToParametreExDBs(paramExDBVitMax);
                bdd.SaveChanges();

                //Ajout pour speedMetric
                var requeteParamSM = from c in bdd.ParametreJeuDBs
                                     where c.LibelleParametre == "SpeedMetric"
                                         && c.IdJeu == jeuBD.IdJeu
                                     select c;
                var paramjeuSM = requeteParamSM.FirstOrDefault();

                ParametreExDB paramExDBSM = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramjeuSM.IdParametreJeuDB,
                    Resultat = (decimal)exo.SpeedMetric,
                    CoefficientVariation = (decimal)exo.CVSpeedMetric,
                    EcartType = (decimal)exo.EcartTypeSM
                };

                bdd.AddToParametreExDBs(paramExDBSM);
                bdd.SaveChanges();

                //Ajout pour speedMetric
                var requeteParamJM = from c in bdd.ParametreJeuDBs
                                     where c.LibelleParametre == "Jerk"
                                         && c.IdJeu == jeuBD.IdJeu
                                     select c;
                var paramjeuJM = requeteParamJM.FirstOrDefault();

                ParametreExDB paramExDBJM = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramjeuJM.IdParametreJeuDB,
                    Resultat = (decimal)exo.JerkMetric,
                    CoefficientVariation = (decimal)exo.CVJerkMetric,
                    EcartType = (decimal)exo.EcartTypeJM
                };

                bdd.AddToParametreExDBs(paramExDBJM);
                bdd.SaveChanges();
            }
        }

        private static void AjoutPartieDataBaseCircle(Circle exo)
        {
            Singleton singlePatient = Singleton.getInstance();

            using (ReaPlanDBEntities bdd = new ReaPlanDBEntities())
            {
                var requete = from c in bdd.JeuDBs
                              where c.NomJeu == "Circle"
                              select c;

                var jeuBD = requete.FirstOrDefault();

                var requeteP = from c in bdd.PatientDBs
                               where c.Nom == singlePatient.PatientSingleton.Nom
                               && c.Prenom == singlePatient.PatientSingleton.Prenom
                               && c.DateNaissance == singlePatient.PatientSingleton.DateNaiss
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

                //Ajout pour amplitude
                var requeteParamPrecision = from c in bdd.ParametreJeuDBs
                                            where c.LibelleParametre == "ShapeAccuracy"
                                            && c.IdJeu == jeuBD.IdJeu
                                            select c;
                var paramJeuPrecision = requeteParamPrecision.FirstOrDefault();

                ParametreExDB paramExDBPrecision = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramJeuPrecision.IdParametreJeuDB,
                    Resultat = (decimal)exo.ShapeAccuracy,
                    CoefficientVariation = (decimal)exo.CVShapeAccuracy,
                    EcartType = (decimal)exo.EcartTypeSA
                };

                bdd.AddToParametreExDBs(paramExDBPrecision);
                bdd.SaveChanges();

                //Ajout pour vitesse moyenne
                var requeteParamVitM = from c in bdd.ParametreJeuDBs
                                       where c.LibelleParametre == "Speed"
                                       && c.IdJeu == jeuBD.IdJeu
                                       select c;
                var paramjeuVitM = requeteParamVitM.FirstOrDefault();

                ParametreExDB paramExDBVitM = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramjeuVitM.IdParametreJeuDB,
                    Resultat = (decimal)exo.VitesseMoy,
                    CoefficientVariation = (decimal)exo.CVVitesseMoy,
                    EcartType = (decimal)exo.EcartTypeVMoy
                };

                bdd.AddToParametreExDBs(paramExDBVitM);
                bdd.SaveChanges();

                //Ajout pour vitesse max
                var requeteParamVitMax = from c in bdd.ParametreJeuDBs
                                         where c.LibelleParametre == "PeakSpeed"
                                         && c.IdJeu == jeuBD.IdJeu
                                         select c;
                var paramjeuVitMax = requeteParamVitMax.FirstOrDefault();

                ParametreExDB paramExDBVitMax = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramjeuVitMax.IdParametreJeuDB,
                    Resultat = (decimal)exo.VitesseMax,
                    CoefficientVariation = (decimal)exo.CVVitesseMax,
                    EcartType = (decimal)exo.EcartTypeVMax
                };

                bdd.AddToParametreExDBs(paramExDBVitMax);
                bdd.SaveChanges();

                //Ajout pour speedMetric
                var requeteParamSM = from c in bdd.ParametreJeuDBs
                                     where c.LibelleParametre == "SpeedMetric"
                                         && c.IdJeu == jeuBD.IdJeu
                                     select c;
                var paramjeuSM = requeteParamSM.FirstOrDefault();

                ParametreExDB paramExDBSM = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramjeuSM.IdParametreJeuDB,
                    Resultat = (decimal)exo.SpeedMetric,
                    CoefficientVariation = (decimal)exo.CVSpeedMetric,
                    EcartType = (decimal)exo.EcartTypeSM
                };

                bdd.AddToParametreExDBs(paramExDBSM);
                bdd.SaveChanges();

                //Ajout pour speedMetric
                var requeteParamJM = from c in bdd.ParametreJeuDBs
                                     where c.LibelleParametre == "Jerk"
                                         && c.IdJeu == jeuBD.IdJeu
                                     select c;
                var paramjeuJM = requeteParamJM.FirstOrDefault();

                ParametreExDB paramExDBJM = new ParametreExDB()
                {
                    IdExercice = exDB.IdExercice,
                    IdParametreJeuDB = paramjeuJM.IdParametreJeuDB,
                    Resultat = (decimal)exo.JerkMetric,
                    CoefficientVariation = (decimal)exo.CVJerkMetric,
                    EcartType = (decimal)exo.EcartTypeJM
                };

                bdd.AddToParametreExDBs(paramExDBJM);
                bdd.SaveChanges();
            }
        }

    }
}

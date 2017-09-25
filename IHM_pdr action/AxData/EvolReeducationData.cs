using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxModel;
using System.Xml.Linq;

namespace AxData
{
    public static class EvolReeducationData
    {
        public static Singleton user;

        public static List<PointEvoEval> ReturnPoint(int param, string nomJeu)
        {
            List<PointEvoEval> value = new List<PointEvoEval>();
            user = Singleton.getInstance();

            using (ReaPlanDBEntities context = new ReaPlanDBEntities())
            {
                var requete = from c in context.PatientDBs
                              where c.Nom == user.PatientSingleton.Nom
                              && c.Prenom == user.PatientSingleton.Prenom
                              && c.DateNaissance == user.PatientSingleton.DateNaiss.Date
                              select c;

                var patient = requete.FirstOrDefault();

                JeuDB jeu = (from j in context.JeuDBs
                             where j.NomJeu == nomJeu
                             select j).FirstOrDefault();

                var requeteEx = (from c in context.ExerciceDBs
                                where c.IdPatient == patient.IdPatient
                                && c.IdJeu == jeu.IdJeu
                                select c);
                //trie les exo par date puis par heure du plus grand au plus petit)
                var listeExerciceOrdre = requeteEx.OrderByDescending(c => c.Date).ThenBy(h => h.Heure);
                List<ExerciceDB> listeExo = new List<ExerciceDB>();
                //cree une liste d'exo avec seulement le dernier exo de chaque jours
                for (int i = 0; i < listeExerciceOrdre.Count(); i++)
                {
                    if (i < listeExerciceOrdre.Count()-1)
                    {
                        if (listeExerciceOrdre.ToArray().ElementAt(i).Date != listeExerciceOrdre.ToArray().ElementAt(i + 1).Date)
                            listeExo.Add(listeExerciceOrdre.ToArray().ElementAt(i)); 
                    }
                    else
                        listeExo.Add(listeExerciceOrdre.ToArray().ElementAt(i)); 
                }
                

                foreach (var exo in listeExo)
                {
                    PointEvoEval p = new PointEvoEval();
                    var resu = from c in context.ParametreExDBs
                               where c.IdParametreJeuDB == param
                               && c.IdExercice == exo.IdExercice
                               select c;
                    if (resu.ToArray().Count() != 0)
                    {
                        //p.Moyenne = Convert.ToDouble(resu.ToArray().ElementAt(0).Resultat);
                        p.EcartT = Convert.ToDouble(resu.ToArray().ElementAt(0).EcartType);
                        p.CV = Convert.ToDouble(resu.ToArray().ElementAt(0).CoefficientVariation);
                        p.Date = exo.Date;
                        value.Add(p); 
                    }
                }
                //inverse l'ordre d'apparition des dates
                value.Reverse();
                return value;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxModel;
using System.Xml.Linq;

namespace AxData
{
    public static class ResultatEvalData
    {
        public static Singleton user;
        //public static XDocument LoadFileResult(string dossier,string exercice)
        //{
        //    XDocument fileResult = XDocument.Load(@"../../Files/Patients/" + dossier + "/Evaluation/" + exercice + ".xml");
        //    return fileResult;
        //}

        //public static XDocument LoadFileNorme(string exercice)
        //{
        //    XDocument fileNorme = XDocument.Load(@"../../Files/Normes/Evaluation/" + exercice + ".xml");
        //    return fileNorme;
        //}

        public static List<PointEvoEval> ReturnVal(int nomParametre)
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

                var requeteEx = from c in context.ExerciceDBs
                                where c.IdPatient == patient.IdPatient
                                select c;

                requeteEx.ToList().ForEach((ExerciceDB ex) =>
                {
                    PointEvoEval p = new PointEvoEval();

                    var resu = from c in context.ParametreExDBs
                               where c.IdParametreJeuDB == nomParametre
                               && c.IdExercice == ex.IdExercice
                               select c;

                    foreach (var item in resu)
                    {
                        if (item.Resultat != null)
                        {
                            //p.Moyenne = Convert.ToDouble(item.Resultat);
                            p.Date = ex.Date;

                            value.Add(p);

                        }
                    }

                    //requeteParamEx.ToList().ForEach((ParametreExDB ParamEx) =>
                    //{
                    //    context.DeleteObject(ParamEx);
                    //    context.SaveChanges();
                    //});
                    //context.DeleteObject(ex);
                    //context.SaveChanges();
                });
                return value;
            }
        }

        public static List<PointEvoEval> CoefficientVar(int nomParametre)
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

                var requeteEx = from c in context.ExerciceDBs
                                where c.IdPatient == patient.IdPatient
                                select c;

                requeteEx.ToList().ForEach((ExerciceDB ex) =>
                {
                    PointEvoEval p = new PointEvoEval();

                    var resu = from c in context.ParametreExDBs
                               where c.IdParametreJeuDB == nomParametre
                               && c.IdExercice == ex.IdExercice
                               select c;

                    foreach (var item in resu)
                    {
                        if (item.Resultat != null)
                        {
                            //p.Moyenne = Convert.ToDouble(item.CoefficientVariation);
                            p.Date = ex.Date;

                            value.Add(p);

                        }
                    }
                });
                return value;
            }
        }

        public static void returnNorme(double[] TabNorme, int valeur)
        {
            double constanteA = 0, y0 = 0;
            double deviationStandard = 0;
            int age;

            List<PointEvoEval> value = new List<PointEvoEval>();
            user = Singleton.getInstance();
            age = CalculateAge(user.PatientSingleton.DateNaiss);

            using (ReaPlanDBEntities context = new ReaPlanDBEntities())
            {
                var requete = from c in context.PatientDBs
                              where c.Nom == user.PatientSingleton.Nom
                              && c.Prenom == user.PatientSingleton.Prenom
                              && c.DateNaissance == user.PatientSingleton.DateNaiss.Date
                              select c;

                var patient = requete.FirstOrDefault();

                var requeteEx = from c in context.ExerciceDBs
                                where c.IdPatient == patient.IdPatient
                                select c;

                requeteEx.ToList().ForEach((ExerciceDB ex) =>
                {
                    PointEvoEval p = new PointEvoEval();

                    var resu = from c in context.ParametreJeuDBs
                               where c.IdParametreJeuDB == valeur
                               select c;

                    foreach (var item in resu)
                    {
                        if (item != null)
                        {
                            constanteA = Convert.ToDouble(item.ConstanteA);
                            y0 = Convert.ToDouble(item.Yo);
                            deviationStandard = Convert.ToDouble(item.DeviationStandard);
                        }
                    }
                });
            }
            TabNorme[0] = (y0 + (constanteA * age) + deviationStandard);
            TabNorme[1] = deviationStandard;
        }

        public static int CalculateAge(DateTime birthdate)
        {
            int years = DateTime.Now.Year - birthdate.Year;
            if (DateTime.Now.Month < birthdate.Month
            || (DateTime.Now.Month == birthdate.Month
                && DateTime.Now.Day < birthdate.Day))
                years--;
            return years;
        }
    }
}

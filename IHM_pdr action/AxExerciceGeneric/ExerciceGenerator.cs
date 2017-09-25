using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxModel;
using AxConfiguration;
using AxTheme;
using AxModelExercice;
namespace AxExerciceGenerator
{
    public static class ExerciceGenerator
    {
        public static ExerciceEvaluation GetExerciceEvaluation(string type,ThemeEvaluationModel themeEnfant, TypeExercicePoulies typeExopoulies)
        {
            ExerciceEvaluation exo;
            switch (type)
            {
                case "poulies": exo = new ExercicePoulies(ExerciceConfig.GetBaseConfig(), ExerciceConfig.GetBigBorne(), themeEnfant, new InteractionConfig(), TypeSymetriePoulies.JssHorloge, typeExopoulies);
                    break;
                default: exo = new ExerciceEvaluation(ExerciceConfig.GetBaseConfig(), ExerciceConfig.GetBigBorne(), null);
                    break;
            }
            return exo;
        }

        public static ExerciceJeu GetExerciceJeu(ThemeModel themeJeu, string niveau)
        {
            ExerciceJeu exo = new ExerciceJeu(ExerciceConfig.GetBaseConfigPourExercice("MvtsComplexes"), ExerciceConfig.GetBigBorne(), themeJeu);
            exo.Niveau = GetNiveauByte(niveau);
            return exo;
        }

        private static byte GetNiveauByte(string s)
        {
            byte difficulte;
            switch (s)
            {
                case "Facile": difficulte = 1;
                    break;
                case "Moyen": difficulte = 2;
                    break;
                case "Difficile": difficulte = 3;
                    break;
                case "Expert": difficulte = 4;
                    break;
                default: difficulte = 1;
                    break;
            }
            return difficulte;
        }
    }
}

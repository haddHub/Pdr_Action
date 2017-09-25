using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using AxModel;
using AxData;
namespace AxTheme
{
    public static class GestionThemes
    {
        private static List<ThemeModel> themesReeducation;
        private static List<ThemeEvaluationModel> themesEvaluation;
        private static ObservableCollection<string> themesFondEvaluation;
        #region Constructors

        #endregion

        #region Properties

        public static List<ThemeModel> ThemesReeducation
        {
            get { return themesReeducation; }
        }

        #endregion

        #region Methods

        public static List<ThemeModel> LoadAllReeducationTheme()
        {
            return themesReeducation = ThemeData.LoadAllReeducationTheme();
        }

        public static List<ThemeEvaluationModel> LoadAllEvalTheme(string nomFond)
        {
            return themesEvaluation = ThemeData.LoadAllEvaluationTheme(nomFond);
        }

        public static ObservableCollection<string> LoadAllFondEvalTheme()
        {
            return themesFondEvaluation = ThemeData.LoadAllFondEvaluation();
        }
        public static ObservableCollection<string> LoadDefaultFondEvalTheme()
        {
            return themesFondEvaluation = ThemeData.LoadDefaultFondEvaluation();
        }
        #endregion
    }
}

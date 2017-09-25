using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxModel;
using AxData;

namespace AxConfiguration
{
    public static class ExerciceConfig
    {

        public static void InitConfigurationProgramme()
        {
            ConfigProgramme configProg = ConfigData.LoadConfigProgramme();
            Singleton.ConfigProg = configProg;
        }

        #region BorneConfig
        public static ExerciceBorneConfig GetSmallBorne()
        {
            ExerciceBorneConfig smallBorne = FindBorne("Petit");//Faire passer les valeur des petites bornes
            return smallBorne;
        }
        public static ExerciceBorneConfig GetMediumBorne()
        {
            ExerciceBorneConfig mediumBorne = FindBorne("Moyen");//Faire passer les valeur des medium bornes
            return mediumBorne;
        }
        public static ExerciceBorneConfig GetBigBorne()
        {
            ExerciceBorneConfig bigBorne = FindBorne("Grand");//Faire passer les valeur des grandes bornes
            return bigBorne;
        }

        private static ExerciceBorneConfig FindBorne(string taille)
        {
            List<ExerciceBorneConfig> listeBornes = ConfigData.LoadAllBorneConfig();
            return listeBornes.Find(b => b.TailleBorne == taille);
        }
        #endregion

        #region BaseConfig
        public static ExerciceBaseConfig GetBaseConfig()
        {
            ExerciceBaseConfig baseConf = ConfigData.LoadExerciceBaseConfig();
            return baseConf;
        }

        public static ExerciceBaseConfig GetBaseConfigPourExercice(string nomJeu)
        {
            return ConfigData.LoadLastConfigPourExercice(nomJeu);
        }
        #endregion
    }
}

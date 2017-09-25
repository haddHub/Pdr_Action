using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public enum ExerciceTypes
    {
        Evaluation,
        Jeu,
        XDent
    }
    public class ExerciceGeneric
    {
        public ExerciceBaseConfig BaseConfig { get; set; }
        public ExerciceBorneConfig BorneConfig { get; set; }
        public ThemeModel Theme { get; set; }
        public ExerciceTypes TypeExercice { get; set; }
        public ExerciceGeneric(ExerciceBaseConfig baseConf,ExerciceBorneConfig borneConf, ThemeModel theme)
        {
            BaseConfig = baseConf;
            BorneConfig = borneConf;
            Theme = theme;
        }
        public ExerciceGeneric()
        {

        }
    }
}

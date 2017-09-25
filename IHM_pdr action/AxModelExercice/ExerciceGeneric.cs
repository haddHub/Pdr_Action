using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxModel;
using GalaSoft.MvvmLight.Messaging;

namespace AxModelExercice
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
        public ExerciceTypes TypeExercice { get; set; }
        public ExerciceGeneric(ExerciceBaseConfig baseConf,ExerciceBorneConfig borneConf)
        {
            BaseConfig = baseConf;
            BorneConfig = borneConf;
        }
        public ExerciceGeneric()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxModel;

namespace AxModelExercice
{
    public class Tonus : ExerciceMouvement
    {
        public Tonus(ExerciceBaseConfig baseConf, ExerciceBorneConfig borneConf,ThemeEvaluationModel theme)
            : base(baseConf,borneConf,theme)
        {
            this.TypeDroite = DroiteType.Tonus;
        }
    }
}

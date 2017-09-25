using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class ExerciceReeducation : ExerciceGeneric
    {
        public byte Difficulte { get; set; }

        public ExerciceReeducation(ExerciceBaseConfig baseConf, ExerciceBorneConfig borneConf, ThemeModel theme)
            : base(baseConf,borneConf,theme)
        {
            this.TypeExercice = ExerciceTypes.Jeu;
        }
    }
}

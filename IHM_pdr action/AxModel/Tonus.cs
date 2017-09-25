using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class Tonus : ExerciceMouvement
    {
        public Tonus(ExerciceBaseConfig baseConf, ExerciceBorneConfig borneConf,ThemeModel theme)
            : base(baseConf,borneConf,theme)
        {
            this.TypeDroite = DroiteType.Tonus;
        }
    }
}

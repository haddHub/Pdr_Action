using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace AxModel
{
    public class UiChasseurModel : UiCibleModel
    {
        public UiChasseurModel()
            : base()
        {

        }

        public UiChasseurModel(string na, UIElement sh)
        {
            this.Name = na;
            this.Shape = sh;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace AxModel
{
    public class CibleCheckBox : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets la coordonée x pour la résolution de l'affichage dans l'interface thérapeute.
        /// </summary>
        public int XAffichage { get; set; }

        /// <summary>
        /// Gets or sets la coordonée y pour la résolution de l'affichage dans l'interface thérapeute.
        /// </summary>
        public int YAffichage { get; set; }

        private bool selected;
        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                OnPropertyChanged(_IsCheckedEventArgs);
            }
        }

        public int Number { get; set; }

        public CibleCheckBox(int x, int y, bool selected, int num)
        {
            XAffichage = x;
            YAffichage = y;
            Selected = selected;
            Number = num;
        }

        static readonly PropertyChangedEventArgs _IsCheckedEventArgs = new PropertyChangedEventArgs("Selected");

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, eventArgs);
            }
        }
    }
}

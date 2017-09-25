using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace AxModel
{
    public class CheckBoxEvolution : INotifyPropertyChanged
    {
        public string Text { get; set; }
        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set 
            { 
                isChecked = value;
                OnPropertyChanged(_IsCheckedEventArgs);
            }
        }
        public ChartsType Type { get; set; }
        public ChartsParam Param { get; set; }
        public CheckBoxEvolution(string txt, bool isChecked, ChartsType type, ChartsParam param)
        {
            this.Text = txt;
            this.IsChecked = isChecked;
            this.Type = type;
            this.Param = param;
        }

        static readonly PropertyChangedEventArgs _IsCheckedEventArgs = new PropertyChangedEventArgs("IsChecked");

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, eventArgs);
            }
        }
    }

    public enum ChartsType
    {
        DiscretSimple,
        DiscretComplexe,
        Cyclique,
        Carre,
        Cercle,
        Droite,
        Cible
    }

    public enum ChartsParam
    {
        Vitesse,
        CVVitesse,
        Init,
        CVInit,
        AssisLat,
        CVAssisLat,
        AssisLong,
        CVAssisLong,
        NbMouvements,
        Souplesse,
        CVSouplesse,
        Amplitude,
        CVAmplitude,
        Linearite,
        CVLinearite,
        Precision,
        CVPrecision
    }
}

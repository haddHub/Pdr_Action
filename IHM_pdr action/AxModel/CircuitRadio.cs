using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace AxModel
{
    public class CircuitRadio : INotifyPropertyChanged
    {
        public string Text { get; set; }

        private bool _checked;

        public bool Checked
        {
            get { return _checked; }
            set 
            {
                _checked = value;
                OnPropertyChanged(_IsCheckedEventArgs);
            }
        }

        private bool _activated;

        public bool Activated
        {
            get { return _activated; }
            set 
            {
                _activated = value;
                OnPropertyChanged(_IsActivatedEventArgs);
            }
        }


        public CircuitRadio(string txt)
        {
            this.Text = txt;
            this.Checked = false;
            this.Activated = true;
        }

        static readonly PropertyChangedEventArgs _IsCheckedEventArgs = new PropertyChangedEventArgs("Checked");
        static readonly PropertyChangedEventArgs _IsActivatedEventArgs = new PropertyChangedEventArgs("Activated");

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

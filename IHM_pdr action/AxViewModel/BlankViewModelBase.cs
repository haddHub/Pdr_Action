using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;

namespace AxViewModel
{
    //Cette classe sert à pouvoir effectuer des retours pour la navigation
    public class BlankViewModelBase : ViewModelBase
    {
        #region properties

        private bool _isRetour;
        public bool IsRetour
        {
            get { return _isRetour; }
            set
            {
                _isRetour = value;
                RaisePropertyChanged(() => IsRetour);
            }
        }
        #endregion

        #region public

        public void SetIsRetour(bool value)
        {
            IsRetour = value;
        }
        #endregion
    }
}

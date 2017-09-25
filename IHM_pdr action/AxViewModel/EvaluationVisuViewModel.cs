using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxViewModel
{
    public class EvaluationVisuViewModel : WizardPageViewModelBase
    {
        #region Fields

        #endregion

        #region Constructors
        public EvaluationVisuViewModel():base(null)
        {

        }
 
        #endregion

        #region Properties

        public override string DisplayName
        {
            get { return "Exercice"; }
        }

        internal override bool IsValid()
        {
            return true;
        }

        #endregion

        #region Methods

        #endregion

        #region RelayCommand

        #endregion

        #region Actions

        #endregion
    }
}

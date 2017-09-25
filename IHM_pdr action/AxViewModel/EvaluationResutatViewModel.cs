using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxView;
using System.Windows;

namespace AxViewModel
{
    class EvaluationResutatViewModel : WizardPageViewModelBase
    {
        #region Fields

        private const string ExFreeAPropertyName = "ExFreeA";
        private bool _exFreeA = false;
        private const string ExTargetPropertyName = "ExTarget";
        private bool _exTarget = false;
        private const string ExSquarePropertyName = "ExSquare";
        private bool _exSquare = false;
        private const string ExCirclePropertyName = "ExCircle";
        private bool _exCircle = false;

        private const string VisiChTargPropertyName = "VisiChTarg";
        private Visibility _visiChTarg = Visibility.Visible;
        private const string VisiChSquaPropertyName = "VisiChSqua";
        private Visibility _visiChSqua = Visibility.Visible;
        private const string VisiChCirPropertyName = "VisiChCir";
        private Visibility _visiChCir = Visibility.Visible;

        #endregion

        #region Constructors
        public EvaluationResutatViewModel(): base(null)
        {
        }

        #endregion

        #region Properties

        public bool ExFreeA
        {
            get { return _exFreeA; }
            set
            {
                if (_exFreeA == value)
                {
                    _exFreeA = value;
                }

                RaisePropertyChanging(ExFreeAPropertyName);
                _exFreeA = value;
                RaisePropertyChanged(ExFreeAPropertyName);
                if (ExFreeA == true)
                {
                    ExTarget = false;
                    ExSquare = false;
                    ExCircle = false;
                    VisiChTarg = Visibility.Hidden;
                    VisiChSqua = Visibility.Hidden;
                    VisiChCir = Visibility.Hidden;
                }
                else
                {
                    VisiChTarg = Visibility.Visible;
                    VisiChSqua = Visibility.Visible;
                    VisiChCir = Visibility.Visible;
                }
            }
        
        }

        public bool ExTarget
        {
            get { return _exTarget; }
            set
            {
                if (_exTarget == value)
                {
                    _exTarget = value;
                }

                RaisePropertyChanging(ExTargetPropertyName);
                _exTarget = value;
                RaisePropertyChanged(ExTargetPropertyName);
                if (ExTarget == true)
                {
                    ExFreeA = false;
                    ExSquare = false;
                    ExCircle = false;
                    VisiChSqua = Visibility.Hidden;
                    VisiChCir = Visibility.Hidden;
                }
                else
                {
                    VisiChSqua = Visibility.Visible;
                    VisiChCir = Visibility.Visible;
                }
            }
        }

        public bool ExSquare
        {
            get { return _exSquare; }
            set
            {
                if (_exSquare == value)
                {
                    _exSquare = value;
                }

                RaisePropertyChanging(ExSquarePropertyName);
                _exSquare = value;
                RaisePropertyChanged(ExSquarePropertyName);
                if (ExSquare == true)
                {
                    ExFreeA = false;
                    ExTarget = false;
                    ExCircle = false;
                    VisiChCir = Visibility.Hidden;
                }
                else
                {
                    VisiChCir = Visibility.Visible;
                }
            }
        }

        public bool ExCircle
        {
            get { return _exCircle; }
            set
            {
                if (_exCircle == value)
                {
                    _exCircle = value;
                }

                RaisePropertyChanging(ExCirclePropertyName);
                _exCircle = value;
                RaisePropertyChanged(ExCirclePropertyName);
                if (ExCircle == true)
                {
                    ExFreeA = false;
                    ExTarget = false;
                    ExSquare = false;
                }
            }
        }

        public Visibility VisiChTarg
        {
            get { return _visiChTarg; }
            set
            {
                if (_visiChTarg == value)
                {
                    _visiChTarg = value;
                }

                RaisePropertyChanging(VisiChTargPropertyName);
                _visiChTarg = value;
                RaisePropertyChanged(VisiChTargPropertyName);
            }
        }

        public Visibility VisiChSqua
        {
            get { return _visiChSqua; }
            set
            {
                if (_visiChSqua == value)
                {
                    _visiChSqua = value;
                }

                RaisePropertyChanging(VisiChSquaPropertyName);
                _visiChSqua = value;
                RaisePropertyChanged(VisiChSquaPropertyName);
            }
        }

        public Visibility VisiChCir
        {
            get { return _visiChCir; }
            set
            {
                if (_visiChCir == value)
                {
                    _visiChCir = value;
                }

                RaisePropertyChanging(VisiChCirPropertyName);
                _visiChCir = value;
                RaisePropertyChanged(VisiChCirPropertyName);
            }
        }



        public override string DisplayName
        {
            get { return "Resultat"; }
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

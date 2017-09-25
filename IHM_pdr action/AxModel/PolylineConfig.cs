using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace AxModel
{
    public class PolylineConfig
    {
        #region Fields

        private bool _visibility;
        private double _taille;
        private string _selectedStyle;
        private Brush _couleur;

        private ObservableCollection<string> _getStyle;
        private DoubleCollection _RobotBaseStyle;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the PolylineConfig class.
        /// </summary>
        public PolylineConfig()
        {
            _visibility = true;
            _taille = 30.0;
            _selectedStyle = "Continu";
            _couleur = Brushes.Gray;

            _getStyle = new ObservableCollection<string>();
            _getStyle.Add("Continu");
            _getStyle.Add("Discontinu");
            _RobotBaseStyle = new DoubleCollection(2);
            _RobotBaseStyle.Add(0.0);
            _RobotBaseStyle.Add(0.0);
            StyleConverter();
        }

        /// <summary>
        /// Initializes a new instance of the PolylineConfig class.
        /// </summary>
        /// <param name="vi">Affiche ou pas la ligne</param>
        /// <param name="ta">Taille de la ligne</param>
        /// <param name="st">Style de la ligne</param>
        /// <param name="co">Couleur de la ligne</param>
        public PolylineConfig(bool vi, double ta, string st, Brush co)
        {
            _visibility = vi;
            _taille = ta;
            _selectedStyle = st;
            _couleur = co;
            _getStyle = new ObservableCollection<string>();
            _getStyle.Add("Continu");
            _getStyle.Add("Discontinu");
            _RobotBaseStyle = new DoubleCollection(2);
            _RobotBaseStyle.Add(0.0);
            _RobotBaseStyle.Add(0.0);
            StyleConverter();
        }

        #endregion

        #region Properties

        public bool Visibility
        {
            get
            {
                return _visibility;
            }
            set
            {
                _visibility = value;
            }
        }

        public double Taille
        {
            get
            {
                return _taille;
            }
            set
            {
                _taille = value;
            }
        }

        public ObservableCollection<string> GetStyleCollection()
        {
            return _getStyle;
        }

        public string SelectedStyle
        {
            get
            {
                return _selectedStyle;
            }
            set
            {
                _selectedStyle = value;
                StyleConverter();
            }
        }

        public Brush Couleur
        {
            get
            {
                return _couleur;
            }
            set
            {
                _couleur = value;
            }
        }

        #endregion

        #region Methods

        public DoubleCollection StyleConverter()
        {
            if (SelectedStyle == "Continu")
            {
                _RobotBaseStyle[0] = 1.0;
                _RobotBaseStyle[1] = 0.0;
            }
            else if (SelectedStyle == "Discontinu")
            {
                _RobotBaseStyle[0] = 1.0;
                _RobotBaseStyle[1] = 1.0;
            }
            else
            {
                throw new Exception("Erreur Style");
            }
            return _RobotBaseStyle;
        }

        #endregion

        #region RelayCommand

        #endregion

        #region Actions

        #endregion
    }
}

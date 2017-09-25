using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using GalaSoft.MvvmLight;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.IO;

namespace AxModel
{
    public class UiCibleModel : CibleModel
    {
        #region Fields

        private UIElement _shape;
        /// <summary>
        /// The X coordinate of the location of the rectangle (in content coordinates).
        /// </summary>
        private double _x;

        /// <summary>
        /// The Y coordinate of the location of the rectangle (in content coordinates).
        /// </summary>
        private double _y;

        /// <summary>
        /// The width of the rectangle (in content coordinates).
        /// </summary>
        private double _width;

        /// <summary>
        /// The height of the rectangle (in content coordinates).
        /// </summary>
        private double _height;

        private Geometry _geometry;
        private TranslateTransform _transform;

        #endregion

        #region Constructors

        public UiCibleModel()
            : base()
        {

        }

        public UiCibleModel(string na, UIElement sh)
        {
            this.Name = na;
            this.Shape = sh;
        }

        #endregion

        #region Properties

        new public UIElement Shape  // remplace la méthode héritée !
        {
            get
            {
                return this._shape;
            }
            set
            {
                this._shape = value;
                RaisePropertyChanged("Shape");
            }
        }

        // A changer ! dans MainPViewModel

        /// <summary>
        /// The X coordinate of the location of the rectangle (in content coordinates).
        /// </summary>
        public double X
        {
            get
            {
                return _x;
            }
            set
            {
                if (_x == value)
                {
                    return;
                }

                _x = value;

                RaisePropertyChanged("X");
            }
        }

        /// <summary>
        /// The Y coordinate of the location of the rectangle (in content coordinates).
        /// </summary>
        public double Y
        {
            get
            {
                return _y;
            }
            set
            {
                if (_y == value)
                {
                    return;
                }

                _y = value;

                RaisePropertyChanged("Y");
            }
        }

        /// <summary>
        /// The width of the rectangle (in content coordinates).
        /// </summary>
        public double Width
        {
            get
            {
                return _width;
            }
            set
            {
                if (_width == value)
                {
                    return;
                }

                _width = value;

                RaisePropertyChanged("Width");
            }
        }

        /// <summary>
        /// The height of the rectangle (in content coordinates).
        /// </summary>
        public double Height
        {
            get
            {
                return _height;
            }
            set
            {
                if (_height == value)
                {
                    return;
                }

                _height = value;

                RaisePropertyChanged("Height");
            }
        }

        public Geometry Geometry
        {
            get
            {
                if (_geometry == null)
                {
                    _geometry = this.Shape.Clip.Clone();
                    _transform = new TranslateTransform();
                    _geometry.Transform = _transform;
                }
                _transform.X = this.X;
                _transform.Y = this.Y;
                return _geometry;
            }
        }

        #endregion

        #region Methods

        #endregion
    }
}

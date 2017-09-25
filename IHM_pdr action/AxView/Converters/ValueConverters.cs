using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows;
using AxViewModel;
using System.Windows.Media;

namespace AxView.Converters
{
    #region XIsHighEnoughValueConverter
    /// <summary>
    /// 
    /// </summary>
    public class XIsHighEnoughValueConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((double)value) > 29.0;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }

    #endregion

    #region YIsHighEnoughValueConverter
    /// <summary>
    /// 
    /// </summary>
    public class YIsHighEnoughValueConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((double)value) > 4.0;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }

    #endregion

    #region DoubleToIntegerConverter
    /// <summary>
    /// DoubleToIntegerValueConverter provides a two-way conversion between
    /// a double value and an integer.
    /// </summary>
    public class DoubleToIntegerValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return System.Convert.ToInt32(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return System.Convert.ToDouble(value);
        }

    }

    /// <summary>
    /// IntegerToDoubleValueConverter provides a two-way conversion between
    /// a integer value and an double.
    /// </summary>
    [ValueConversion(typeof(double), typeof(int))]
    public class IntegerToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value);
            //throw new NotSupportedException("Cannot convert back");
        }
    }

    #endregion // DoubleToIntegerConverter

    #region AbsoluteConverter
    /// <summary>
    /// 
    /// </summary>
    public class AbsoluteConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double actualValue = System.Convert.ToDouble(value);
            return Math.Abs(actualValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("This will never be called as our Binding is OneWay");
        }

        #endregion
    }

    #endregion

    #region FormeConverter
    /// <summary>
    /// 
    /// </summary>
    public class FormeConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double actualValue = System.Convert.ToDouble(value);
            return actualValue + 40.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("This will never be called as our Binding is OneWay");
        }

        #endregion
    }

    #endregion

    #region DotXConverter
    /// <summary>
    /// 
    /// </summary>
    public class DotXConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            PointCollection actualValue = (PointCollection)value;
            if (actualValue.Count != 0)
            {
                return actualValue[actualValue.Count - 1].X - 15.0;
            }
            else
            {
                return 0.0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("This will never be called as our Binding is OneWay");
        }

        #endregion
    }

    #endregion

    #region DotYConverter
    /// <summary>
    /// 
    /// </summary>
    public class DotYConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            PointCollection actualValue = (PointCollection)value;
            if (actualValue.Count != 0)
            {
                return actualValue[actualValue.Count - 1].Y - 15.0;
            }
            else
            {
                return 0.0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("This will never be called as our Binding is OneWay");
        }

        #endregion
    }

    #endregion

    #region DotXCenterConverter
    /// <summary>
    /// 
    /// </summary>
    public class DotXCenterConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double actualValue = (double)value;

            return actualValue - 15.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("This will never be called as our Binding is OneWay");
        }

        #endregion
    }

    #endregion

    #region DotYCenterConverter
    /// <summary>
    /// 
    /// </summary>
    public class DotYCenterConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double actualValue = (double)value;

            return actualValue - 15.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("This will never be called as our Binding is OneWay");
        }

        #endregion
    }

    #endregion

    #region LineXConverter
    /// <summary>
    /// 
    /// </summary>
    public class LineXConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double actualValue = System.Convert.ToDouble(value);
            return actualValue * 4.333333333333333;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("This will never be called as our Binding is OneWay");
        }

        #endregion
    }

    #endregion

    #region LineYConverter
    /// <summary>
    /// 
    /// </summary>
    public class LineYConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double actualValue = System.Convert.ToDouble(value);
            return actualValue * 4.190740740740741;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("This will never be called as our Binding is OneWay");
        }

        #endregion
    }

    #endregion

    #region ArrowMultiValueConverter
    /// <summary>
    /// 
    /// </summary>
    public class ArrowMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double actualPosition = System.Convert.ToDouble(values[0]);
            double actualForce = System.Convert.ToDouble(values[1]);
            return actualPosition + actualForce - 100.0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("This will never be called as our Binding is OneWay");
        }
    }

    #endregion

    #region EllipseMultiValueConverter
    /// <summary>
    /// 
    /// </summary>
    public class EllipseMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double actualPositionX = System.Convert.ToDouble(values[0]);
            double actualPositionY = System.Convert.ToDouble(values[1]);
            //actualPositionX = (actualPositionX) / 4.89445;
            //actualPositionY = (actualPositionY) / 4.73333;
            //return "X: " + actualPositionX + "\nY: " + actualPositionY ;
            return "X: " + actualPositionX.ToString("0.###") + "\nY: " + actualPositionY.ToString("0.###");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("This will never be called as our Binding is OneWay");
        }
    }

    #endregion

    #region BoolToVisibilityConverter
    /// <summary>
    /// 
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var v = (bool)value;
                return v ? Visibility.Visible : Visibility.Collapsed;
            }
            catch (InvalidCastException)
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region BoolToBoolNotConverter
    /// <summary>
    /// 
    /// </summary>
    public class BoolToBoolNotConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                bool v = (bool)value;
                return !v;
            }
            catch (InvalidCastException)
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region PortValueConverter
    /// <summary>
    /// 
    /// </summary>
    public class PortValueConverter : IValueConverter
    {
        public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
        {
            bool etat = System.Convert.ToBoolean(values);
            return (etat ? "Connecté" : "Déconnecté");
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("This will never be called as our Binding is OneWay");
        }
    }

    #endregion

    #region ViewMultiValueConverter
    /// <summary>
    /// 
    /// </summary>
    public class ViewMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double actualPositionX = System.Convert.ToDouble(values[0]);
            double actualPositionY = System.Convert.ToDouble(values[1]);
            double actualRaidLat = System.Convert.ToDouble(values[2]);
            double actualRaidLong = System.Convert.ToDouble(values[3]);
            //actualPositionX = (actualPositionX) / 4.89445;
            //actualPositionY = (actualPositionY) / 4.73333;
            //return "X: " + actualPositionX + "\nY: " + actualPositionY ;
            return "X: " + actualPositionX.ToString("0.###") + " Y: " + actualPositionY.ToString("0.###") + " Couple X : " + actualRaidLat.ToString("0.###") + " CoupleY : " + actualRaidLong.ToString("0.###");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("This will never be called as our Binding is OneWay");
        }
    }

    #endregion

    #region SliderValueConverter
    public class SliderValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            else
                return Math.Round(System.Convert.ToDouble(value.ToString()));

        }



        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            throw new NotImplementedException();

        }

    }
    #endregion

    #region RadioButtonConverter
    public class RadioButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value.ToString() == parameter.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    #endregion

    #region ColorConverter
    /// <summary>
    /// Retourne une Couleur
    /// </summary>
    public class ColorConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                bool colorValue = (bool)value;
                if (colorValue == true)
                {
                    return "Red";
                }
                else if (colorValue == false)
                {
                    return "Green";
                }
                else
                {
                    return "Black";
                }
            }
            catch
            {
                return "Black";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}

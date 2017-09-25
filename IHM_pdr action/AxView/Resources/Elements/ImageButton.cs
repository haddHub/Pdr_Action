using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace AxView.Resources.Elements
{
    class ImageButton : Button
    {
        Image _image = null;
        TextBlock _textBlock = null;
        StackPanel panel = null;

        public ImageButton()
        {
            panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;

            panel.Margin = new System.Windows.Thickness(10);
            //panel.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            //panel.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            _image = new Image();
            _image.Margin = new System.Windows.Thickness(0, 0, 10, 0);
            //_image.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            //_image.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            panel.Children.Add(_image);

            _textBlock = new TextBlock();
            panel.Children.Add(_textBlock);

            this.Content = panel;
        }

        public string Text
        {
            get
            {
                if (_textBlock != null)
                    return _textBlock.Text;
                else
                    return String.Empty;
            }
            set
            {
                if (_textBlock != null)
                    _textBlock.Text = value;
            }
        }

        public ImageSource Image
        {
            get
            {
                if (_image != null)
                    return _image.Source;
                else
                    return null;
            }
            set
            {
                if (_image != null)
                    _image.Source = value;
            }
        }

        public double ImageWidth
        {
            get
            {
                if (_image != null)
                    return _image.Width;
                else
                    return double.NaN;
            }
            set
            {
                if (_image != null)
                    _image.Width = value;
            }
        }

        public double ImageHeight
        {
            get
            {
                if (_image != null)
                    return _image.Height;
                else
                    return double.NaN;
            }
            set
            {
                if (_image != null)
                    _image.Height = value;
            }
        }

        public Orientation ImageOrientation
        {
            get
            {
                return panel.Orientation;
            }
            set
            {
                panel.Orientation = value;
            }
        }
    }
}

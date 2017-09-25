using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;

namespace AxModel
{
    public class EntiteeModel : ViewModelBase  // TODO : à changer
    {
        #region Fields

        private string _shape;
        private string _name;

        #endregion

        #region Constructors

        public EntiteeModel()
        {
            this._shape = "\\Resources\\black_flower.png";
            this._name = "test";
        }

        public EntiteeModel(string sh, string na)
        {
            this._shape = sh;
            this._name = na;
        }

        #endregion

        #region Properties

        public string Shape
        {
            get
            {
                return this._shape;
            }
            set
            {
                this._shape = value;
            }
        }

        public string Name
        {
            get { return this._name; }
            set
            {
                if (value != this._name)
                {
                    this._name = value;
                }
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public enum Genre
    {
        Futuriste,
        Nature
    }

    public class ThemeModel
    {
        #region Fields

        private string _name;
        private CibleModel _cible;
        private CibleModel _cibleD;
        private ChasseurModel _chasseur;
        private string _fond;
        private string _fondD;
        private string _type;

        #endregion

        #region Constructors

        public ThemeModel()
        {
            this._name = "UnnamedTheme";
            this.Fond = "Resources\\Image\\Background\\Axi_FondEcran_NT.png";
        }
        public ThemeModel(string na, CibleModel ci, CibleModel cid, ChasseurModel ch, string fo, string fod, string ge)
        {
            this._name = na;
            this._cible = ci;
            this._cibleD = cid;
            this._chasseur = ch;
            this._fond = fo;
            this._fondD = fod;
            this._type = ge;
        }

        public ThemeModel(ThemeModel tm)
        {
            this._name = tm.Name;
            this._cible = tm.Cible;
            this._cibleD = tm.CibleD;
            this._chasseur = tm.Chasseur;
            this._fond = tm.Fond;
            this._fondD = tm.FondD;
            this._type = tm.Type;
        }

        //themes avec uniquement le background pour les jeux flash.
        public ThemeModel(string background)
        {
            this._fond = background;
        }
        #endregion

        #region Properties

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public CibleModel Cible
        {
            get
            {
                return _cible;
            }
            set
            {
                _cible = value;
            }
        }

        public CibleModel CibleD
        {
            get
            {
                return _cibleD;
            }
            set
            {
                _cibleD = value;
            }
        }

        public ChasseurModel Chasseur
        {
            get
            {
                return _chasseur;
            }
            set
            {
                _chasseur = value;
            }
        }

        public string Fond
        {
            get
            {
                return _fond;
            }
            set
            {
                _fond = value;
            }
        }

        public string FondD
        {
            get
            {
                return _fondD;
            }
            set
            {
                _fondD = value;
            }
        }

        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }
        #endregion
    }
}

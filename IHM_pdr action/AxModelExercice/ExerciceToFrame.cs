using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxModel;

namespace AxModelExercice
{
    public class ExerciceToFrame
    {
        public static FrameExerciceDataModel ExercicesEvaluationToFrameExercice(ExerciceEvaluation exercice, int i)
        {
            FrameExerciceDataModel _exFrame = new FrameExerciceDataModel();

            if (exercice.TypeEval == ExerciceEvalTypes.Forme)
            {
                ExerciceForme temps = (ExerciceForme)exercice;
                _exFrame.Address = ConfigAddresses.Formes;

                switch (temps.TypeForme) // polygones
                {
                    case FormeType.Carré: _exFrame.Data1 = 0x04;
                        break;
                    case FormeType.Cercle: _exFrame.Data1 = 0x28;   // 0x1E
                        break;
                    case FormeType.Triangle: _exFrame.Data1 = 0x06; // Hexagone
                        break;
                }
                _exFrame.Data2 = (byte)(4 * temps.Taille);
                _exFrame.Data3 = temps.Origine;  // TODO : peut etre ?
                _exFrame.Data4 = Convert.ToByte(temps.AllerRetour);
            }
            else
            {
                //if (exercice.Exercice[i].ExerciceType == ExerciceTypes.Cibles)
                //{
                //    ExerciceXDent temps = (ExerciceXDent)exercice.Exercice[i];
                //    _exFrame.Address = ConfigAddresses.Cibles;
                //    _exFrame.Data1 = temps.NbrsCibles;
                //    _exFrame.Data2 = temps.DistCibles;
                //    _exFrame.Data3 = temps.Anglecible;
                //    //_exFrame.Data4 = temps.Origine;  // TODO : peut etre ?
                ////}
                //else
                //{
                    //if (exercice.TypeExercice == ExerciceTypes.Mouvements_Complexes)
                    //{
                    //    ExerciceMouvement temps = (ExerciceMouvement)exercice.Exercice[i];
                    //    _exFrame.Address = ConfigAddresses.Mouvements;

                    //    switch (temps.DroiteType)
                    //    {
                    //        case DroiteType.Horizontal: _exFrame.Data1 = 0x02;
                    //            break;
                    //        case DroiteType.Oblique: _exFrame.Data1 = 0x03;
                    //            break;
                    //        case DroiteType.Vertical: _exFrame.Data1 = 0x01;
                    //            break;
                    //        default: _exFrame.Data1 = 0x01; // TODO : Erreur !
                    //            break;
                    //    }

                    //    _exFrame.Data2 = temps.PositionDroite;
                    //}
                    //else
                    //{
                        if (exercice.TypeEval == ExerciceEvalTypes.Mouvement)
                        {
                            ExerciceMouvement temps = (ExerciceMouvement)exercice;
                            _exFrame.Address = ConfigAddresses.Mouvements;  // TODO : faire un mod spécifique !

                            switch (temps.TypeDroite)
                            {
                                case DroiteType.Vertical: _exFrame.Data1 = 0x01;
                                    break;
                                case DroiteType.VerticalLong: _exFrame.Data1 = 0x04;
                                    break;
                                case DroiteType.Tonus: _exFrame.Data1 = 0x05; // TONUS !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                                    break;
                                default: _exFrame.Data1 = 0x01; // TODO : Erreur !
                                    break;
                            }

                            _exFrame.Data2 = temps.PositionDroite;
                        }
                        else
                        {
                            // TODO : Erreur !
                        }
                    //}
                //}
            }
            return _exFrame;
        }

        internal static FrameConfigDataModel MasseViscoToFrame(ExerciceBaseConfig newExerciceConf)
        {
            FrameConfigDataModel _configFrame = new FrameConfigDataModel();
            _configFrame.Address = ConfigAddresses.MasseVisco;
            _configFrame.Data1_2 = newExerciceConf.Masse;
            _configFrame.Data3_4 = newExerciceConf.Viscosite;
            return _configFrame;
        }

        public static FrameConfigDataModel RlatRlongToFrame(ExerciceBaseConfig newExerciceConf)
        {
            FrameConfigDataModel _configFrame = new FrameConfigDataModel();
            _configFrame.Address = ConfigAddresses.KlatClong;
            _configFrame.Data1_2 = newExerciceConf.RaideurLat;
            _configFrame.Data3_4 = newExerciceConf.RaideurLong;
            return _configFrame;
        }

        public static FrameConfigDataModel VitesseNbrRepToFrame(ExerciceBaseConfig newExerciceConf)
        {
            FrameConfigDataModel _configFrame = new FrameConfigDataModel();
            _configFrame.Address = ConfigAddresses.VitesseNbrsrep;
            _configFrame.Data1_2 = newExerciceConf.Vitesse;
            _configFrame.Data3_4 = newExerciceConf.NbrRep;
            return _configFrame;
        }

        public static FrameExerciceDataModel BorneToFrame(ExerciceBorneConfig newExerciceConf)
        {
            FrameExerciceDataModel _configFrame = new FrameExerciceDataModel();
            _configFrame.Address = ConfigAddresses.BorneCirc;
            _configFrame.Data1 = (byte)newExerciceConf.BorneG;
            _configFrame.Data2 = (byte)newExerciceConf.BorneD;
            _configFrame.Data3 = 0x01;
            _configFrame.Data4 = (byte)newExerciceConf.BorneArc_H;
            return _configFrame;
        }
    }
}

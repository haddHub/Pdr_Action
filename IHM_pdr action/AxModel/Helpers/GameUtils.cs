using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;

namespace AxModel.Helpers
{
    public static class GameUtils
    {
        private static Random _randomNbr = new Random();

        public static int RandomNumber(int begin, int end)
        {
            return _randomNbr.Next(begin, end);
        }

        public static Point RandomCircleNumber(Point centerPoint, double BorneXMin, double BorneXMax, double BorneYMin, double BorneYMax)
        {
            Point result = new Point(0, 0);
            double angle = ((double)(_randomNbr.Next(0, 6283)) / 1000.0);   // between 0 and 2 * PI, angle is in radians
            //int distance = (int)EchelleUtils.MiseEchelle(10);
            int distance = 11;  // cm
            try
            {
                result.Y = centerPoint.Y + (int)Math.Round(distance * Math.Sin(angle));

                if ((result.Y > BorneYMax) || (result.Y < BorneYMin))
                {
                    result.Y = centerPoint.Y - (int)Math.Round(distance * Math.Sin(angle));
                }
                if ((result.Y > BorneYMax) || (result.Y < BorneYMin))
                {
                    result = testBorneY(ref result, ref centerPoint, ref distance, ref angle, ref BorneYMin, ref BorneYMax); // test recursif !
                }

                result.X = centerPoint.X + (int)Math.Round(distance * Math.Cos(angle));

                if ((result.X > BorneXMax) || (result.X < BorneXMin))
                {
                    result.X = centerPoint.X - (int)Math.Round(distance * Math.Cos(angle));
                }

                //Debug.Print("X: " + result.X.ToString() + " Y: " + result.Y.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message, "Erreur !", MessageBoxButton.OK, MessageBoxImage.Error);
                //Debug.Print("Erreur génération cible patient!" + ex);
            }
                return result;
        }

        /// <summary>
        /// testBorneY recursif
        /// </summary>
        /// <param name="result"></param>
        /// <param name="centerPoint"></param>
        /// <param name="distance"></param>
        /// <param name="angle"></param>
        /// <param name="BorneYMin"></param>
        /// <param name="BorneYMax"></param>
        /// <returns></returns>
        private static Point testBorneY(ref Point result,ref Point centerPoint,ref int distance,ref double angle,ref double BorneYMin,ref double BorneYMax)
        {
            angle += Math.PI / 4.0;
            result.Y = centerPoint.Y - (int)Math.Round(distance * Math.Sin(angle));

            if ((result.Y < BorneYMax) && (result.Y > BorneYMin))
            {
                Debug.Print("Gen X: " + result.X.ToString() + "Gen Y: " + result.Y.ToString());
                return result;
            }
            else
            {
                return testBorneY(ref result, ref centerPoint, ref distance, ref angle, ref BorneYMin, ref BorneYMax);
            }
        }

        public static bool CheckCollision(UiChasseurModel ctl1, UiCibleModel ctl2, double detectX, double detectY)
        {
            bool retval = false;
            //Point ptTopLeft = new Point(ctl1.X, ctl1.Y);    // TODO : changer ! enlever la moitier de la taille !
            //Point ptBottomRight = new Point(ctl1.X + ctl1.Width - 175.0, ctl1.Y + ctl1.Height - 175.0); // 90
            //Rect r1 = new Rect(ptTopLeft, ptBottomRight);
            Size sizeR1 = new Size(20.0, 20.0);
            Point centerR1 = new Point(ctl1.X + ((ctl1.Width / 2.0) - (sizeR1.Width / 2.0)), ctl1.Y + ((ctl1.Height / 2.0) - (sizeR1.Height / 2.0)));
            Rect r1 = new Rect(centerR1, sizeR1);

            //System.Diagnostics.Debug.WriteLine("+++x:" + ptTopLeft.X.ToString() + " Y " + ptTopLeft.Y.ToString() + " " + ptBottomRight.X.ToString() + " " + ptBottomRight.Y.ToString());

            //Point ptTopLeft2 = new Point(ctl2.X, ctl2.Y);   // TODO : changer ! enlever la moitier de la taille !
            //Point ptBottomRight2 = new Point(ctl2.X + ctl2.Width - 175.0, ctl2.Y + ctl2.Height - 175.0);
            //Rect r2 = new Rect(ptTopLeft2, ptBottomRight2);
            Size sizeR2 = new Size(detectX, detectY);   // 40.0
            Point centerR2 = new Point(ctl2.X + ((ctl2.Width / 2.0) - (sizeR2.Width / 2.0)), ctl2.Y + ((ctl2.Height / 2.0) - (sizeR2.Height / 2.0)));
            Rect r2 = new Rect(centerR2, sizeR2);

            r1.Intersect(r2);
            if (!r1.IsEmpty)
            {
                retval = true;
            }
            return retval;
        }

        public static bool CheckCollisionEva(UiChasseurModel ctl1, UiCibleModel ctl2)
        {
            bool retval = false;
            Point ptTopLeft = new Point(ctl1.X, ctl1.Y);
            Point ptBottomRight = new Point(ctl1.X + ctl1.Width - 45, ctl1.Y + ctl1.Height - 45);
            Rect r1 = new Rect(ptTopLeft, ptBottomRight);

            //System.Diagnostics.Debug.WriteLine("+++x:" + ptTopLeft.X.ToString() + " Y " + ptTopLeft.Y.ToString() + " " + ptBottomRight.X.ToString() + " " + ptBottomRight.Y.ToString());

            Point ptTopLeft2 = new Point(ctl2.X, ctl2.Y);
            Point ptBottomRight2 = new Point(ctl2.X + ctl2.Width - 45, ctl2.Y + ctl2.Height - 45);
            Rect r2 = new Rect(ptTopLeft2, ptBottomRight2);

            r1.Intersect(r2);
            if (!r1.IsEmpty)
            {
                retval = true;
            }
            return retval;
        }


        public static bool TestCollision(UiChasseurModel element1, UiCibleModel element2)
        {
            Geometry intersect = Geometry.Combine(element1.Geometry, element2.Geometry, GeometryCombineMode.Intersect, null);

            if (intersect.IsEmpty() == true)
            {
                // no collision - GET OUT!
                return false;
                // return !intersect.IsEmpty();
            }
            else
            {
                //bool bCollision = false;
                Point ptCheck = new Point();

                // now we do a more accurate pixel hit test
                for (int x = Convert.ToInt32(element1.X); x < Convert.ToInt32(element1.X + element1.Width); x++)
                {
                    for (int y = Convert.ToInt32(element1.Y); y < Convert.ToInt32(element1.Y + element1.Height); y++)
                    {
                        ptCheck.X = x;
                        ptCheck.Y = y;

                        //List<UIElement> hits = (List<UIElement>)System.Windows.Media.VisualTreeHelper.FindElementsInHostCoordinates(ptCheck, control1);
                        //HitTestResult hits = System.Windows.Media.VisualTreeHelper.HitTest(element1, ptCheck);
                        //if (hits.VisualHit == controlElem1)
                        //{
                        //    // we have a hit on the first control elem, now see if the second elem has a similar hit
                        //    //List<UIElement> hits2 = (List<UIElement>)System.Windows.Media.VisualTreeHelper.FindElementsInHostCoordinates(ptCheck, control2);
                        //    HitTestResult hits2 = System.Windows.Media.VisualTreeHelper.HitTest(element2, ptCheck);
                        //    if (hits.VisualHit == controlElem2)
                        //    {
                        //        //bCollision = true;
                        //        break;
                        //    }
                        //}
                    }
                    //if (bCollision)
                    // break;
                }
                //return bCollision;
                return true;
            }
        }
    }
}

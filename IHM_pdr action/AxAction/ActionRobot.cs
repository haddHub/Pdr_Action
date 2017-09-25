using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxCommunication;
using AxModel;
using System.Collections.ObjectModel;
using AxError;
namespace AxAction
{
    public class ActionRobot
    {
        public PortSerieService Pss { get; set; }

        public ActionRobot()
        {
            try
            {
                Pss = new PortSerieService();
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }  
        }

        public void Open()
        {
            Pss.Open();
        }
        public void Close()
        {
            Pss.Close();
        }
        public void SendCommandFrame(CommandCodes commandCode)
        {
            Pss.SendCommandFrame(commandCode);
        }
        public void SendConfigFrame(FrameConfigDataModel configData)
        {
            Pss.SendConfigFrame(configData);
        }
        public void SendExerciceFrame(FrameExerciceDataModel exerciceData)
        {
            Pss.SendExerciceFrame(exerciceData);
        }
        public void SendExerciceGameFrame(FrameExerciceDataModel exerciceData)
        {
            Pss.SendExerciceGameFrame(exerciceData);
        }
        public void Dispose()
        {
            Pss.Dispose();
        }
        public ObservableCollection<string> GetPortNameCollection()
        {
            return Pss.GetPortNameCollection();
        }
        public bool IsOpen()
        {
            return Pss.IsOpen();
        }
    }
}

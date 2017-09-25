using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using AxModel;
using System.Windows;

namespace AxCommunication
{
    public delegate void onaXdataReceived(object sender, aXdataModel e);
    public delegate void onErrorDataReceived(object sender, ErrorDataModel e);
	public delegate void onCoupleDataReceived(object sender, CoupleDataModel e);
    public delegate void onPositionDataReceived(object sender, PositionDataModel e);
    public delegate void onPosition2DataReceived(object sender, Position2DataModel e);
    public delegate void onForceDataReceived(object sender, ForceDataModel e);
    public delegate void onForce2DataReceived(object sender, Force2DataModel e);
	public delegate void onAcosTDataReceived(object sender, AcosTDataModel e);
    public delegate void onVitesseDataReceived(object sender, VitesseModel e);
    public delegate void onVitesse2DataReceived(object sender, Vitesse2Model e);
	public delegate void onForceRapDataReceived(object sender, ForceRapDataModel e);
    public delegate void onForceRap2DataReceived(object sender, ForceRap2DataModel e);
    public delegate void onACKDataReceived(object sender, ACKDataModel e);
    public delegate void onStreamACKDataReceived(object sender, StreamAckDataModel e);
    public delegate void onFrameConfigDataReceived(object sender, FrameConfigDataModel e);
	public delegate void onFrameExerciceDataReceived(object sender, FrameExerciceDataModel e);
    public delegate void onBorneDataReceived(object sender, BorneDataModel e);
    public delegate void onPprDataReceived(object sender, PprDataModel e);

    /// <summary>
    /// This interface defines a interface that will allow
    /// communication with the robot
    /// </summary>
    public interface IPortSerieService
    {
        /// <summary>
        /// Available ports on the computer
        /// </summary>
        ObservableCollection<string> GetPortNameCollection();   // retourne une liste de ports présents sur la machine

        string SelectedPortName { get; set; }   // retourne le port sélectionné

        bool IsOpen();  // vérifie si le port est ouvert

        void Open();    // ouvre le port série

        void Close();   // ferme le port série

        void Dispose();

        void SendCommandFrame(CommandCodes commandCode);

        void SendConfigFrame(FrameConfigDataModel configData);

        void SendExerciceFrame(FrameExerciceDataModel exerciceData);

        void SendExerciceGameFrame(FrameExerciceDataModel exerciceData);

        void StreamTrajectory(List<List<Point>> traj);

        event onaXdataReceived aXdataReceived;
        event onErrorDataReceived ErrorDataReceived;
		event onCoupleDataReceived CoupleDataReceived;
        event onPositionDataReceived PositionDataReceived;
        event onPosition2DataReceived Position2DataReceived;
        event onForceDataReceived ForceDataReceived;
        event onForce2DataReceived Force2DataReceived;
        event onVitesseDataReceived VitesseDataReceived;
        event onVitesse2DataReceived Vitesse2DataReceived;
		event onForceRapDataReceived ForceRapDataReceived;
        event onForceRap2DataReceived ForceRap2DataReceived;
        event onAcosTDataReceived AcosTDataReceived;
        event onACKDataReceived ACKDataReceived;
        event onFrameConfigDataReceived FrameConfigDataReceived;
		event onFrameExerciceDataReceived FrameExerciceDataReceived;
        event onBorneDataReceived BorneDataReceived;
        event onPprDataReceived PprDataReceived;
        event EventHandler StreamingDone;
    }
}

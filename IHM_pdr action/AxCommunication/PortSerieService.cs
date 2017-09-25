using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using AxModel;
using AxError;
using System.Windows;
namespace AxCommunication
{

    /// <summary>
    /// This class implements the IPortSerieService for Communication purposes.
    /// </summary>
    public class PortSerieService : IPortSerieService, IDisposable
    {
        #region Fields
        /// <summary>
        /// 
        /// </summary>
        private Thread _closeThread;        // Déclaration du thread pour fermer le port com
        private Thread _frameInBuffer;      // Déclaration du thread pour la réception
        private Thread _frameOutBuffer;     // Déclaration du thread pour l'émission
        private AutoResetEvent _frameBufferEvent;
        private AutoResetEvent _frameBufferEventOut;
        private SerialPort _portSerie;
        private Queue<byte> _inDataBuffer = new Queue<byte>();  // Buffer d'entrée pour OnDataReceived
        private ObservableCollection<string> _portNameCollection = new ObservableCollection<string>(SerialPort.GetPortNames());
        private ConcurrentQueue<byte[]> _inFrameBuffer;     // Buffer d'entrée pour le thread _frameInBuffer
        private ConcurrentQueue<byte[]> _outFrameBuffer;    // Buffer de sortie pour le thread _frameOutBuffer

        private string _selectedPortName;                   // Port Serie selectionné
        //Object locker = new Object();

        /// <summary>
        /// Gets the number of reception errors.
        /// </summary>
        public int ReceptionErrors { get; private set; }

        /// <summary>
        /// Gets the number of frames read.
        /// </summary>
        public FrameCountModel FramesReadCounter { get; private set; }

        /// <summary>
        /// Gets the number of frames written.
        /// </summary>
        public FrameCountModel FramesWrittenCounter { get; private set; }

        /// <summary>
        /// Private receive buffer.
        /// </summary>
        private byte[] receiveBuffer = new byte[256];

        /// <summary>
        /// Private receive buffer index.
        /// </summary>
        private byte[] encodedFrame = new byte[12];
        private byte[] encodedFrameQ = new byte[12];
        private byte[] encodedFrameOutQ = new byte[12];

        bool _ack_ok = false;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the PortSerieModel class.
        /// </summary>
        public PortSerieService()
        {
            try
            {
                _frameBufferEvent = new AutoResetEvent(false);
                _frameBufferEventOut = new AutoResetEvent(false);
                _inFrameBuffer = new ConcurrentQueue<byte[]>();
                _outFrameBuffer = new ConcurrentQueue<byte[]>();
                // Instanciation du thread, on spécifie dans le 
                // délégué ThreadStart le nom de la méthode qui
                // sera exécutée lorsque l'on appele la méthode
                // Start() de notre thread.
                _closeThread = new Thread(new ThreadStart(ThreadDispose));

                _frameInBuffer = new Thread(() =>
                {
                    for (; ; )
                    {
                        if (_inFrameBuffer.Count == 0)
                        {
                            _frameBufferEvent.WaitOne();
                        }
                        //Debug.Print("WaitOne"+ DateTime.Now);
                        aXdataModel dataObject = null;
                        try
                        {
                            _inFrameBuffer.TryDequeue(out encodedFrameQ);
                            if (encodedFrameQ != null)
                            {
                                dataObject = FrameConstruction.DeconstructFrame(encodedFrameQ);
                            }
                        }
                        catch (Exception ex)
                        {
                            ReceptionErrors++;
                            // TODO : et l'affichage ???
                        }
                        if (dataObject != null)             // if frame successfully deconstructed
                        {
                            OnaXdataReceived(dataObject);
                            if (dataObject is ErrorDataModel) { OnErrorDataReceived((ErrorDataModel)dataObject); FramesReadCounter.ErrorFrames++; }
							else if (dataObject is CoupleDataModel) { OnCoupleDataReceived((CoupleDataModel)dataObject); }
                            else if (dataObject is PositionDataModel) { OnPositionDataReceived((PositionDataModel)dataObject); }
                            else if (dataObject is Position2DataModel) { OnPosition2DataReceived((Position2DataModel)dataObject); }
                            else if (dataObject is PprDataModel) { OnPprDataReceived((PprDataModel)dataObject); }
                            else if (dataObject is ForceDataModel) { OnForceDataReceived((ForceDataModel)dataObject); }
                            else if (dataObject is Force2DataModel) { OnForce2DataReceived((Force2DataModel)dataObject); }
							else if (dataObject is ForceRapDataModel) { OnForceRapDataReceived((ForceRapDataModel)dataObject); }
                            else if (dataObject is ForceRap2DataModel) { OnForceRap2DataReceived((ForceRap2DataModel)dataObject); }
                            else if (dataObject is AcosTDataModel) { OnAcosTDataReceived((AcosTDataModel)dataObject); }
                            else if (dataObject is VitesseModel) { OnVitesseDataReceived((VitesseModel)dataObject); }
                            else if (dataObject is Vitesse2Model) { OnVitesse2DataReceived((Vitesse2Model)dataObject); }
                            else if (dataObject is ACKDataModel) { OnACKDataReceived((ACKDataModel)dataObject); }
                            else if (dataObject is StreamAckDataModel) { OnStreamACKDataReceived((StreamAckDataModel)dataObject); }
                            else if (dataObject is FrameConfigDataModel) { OnFrameConfigDataReceived((FrameConfigDataModel)dataObject); }
							else if (dataObject is FrameExerciceDataModel) { OnFrameExerciceDataReceived((FrameExerciceDataModel)dataObject); }
                            else if (dataObject is BorneDataModel) { OnBorneDataReceived((BorneDataModel)dataObject); }
                        }
                    }
                }, 1);
                _frameInBuffer.IsBackground = true;
                _frameInBuffer.Name = "Frame Buffer Thread IN";
                _frameInBuffer.Start();

                _frameOutBuffer = new Thread(() =>
                {
                    for (; ; )
                    {
                        if (_outFrameBuffer.Count == 0)
                        {
                            _frameBufferEventOut.WaitOne();
                        }
                        try
                        {
                            //if (ACK_ok == true) // TODO : ack c'est bien comme ca ?
                            //{
                            //ACK_ok = false;
                            _outFrameBuffer.TryDequeue(out encodedFrameOutQ);
                            if (encodedFrameOutQ != null)
                            {
                                SendByteArray(encodedFrameOutQ);
                                System.Threading.Thread.Sleep(12); // TODO : Pause car pas encore d'ACK
                            }
                            // }
                        }
                        catch
                        {
                            // TODO : erreur ++ envois
                        }
                    }
                }, 1);
                _frameOutBuffer.IsBackground = true;
                _frameOutBuffer.Name = "Frame Buffer Thread OUT";
                _frameOutBuffer.Start();
                //SFrame STrame = new SFrame(SendFrame);
                //_outDataBuffer = new ProducerConsumerQueue(STrame);
                this._portSerie = new SerialPort(); // Création d'un nouvel objet SerialPort par défaut
                FramesReadCounter = new FrameCountModel();
                FramesWrittenCounter = new FrameCountModel();
                ReceptionErrors = 0;
                Debug.Print("PortSerieModel OK");
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        #endregion

        #region Properties
        /// <summary>
        /// Available ports on the computer
        /// </summary>
        public ObservableCollection<string> GetPortNameCollection()
        {
            return this._portNameCollection;
        }

        public string SelectedPortName
        {
            get
            {
                return this._selectedPortName;
            }
            set
            {
                this._selectedPortName = value;
            }
        }
        public SerialPort PortSerie
        {
            get
            {
                return this._portSerie;
            }
            set
            {
                this._portSerie = value;
            }
        }
        public bool IsOpen()
        {
            bool test = false;
            try
            {
                test = _portSerie.IsOpen;
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
            return test;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Ouverture port Serie + abonnement aux evenements
        /// </summary>
        public void Open()
        {
                // Closing serial port if it is open
                if (this._portSerie != null && this._portSerie.IsOpen)
                    this._portSerie.Close();
                // Setting serial port settings
                this._portSerie.Handshake = Handshake.None;
                this._portSerie.ReadTimeout = 512;
                this._portSerie.Parity = Parity.None;
                this._portSerie = new SerialPort(_selectedPortName, 460800, Parity.None, 8, StopBits.One); // Création d'un nouvel objet SerialPort
                // Subscribe to event and open serial port for data 
                this._portSerie.DataReceived += new SerialDataReceivedEventHandler(OnDataReceived);
                this._portSerie.ErrorReceived += new SerialErrorReceivedEventHandler(OnErrorReceived);
                this._portSerie.ReceivedBytesThreshold = 8;
                this._portSerie.Open();
        }

        /// <summary>
        /// Fermeture du port Serie + désabonnement aux evenements
        /// </summary>
        public void Close()
        {
            if (_portSerie.IsOpen)
            {
                _portSerie.Close();
                _portSerie.DataReceived -= OnDataReceived;
                _portSerie.ErrorReceived -= OnErrorReceived;
            }
        }

        private void OnErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            try
            {
                if (_portSerie.IsOpen)
                {
                    Close();    // Ferme le port Serie
                    Open();
                }
                else
                {
                    Open();
                }

            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        #region OnDataReceived
        /// <summary>
        /// Méthode appelée quand il y a des données en attentes dans le buffer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int numBytesRead;
            byte[] serialBuffer;

            // Fetch bytes from serial buffer
            try
            {
                numBytesRead = _portSerie.BytesToRead;          // get the number of bytes in the serial buffer
                serialBuffer = new byte[numBytesRead];          // local array to hold bytes in the serial buffer
                _portSerie.Read(serialBuffer, 0, numBytesRead); // transfer bytes
                serialBuffer.ToList().ForEach(b => _inDataBuffer.Enqueue(b));   // mise des données dans la liste FIFO
            }
            catch
            {
                return;                                         // return on exception; e.g. serial port closed unexpectedly
            }
            // _inDataBuffer.ToList().ForEach(b => Debug.Print(b.ToString()));
            // Process each byte one-by-one
            for (int i = 0; _inDataBuffer.Count > 11; i++)
            {
                if (_inDataBuffer.ElementAt(0) == 0xCF && _inDataBuffer.ElementAt(1) == 0xDF && _inDataBuffer.ElementAt(10) == 0xEF && _inDataBuffer.ElementAt(11) == 0xFF)
                {
                    //aXdataModel dataObject = null;
                    //try
                    //{
                        encodedFrame[0] = _inDataBuffer.Dequeue();
                        encodedFrame[1] = _inDataBuffer.Dequeue();
                        encodedFrame[2] = _inDataBuffer.Dequeue();
                        encodedFrame[3] = _inDataBuffer.Dequeue();
                        encodedFrame[4] = _inDataBuffer.Dequeue();
                        encodedFrame[5] = _inDataBuffer.Dequeue();
                        encodedFrame[6] = _inDataBuffer.Dequeue();
                        encodedFrame[7] = _inDataBuffer.Dequeue();
                        encodedFrame[8] = _inDataBuffer.Dequeue();
                        encodedFrame[9] = _inDataBuffer.Dequeue();
                        encodedFrame[10] = _inDataBuffer.Dequeue();
                        encodedFrame[11] = _inDataBuffer.Dequeue();
                        _inFrameBuffer.Enqueue(encodedFrame);
                        encodedFrame = new byte[12];
                        _frameBufferEvent.Set();
                        //dataObject = FrameConstruction.DeconstructFrame(encodedFrame);
                    //}
                    //catch
                    //{
                    //    ReceptionErrors++;
                    //     TODO : et l'affichage ???
                    //}
                    /*if (dataObject != null)             // if frame successfully deconstructed
                    {
                        OnaXdataReceived(dataObject);
                        if (dataObject is ErrorDataModel) { OnErrorDataReceived((ErrorDataModel)dataObject); FramesReadCounter.ErrorFrames++; }
                        else if (dataObject is CoupleDataModel) { OnCoupleDataReceived((CoupleDataModel)dataObject); }
                        else if (dataObject is PositionDataModel) { OnPositionDataReceived((PositionDataModel)dataObject); }
                        else if (dataObject is ForceDataModel) { OnForceDataReceived((ForceDataModel)dataObject); }
                        else if (dataObject is ACKDataModel) { OnACKDataReceived((ACKDataModel)dataObject); }
                        else if (dataObject is FrameConfigDataModel) { OnFrameConfigDataReceived((FrameConfigDataModel)dataObject); }
                        else if (dataObject is BorneDataModel) { OnBorneDataReceived((BorneDataModel)dataObject); }
                    }*/

                }
                else
                {
                    _inDataBuffer.Dequeue();
                }
            }
        }

        #endregion

        //public delegate void onaXdataReceived(object sender, aXdataModel e);
        public event onaXdataReceived aXdataReceived;
        protected virtual void OnaXdataReceived(aXdataModel e) { if (aXdataReceived != null) aXdataReceived(this, e); }

        //public delegate void onErrorDataReceived(object sender, ErrorDataModel e);
        public event onErrorDataReceived ErrorDataReceived;
        protected virtual void OnErrorDataReceived(ErrorDataModel e) { if (ErrorDataReceived != null) ErrorDataReceived(this, e); }

		public event onCoupleDataReceived CoupleDataReceived;
        protected virtual void OnCoupleDataReceived(CoupleDataModel e) { if (CoupleDataReceived != null) CoupleDataReceived(this, e); }
        //public delegate void onPositionDataReceived(object sender, PositionDataModel e);
        public event onPositionDataReceived PositionDataReceived;
        protected virtual void OnPositionDataReceived(PositionDataModel e) { if (PositionDataReceived != null) PositionDataReceived(this, e); }

        //public delegate void onCoupleDataReceived(object sender, CoupleDataModel e);
        public event onPosition2DataReceived Position2DataReceived;
        protected virtual void OnPosition2DataReceived(Position2DataModel e) { if (Position2DataReceived != null) Position2DataReceived(this, e); }

		public event onVitesseDataReceived VitesseDataReceived;
        protected virtual void OnVitesseDataReceived(VitesseModel e) { if (VitesseDataReceived != null) VitesseDataReceived(this, e); }

        public event onVitesse2DataReceived Vitesse2DataReceived;
        protected virtual void OnVitesse2DataReceived(Vitesse2Model e) { if (Vitesse2DataReceived != null) Vitesse2DataReceived(this, e); }
        //public delegate void onForceDataReceived(object sender, ForceDataModel e);
        public event onForceDataReceived ForceDataReceived;
        protected virtual void OnForceDataReceived(ForceDataModel e) { if (ForceDataReceived != null) ForceDataReceived(this, e); }

        //public delegate void onForceDataReceived(object sender, ForceDataModel e);
        public event onForce2DataReceived Force2DataReceived;
        protected virtual void OnForce2DataReceived(Force2DataModel e) { if (Force2DataReceived != null) Force2DataReceived(this, e); }
        //public delegate void onACKDataReceived(object sender, ACKDataModel e);
        public event onACKDataReceived ACKDataReceived;
        protected virtual void OnACKDataReceived(ACKDataModel e) { if (ACKDataReceived != null) ACKDataReceived(this, e); }

        public event onStreamACKDataReceived StreamACKDataReceived;
        protected virtual void OnStreamACKDataReceived(StreamAckDataModel e) { if (StreamACKDataReceived != null) StreamACKDataReceived(this, e); }

        //public delegate void onFrameConfigDataReceived(object sender, FrameConfigDataModel e);
        public event onFrameConfigDataReceived FrameConfigDataReceived;
        protected virtual void OnFrameConfigDataReceived(FrameConfigDataModel e) { if (FrameConfigDataReceived != null) FrameConfigDataReceived(this, e); }

		 public event onFrameExerciceDataReceived FrameExerciceDataReceived;
        protected virtual void OnFrameExerciceDataReceived(FrameExerciceDataModel e) { if (FrameExerciceDataReceived != null) FrameExerciceDataReceived(this, e); }
        //public delegate void onBorneDataReceived(object sender, BorneDataModel e);
        public event onBorneDataReceived BorneDataReceived;
        protected virtual void OnBorneDataReceived(BorneDataModel e) { if (BorneDataReceived != null) BorneDataReceived(this, e); }

        //public delegate void onPprDataReceived(object sender, PositionDataModel e);
        public event onPprDataReceived PprDataReceived;
        protected virtual void OnPprDataReceived(PprDataModel e) { if (PprDataReceived != null) PprDataReceived(this, e); }

		public event onForceRapDataReceived ForceRapDataReceived;
        protected virtual void OnForceRapDataReceived(ForceRapDataModel e) { if (ForceRapDataReceived != null) ForceRapDataReceived(this, e); }

        public event onForceRap2DataReceived ForceRap2DataReceived;
        protected virtual void OnForceRap2DataReceived(ForceRap2DataModel e) { if (ForceRap2DataReceived != null) ForceRap2DataReceived(this, e); }

        public event onAcosTDataReceived AcosTDataReceived;
        protected virtual void OnAcosTDataReceived(AcosTDataModel e) { if (AcosTDataReceived != null) AcosTDataReceived(this, e); }

        public event EventHandler StreamingDone;

        #region Methods
        public bool ACK_ok
        {
            get
            {
                return _ack_ok;
            }
            set
            {
                _ack_ok = value;
            }
        }
        /// <summary>
        /// Sends command frame.
        /// </summary>
        /// <param name="commandCode">
        /// Command data to be sent.
        /// </param> 
        public void SendCommandFrame(CommandCodes commandCode)
        {
            // TODO : ajouter la commande dans la FILE pour ACK + timer
            AddTrameOut(FrameConstruction.ConstructCommandFrame(commandCode));
            //FramesWrittenCounter.CommandFrames++;
        }

        /// <summary>
        /// Sends config frame.
        /// </summary>
        /// <param name="commandCode">
        /// Command data to be sent.
        /// </param> 
        public void SendConfigFrame(FrameConfigDataModel configData)
        {
            // TODO : ajouter la commande dans la FILE pour ACK + timer
            //SendByteArray(FrameConstruction.ConstructWriteConfigDataFrame(configData));
            AddTrameOut(FrameConstruction.ConstructWriteConfigDataFrame(configData));
            //FramesWrittenCounter.CommandFrames++;
        }

        /// <summary>
        /// Sends exercice frame.
        /// </summary>
        /// <param name="commandCode">
        /// Command data to be sent.
        /// </param> 
        public void SendExerciceFrame(FrameExerciceDataModel exerciceData)
        {
            // TODO : ajouter la commande dans la FILE pour ACK + timer
            AddTrameOut(FrameConstruction.ConstructWriteExerciceDataFrame(exerciceData));
            //SendFrame(FrameConstruction.ConstructWriteExerciceDataFrame(exerciceData));
            //FramesWrittenCounter.CommandFrames++;
        }

        /// <summary>
        /// Sends exercice frame.
        /// </summary>
        /// <param name="commandCode">
        /// Command data to be sent.
        /// </param> 
        public void SendExerciceGameFrame(FrameExerciceDataModel exerciceData)
        {
            // TODO : ajouter la commande dans la FILE pour ACK + timer
            //SendByteArray(FrameConstruction.ConstructWriteExerciceDataFrame(exerciceData));
            AddTrameOut(FrameConstruction.ConstructWriteExerciceDataFrame(exerciceData));
            //FramesWrittenCounter.CommandFrames++;
        }

        /// <summary>
        /// Sends byte array to serial port and increments the sent frames counter.
        /// </summary>
        /// <param name="byteArray">
        /// Byte array to be sent to serial port.
        /// </param>
        private void SendByteArray(byte[] byteArray)
        {
            _portSerie.Write(byteArray, 0, byteArray.Length);
            //_outDataBuffer.EnqueueTask(byteArray);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteArray"></param>
        private void AddTrameOut(byte[] byteArray)
        {
            _outFrameBuffer.Enqueue(byteArray);
            _frameBufferEventOut.Set();
        }

        /// <summary>
        /// Sends byte frame to serial port
        /// </summary>
        /// <param name="frame"></param>
        public void SendFrame(byte[] frame)
        {
            //lock (locker)
            //{
                _portSerie.Write(frame, 0, frame.Length);
            //}
        }

        private byte currentSeg;
        private byte currentPoint;
        List<List<Point>> trajectory = new List<List<Point>>();
        private System.Timers.Timer streamingTimer;

        public void StreamTrajectory(List<List<Point>> traj)
        {
            if (traj != null && traj.Count > 0)
            {
                currentSeg = 0;
                currentPoint = 0;
                trajectory = traj;

                streamingTimer = new System.Timers.Timer();
                streamingTimer.AutoReset = false;
                streamingTimer.Interval = 2000;
                streamingTimer.Elapsed += new System.Timers.ElapsedEventHandler(streamingTimer_Elapsed);
                

                this.StreamACKDataReceived += new onStreamACKDataReceived(PortSerieService_StreamACKDataReceived);

                StreamInitModel streamInit = new StreamInitModel((byte)trajectory.Count);
                var frameInit = streamInit.MakeFrame();

                //streamingTimer.Start();
                SendExerciceFrame(frameInit);
            }
        }

        private void SendTrameSegment()
        {
            if (trajectory != null && trajectory.Count > 0)
            {
                if (currentSeg >= trajectory.Count)
                {
                    AbortStreaming();
                    //Fin du streaming
                }
                else
                {
                    StreamInitSegModel streamSeg = new StreamInitSegModel(currentSeg, (byte)trajectory[currentSeg].Count);
                    var frameSeg = streamSeg.MakeFrame();

                    //streamingTimer.Start();
                    SendExerciceFrame(frameSeg);
                }
            }
        }

        private void SendPoint()
        {
            if (trajectory != null && trajectory.Count > 0)
            {
                if (currentPoint >= trajectory[currentSeg].Count)
                {
                    currentSeg++;
                    currentPoint = 0;
                    SendTrameSegment();
                }
                else
                {
                    ushort x = (ushort)(trajectory[currentSeg][currentPoint].X * 10);
                    ushort y = (ushort)(trajectory[currentSeg][currentPoint].Y * 10);
                    StreamPointModel streamPoint = new StreamPointModel(x, y, currentPoint);
                    var framePoint = streamPoint.MakeFrame();
                    Debug.Print(x.ToString() + " " + y.ToString());
                    //streamingTimer.Start();
                    SendExerciceFrame(framePoint);
                }
            }
        }

        private void PortSerieService_StreamACKDataReceived(object sender, StreamAckDataModel e)
        {
            switch (e.Address)
            {
                case FrameHeaders.ACK_StreamingInit:
                    streamingTimer.Stop();
                    // l'init du streaming est ok on envoie le premier segment
                    SendTrameSegment();
                    break;
                case FrameHeaders.ACK_StreamingSeg:
                    streamingTimer.Stop();
                    if (currentSeg == e.Data1)
                    {
                        SendPoint();
                    }
                    else
                    {
                        SendTrameSegment();
                    }
                    break;
                case FrameHeaders.ACK_StreamingPoint:
                    streamingTimer.Stop();
                    if(currentPoint == e.Data4)
                    {
                        currentPoint++;
                    }
                    SendPoint();
                    break;
                case FrameHeaders.ACK_StreamingError:
                    break;
                case FrameHeaders.ACK_StreamingMod:
                    break;
                default:
                    break;
            }
        }

        private void streamingTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            AbortStreaming();
        }

        private void AbortStreaming()
        {
            if (this.StreamingDone != null)
            {
                this.StreamingDone(this, EventArgs.Empty);
            }

            this.StreamACKDataReceived -= new onStreamACKDataReceived(PortSerieService_StreamACKDataReceived);

            if (trajectory != null)
            {
                trajectory.Clear();
            }

            currentSeg = 0;
            currentPoint = 0;

            if (streamingTimer.Enabled)
            {
                streamingTimer.Stop();
            }
        }
        #endregion

        /// <summary>
        /// Call to release serial port
        /// </summary>
        public void Dispose()
        {
           // _outDataBuffer.Dispose();
            Dispose(true);
        }

        /// <summary>
        /// Part of basic design pattern for implementing Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _closeThread.Start();   // Résolution bug fermeture port serie, merci Microsoft :D
            }
        }

        private void ThreadDispose()    // https://connect.microsoft.com/VisualStudio/feedback/details/202137/serialport-close-hangs-the-application
        {
           
                _portSerie.DataReceived -= new SerialDataReceivedEventHandler(OnDataReceived);
                _portSerie.ErrorReceived -= new SerialErrorReceivedEventHandler(OnErrorReceived);
            
            // Releasing serial port (and other unmanaged objects)
            if (_portSerie != null)
            {
                if (_portSerie.IsOpen)
                {
                    _portSerie.DiscardInBuffer();
                    _portSerie.DiscardOutBuffer();
                    _portSerie.Close();
                }

                _portSerie.Dispose();
            }
        }
        #endregion
    }
}

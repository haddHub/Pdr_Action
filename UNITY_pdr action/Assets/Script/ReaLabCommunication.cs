using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

public class ReaLabCommunication : MonoBehaviour
{

    public string AdressIP;
    public int SendPort;
    public int ReceivePort;
    public float ConfigTimeOut = 5;
    private float configAskCD;
    private bool configReceived = false;

    #region Fields
    /// <summary>
    /// Client udp pour l'envoie de données à Unity
    /// </summary>
    private UdpClient udpSender;

    /// <summary>
    /// Endpoint du sender udp
    /// </summary>
    private IPEndPoint sendEndPoint;

    /// <summary>
    /// Client Udp pour la réception de données envoyé par Unity
    /// </summary>
    private UdpClient udpReceiver;

    /// <summary>
    /// Endpoint du receiver udp
    /// </summary>
    private IPEndPoint receiveEndPoint;

    /// <summary>
    /// Instance du thread d'envoie de données
    /// </summary>
    private Thread receiveThread;

    /// <summary>
    /// Control du thread de réception
    /// </summary>
    private bool receiveThreadRunning = false;

    /// <summary>
    /// Dico des set et get value
    /// </summary>
    private Dictionary<string, object> values = new Dictionary<string, object>();
    #endregion

    #region Event
    public event NewConfigurationHandler onNewConfiguration;

    public event NewPositionsHandler onNewPositions;
    public event NewPositions2Handler onNewPositions2;

    public event NewForcesHandler onNewForces;
    public event NewForces2Handler onNewForces2;

    public event PauseGameHandler onPause;
    public event NextLevelHandler onNextLevel;
    public event StopGameHandler onStop;

    public event NewValueHandler onNewValue;
    #endregion

    void Awake()
    {
        // création des endpoints
        this.sendEndPoint = new IPEndPoint(IPAddress.Parse(AdressIP), SendPort);
        this.receiveEndPoint = new IPEndPoint(IPAddress.Any, 0);

        // création des client udp
        this.udpSender = new UdpClient();

        this.udpReceiver = new UdpClient(ReceivePort);

        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// Connect le client(Unity) au serveur(ReaLab)
    /// </summary>
    public void Connect()
    {
        // Démarre la réception de données
        //this.udpReceiver.BeginReceive(this.ReceiveData, this.udpReceiver);
        this.receiveThread = new Thread(new ThreadStart(this.ReceiveData));
        this.receiveThread.IsBackground = true;
        this.receiveThreadRunning = true;
        this.receiveThread.Start();

        // Demande la configuration
        this.AskConfiguration();
        this.configAskCD = this.ConfigTimeOut;
    }

    void Update()
    {
        if (this.configReceived == false)
        {
            this.configAskCD -= Time.deltaTime;

            if (this.configAskCD <= 0)
            {
                this.AskConfiguration();
                this.configAskCD = this.ConfigTimeOut;
            }
        }
    }

    /// <summary>
    /// Coupe le connexion entre le client et le serveur
    /// </summary>
    public void Disconnect()
    {
        Debug.Log("Disconnect");
        this.receiveThreadRunning = false;
        this.receiveThread = null;

        // Stop la réception de données
        this.udpReceiver.Close();
        this.udpReceiver = null;

        // Stop l'envoie de données
        this.udpSender.Close();
        this.udpSender = null;
    }

    /// <summary>
    /// Lit les trames sur le stream et les interpretes
    /// </summary>
    private void ReceiveData()//IAsyncResult ar
    {
        try
        {
            while (this.receiveThreadRunning == true)
            {
                if (this.udpReceiver != null)
                {
                    TrameId id;

                    var trame = this.udpReceiver.Receive(ref this.receiveEndPoint);//this.udpReceiver.EndReceive(ar, ref this.receiveEndPoint);
                    // Analyse la trame qui à été reçue
                    var result = FrameDeconstruct.Deconstruct(trame, out id);

                    // En fonction de la trame il faut adopter le bon comprtement
                    // Dans certains cas il faut lever des évenements dans d'autres il faut garder des valeurs en mémoire
                    switch (id)
                    {
                        case TrameId.Configuration:
                            if (this.onNewConfiguration != null)
                            {
                                this.configReceived = true;
                                ConfigEvent configArgs = new ConfigEvent(result as ReaLabConfiguration);
                                this.onNewConfiguration(this, configArgs);
                            }
                            break;
                        case TrameId.Positions:
                            if (this.onNewPositions != null)
                            {
                                PointEvent pointArgs = new PointEvent(result as PointData);
                                this.onNewPositions(this, pointArgs);
                            }
                            break;
                        case TrameId.Positions2:
                            if (this.onNewPositions2 != null)
                            {
                                PointEvent pointArgs = new PointEvent(result as PointData);
                                this.onNewPositions2(this, pointArgs);
                            }
                            break;
                        case TrameId.Forces:
                            if (this.onNewForces != null)
                            {
                                PointEvent pointArgs = new PointEvent(result as PointData);
                                this.onNewForces(this, pointArgs);
                            }
                            break;
                        case TrameId.Forces2:
                            if (this.onNewForces2 != null)
                            {
                                PointEvent pointArgs = new PointEvent(result as PointData);
                                this.onNewForces2(this, pointArgs);
                            }
                            break;
                        case TrameId.PauseGame:
                            if (this.onPause != null)
                            {
                                this.onPause(this, EventArgs.Empty);
                            }
                            break;
                        case TrameId.StopGame:
                            if (this.onStop != null)
                            {
                                this.onStop(this, EventArgs.Empty);
                            }
                            break;
                        case TrameId.SetValue:
                            string key = (result as ValueData).Key;
                            object data = (result as ValueData).Data;
                            if (this.values.ContainsKey(key))
                            {
                                this.values.Remove(key);
                            }

                            this.values.Add(key, data);

                            if (this.onNewValue != null)
                            {
                                ValueEvent valueArgs = new ValueEvent(key, data);
                                this.onNewValue(this, valueArgs);
                            }
                            break;
                        case TrameId.NextLevel:
                            if (this.onNextLevel != null)
                            {
                                this.onNextLevel(this, EventArgs.Empty);
                            }
                            break;
                        case TrameId.Null:
                            break;
                        default:
                            break;
                    }
                }

                Thread.Sleep(1);
            }
        }
        catch (Exception ex)
        {
        }
    }

    /// <summary>
    /// Demande à ReaLab d'envoyer sa configuration
    /// </summary>
    public void AskConfiguration()
    {
        var trame = FrameConstruct.ConstructAskConfiguration();

        this.SendTrameToServer(trame);
    }

    /// <summary>
    /// Envoie la trajectoire que le robot devra suivre à ReaLab
    /// </summary>
    /// <param name="path"></param>
    public void SetTrajectory(PointData[] path)
    {
        var trame = FrameConstruct.ConstructTrajectory(path);

        this.SendTrameToServer(trame);
    }

    /// <summary>
    /// Envoie la série de checkpoints à parcourir
    /// </summary>
    /// <param name="checkpoints"></param>
    public void SetCheckpoints(PointData[] checkpoints)
    {
        var trame = FrameConstruct.ConstructCheckppoints(checkpoints);

        this.SendTrameToServer(trame);
    }

    /// <summary>
    /// Indique au serveur que le niveau à bien été chargé
    /// </summary>
    /// <param name="levelNum">indice du niveau chargé</param>
    public void LevelLoaded(int levelNum)
    {
        var trame = FrameConstruct.ConstructLevelLoaded(levelNum);
        this.SendTrameToServer(trame);
    }

    /// <summary>
    /// Indique au serveur que la phase de jeu a commencé
    /// Apres le décompte 3-2-1
    /// </summary>
    public void LevelStarted()
    {
        var trame = FrameConstruct.ConstructLevelStarted();
        this.SendTrameToServer(trame);
    }

    /// <summary>
    /// Indique au serveur que le niveau actuel est fini
    /// </summary>
    public void LevelStopped()
    {
        var trame = FrameConstruct.ConstructLevelStopped();
        this.SendTrameToServer(trame);
    }

    /// <summary>
    /// Indique au serveur qu' un checkpoint est atteint
    /// </summary>
    /// <param name="checkpointNum">indice de ce cjeckpoint</param>
    public void CheckpointReached(int checkpointNum)
    {
        var trame = FrameConstruct.ConstructCheckpointReached(checkpointNum);
        this.SendTrameToServer(trame);
    }

    /// <summary>
    /// Envoie un trame au serveur sur le thread d'envoie
    /// </summary>
    private void SendTrameToServer(byte[] trame)
    {
        if (this.udpSender != null)
        {
            this.udpSender.Send(trame, trame.Length, this.sendEndPoint);
        }
    }

    public void SetValue(string key, string data)
    {
        this.SetValueTrame(key, data);
    }

    public void SetValue(string key, double data)
    {
        this.SetValueTrame(key, data);
    }

    public void SetValue(string key, bool data)
    {
        this.SetValueTrame(key, data);
    }

    public void SetValues(string key, int[] data)
    {
        this.SetValueTrame(key, data);
    }

    private void SetValueTrame(string key, object data)
    {
        if (this.values.ContainsKey(key))
        {
            this.values.Remove(key);
        }

        this.values.Add(key, data);

        var trame = FrameConstruct.ConstructValue(key, data);

        this.SendTrameToServer(trame);
    }

    public object GetValue(string key)
    {
        object value;
        this.values.TryGetValue(key, out value);
        return value;
    }

    public Dictionary<string, object> GetAllValues()
    {
        return this.values;
    }
}

#region Classes
public delegate void NewConfigurationHandler(object obj, ConfigEvent configArgs);
public delegate void NewPositionsHandler(object obj, PointEvent positionsArgs);
public delegate void NewPositions2Handler(object obj, PointEvent positions2Args);
public delegate void NewForcesHandler(object obj, PointEvent forcesArgs);
public delegate void NewForces2Handler(object obj, PointEvent forces2Args);
public delegate void PauseGameHandler(object obj, EventArgs pauseArgs);
public delegate void NextLevelHandler(object obj, EventArgs nextLevelArgs);
public delegate void StopGameHandler(object obj, EventArgs stopArgs);
public delegate void NewValueHandler(object obj, ValueEvent valueArgs);
public class ReaLabConfiguration
{
    /// <summary>
    /// Constructor of ReaLabConfiguration
    /// </summary>
    /// <param name="firstName">first name of the current patient</param>
    /// <param name="lastName">last name of the current patient</param>
    /// <param name="birthDate">birth date of the current patient</param>
    /// <param name="difficulty">game difficulty selected by the therapist</param>
    /// <param name="zoom">game zoom selected by the therapist</param>
    /// <param name="level">level that the game need to load</param>
    /// <param name="debug">debug status</param>
    public ReaLabConfiguration(string lastName, string firstName, DateTime birthDate, int difficulty, int zoom, int level, bool debug)
    {
        this.PatientLastName = lastName;
        this.PatientFirstName = firstName;
        this.PatientBirthDate = birthDate;
        this.GameDifficulty = difficulty;
        this.GameZoom = zoom;
        this.LevelToLoad = level;
        this.IsDebug = debug;
    }

    #region Properties
    /// <summary>
    /// Gets the first name of the current patient
    /// </summary>
    public string PatientFirstName { get; set; }

    /// <summary>
    /// Gets the last name of the current patient
    /// </summary>
    public string PatientLastName { get; set; }

    /// <summary>
    /// Gets the birth date of the current patient
    /// </summary>
    public DateTime PatientBirthDate { get; set; }

    /// <summary>
    /// Gets the game difficulty selected by the therapist
    /// 1 = easy, 2 = medium, 3 = hard, 4 = expert
    /// </summary>
    public int GameDifficulty { get; set; }

    /// <summary>
    /// Gets the game zoom selected by the therapist
    /// 1 = small, 2 = medium, 3 = large
    /// </summary>
    public int GameZoom { get; set; }

    /// <summary>
    /// Gets the level that the game need to load
    /// </summary>
    public int LevelToLoad { get; set; }

    /// <summary>
    /// Gets the debug status
    /// </summary>
    public bool IsDebug { get; set; }
    #endregion
}
public class PointEvent : EventArgs
{
    private PointData point;

    public PointEvent(PointData point)
    {
        this.point = point;
    }

    public PointData Point
    {
        get
        {
            if (this.point != null)
            {
                return this.point;
            }
            else
            {
                return null;
            }
        }
    }
}
public class ConfigEvent : EventArgs
{
    private ReaLabConfiguration config;

    public ConfigEvent(ReaLabConfiguration config)
    {
        this.config = config;
    }

    public ReaLabConfiguration Config
    {
        get
        {
            if (this.config != null)
            {
                return this.config;
            }
            else
            {
                return null;
            }
        }
    }
}
public class ValueEvent : EventArgs
{
    private object value;
    private string key;

    public ValueEvent(string key, object value)
    {
        this.key = key;
        this.value = value;
    }

    public string Key
    {
        get
        {
            return this.key;
        }
    }

    public object Value
    {
        get
        {
            return this.value;
        }
    }
}
internal class ValueData
{
    public string Key { get; set; }
    public DataTypeTrameId DataType { get; set; }
    public object Data { get; set; }

    public ValueData(string key, DataTypeTrameId dataType, object data)
    {
        this.Key = key;
        this.DataType = dataType;
        this.Data = data;
    }
}
public class PointData
{
    public PointData(double x, double y)
    {
        this.Xd = x;
        this.Yd = y;

        this.Xf = (float)x;
        this.Yf = (float)y;
    }

    public PointData(float x, float y)
    {
        this.Xd = x;
        this.Yd = y;

        this.Xf = x;
        this.Yf = y;
    }

    public double Xd;
    public double Yd;

    public float Xf;
    public float Yf;
}
internal enum TrameId
{
    Configuration = 0,
    Trajectory = 1,
    Checkppoints = 2,
    Positions = 3,
    Positions2 = 4,
    Forces = 5,
    Forces2 = 6,
    PauseGame = 7,
    StopGame = 8,
    LevelLoaded = 9,
    LevelStarted = 10,
    LevelStopped = 11,
    CheckpointReached = 12,
    SetValue = 13,
    NextLevel = 14,
    Null = -1
}
internal enum DataTypeTrameId
{
    Numeric = 0,
    Alphabetic = 1,
    Boolean = 2,
    NumericTab = 3
}
internal static class FrameConstruct
{
    public static byte[] ConstructTrajectory(PointData[] path)
    {
        byte[] trame = ConstructPath(path, TrameId.Trajectory);
        return trame;
    }

    public static byte[] ConstructCheckppoints(PointData[] path)
    {
        byte[] trame = ConstructPath(path, TrameId.Checkppoints);
        return trame;
    }

    private static byte[] ConstructPath(PointData[] path, TrameId id)
    {
        List<byte> trame = new List<byte>();

        // Ajout de l'ID à la trame
        trame.Add((byte)id);

        // Ajout du nombre de points codé sur 2 bytes
        var pointsCount = (ushort)path.Length;
        trame.AddRange(BitConverter.GetBytes(pointsCount));

        // Ajout de tous les x et y
        // chaque point est composé de 1 x puis 1 y chacun codé sur un double de 8 bytes
        for (int i = 0; i < path.Length; i++)
        {
            trame.AddRange(BitConverter.GetBytes(path[i].Xd));
            trame.AddRange(BitConverter.GetBytes(path[i].Yd));
        }

        return trame.ToArray();
    }

    public static byte[] ConstructAskConfiguration()
    {
        return ConstructMessage(0, TrameId.Configuration);
    }

    public static byte[] ConstructLevelLoaded(int levelNum)
    {
        return ConstructMessage(levelNum, TrameId.LevelLoaded);
    }

    public static byte[] ConstructLevelStarted()
    {
        return ConstructMessage(0, TrameId.LevelStarted);
    }

    public static byte[] ConstructLevelStopped()
    {
        return ConstructMessage(0, TrameId.LevelStopped);
    }

    public static byte[] ConstructCheckpointReached(int checkpointNum)
    {
        return ConstructMessage(checkpointNum, TrameId.CheckpointReached);
    }

    private static byte[] ConstructMessage(object data, TrameId id)
    {
        List<byte> trame = new List<byte>();

        // Ajout de l'ID à la trame
        trame.Add((byte)id);

        // Ajoute une valeur par défaut sur les 4 bytes reservé aux data pour que la structure de la trame soit toujours correcte
        var placeholder = BitConverter.GetBytes((int)data);
        trame.AddRange(placeholder);

        return trame.ToArray();
    }

    public static byte[] ConstructValue(string key, object data)
    {
        List<byte> trame = new List<byte>();

        // Ajout de l'ID à la trame
        trame.Add((byte)TrameId.SetValue);

        // Ajout de la taille de la key
        var keyCount = (ushort)Encoding.ASCII.GetByteCount(key);
        trame.AddRange(BitConverter.GetBytes(keyCount));
        // Ajout de la key
        trame.AddRange(Encoding.ASCII.GetBytes(key));

        // Détermine le type de data et la taille de la data
        byte typeData = 0;
        ushort dataCount = 0;
        byte[] dataByte = new byte[1];
        List<byte[]> dataByteTab = new List<byte[]>();
        if (data is string)
        {
            typeData = (byte)DataTypeTrameId.Alphabetic;
            dataCount = (ushort)Encoding.ASCII.GetByteCount(data as string);
            dataByte = Encoding.ASCII.GetBytes(data as string);
        }
        else if (data is double)
        {
            typeData = (byte)DataTypeTrameId.Numeric;
            dataByte = BitConverter.GetBytes((double)data);
            dataCount = (ushort)dataByte.Length;
        }
        else if (data is bool)
        {
            typeData = (byte)DataTypeTrameId.Boolean;
            dataByte = BitConverter.GetBytes((bool)data);
            dataCount = (ushort)dataByte.Length;
        }
        else if (data is int[])
        {
            typeData = (byte)DataTypeTrameId.NumericTab;
            dataCount = (ushort)(data as int[]).Length;
            foreach (var item in data as int[])
            {
                var dataTab = BitConverter.GetBytes(item);
                dataByteTab.Add(dataTab);
            }
        }

        // Ajout du type de data à la trame
        trame.Add(typeData);
        // Ajout de la taille de la data
        trame.AddRange(BitConverter.GetBytes(dataCount));

        if (typeData == (byte)DataTypeTrameId.NumericTab)
        {
            // Ajout de chaque int sous forme de byte
            foreach (var item in dataByteTab)
            {
                // Ajout de la data
                trame.AddRange(item);
            }
        }
        else
        {
            // Ajout de la data
            trame.AddRange(dataByte);
        }

        return trame.ToArray();
    }
}
internal static class FrameDeconstruct
{
    private static int indiceTrame;

    public static object Deconstruct(byte[] trame, out TrameId trameId)
    {
        // Valeurs par défaut des objets qui seront envoyés si il y a un problème avec le dechiffrage de la trame
        object result = new object();
        trameId = TrameId.Null;
        indiceTrame = 0;

        try
        {
            // Lecture du premier byte qui correspond TOUJOURS à l'Id
            byte id = trame[indiceTrame];
            trameId = (TrameId)id;
            indiceTrame++;

            switch (trameId)
            {
                case TrameId.Configuration:
                    result = DeconstructConfiguration(trame);
                    break;
                case TrameId.Positions:
                    result = DeconstructPoint(trame);
                    break;
                case TrameId.Positions2:
                    result = DeconstructPoint(trame);
                    break;
                case TrameId.Forces:
                    result = DeconstructPoint(trame);
                    break;
                case TrameId.Forces2:
                    result = DeconstructPoint(trame);
                    break;
                case TrameId.PauseGame:
                    result = DeconstructMessage(trame);
                    break;
                case TrameId.StopGame:
                    result = DeconstructMessage(trame);
                    break;
                case TrameId.SetValue:
                    result = DeconstructValue(trame);
                    break;
                case TrameId.NextLevel:
                    result = DeconstructMessage(trame);
                    break;
                case TrameId.Null:
                    break;
                default:
                    break;
            }
        }
        catch (Exception)
        {

        }

        return result;
    }

    private static ReaLabConfiguration DeconstructConfiguration(byte[] trame)
    {
        ReaLabConfiguration config;

        // Taille du nom codé sur 2 byte car c'est un ushort
        ushort lastNameCount = (ushort)BitConverter.ToInt16(trame, indiceTrame);
        indiceTrame += 2;
        string lastName = Encoding.ASCII.GetString(trame, indiceTrame, lastNameCount);
        indiceTrame += lastNameCount;

        // Taille du prénom codé sur 2 byte car c'est un ushort
        ushort firstNameCount = (ushort)BitConverter.ToInt16(trame, indiceTrame);
        indiceTrame += 2;
        string firstName = Encoding.ASCII.GetString(trame, indiceTrame, firstNameCount);
        indiceTrame += firstNameCount;

        // Année de naissance codé sur 2 byte car c'est un ushort
        ushort year = (ushort)BitConverter.ToInt16(trame, 0);
        indiceTrame += 2;
        // Mois de naissance codé sur un byte
        int month = trame[indiceTrame];
        indiceTrame++;
        // Jour de naissance codé sur un byte
        int day = trame[indiceTrame];
        indiceTrame++;

        // Sexe du patient sur 1 byte car char
        char sexe = Encoding.ASCII.GetString(trame, indiceTrame, 1)[0];
        indiceTrame++;

        // Difficulté du jeu codé sur un byte
        int difficulty = trame[indiceTrame];
        indiceTrame++;
        // Zoom du jeu codé sur un byte
        int zoom = trame[indiceTrame];
        indiceTrame++;
        // Niveau à charger codé sur un byte
        int level = trame[indiceTrame];
        indiceTrame++;
        // Etat du debug
        byte debugByte = trame[indiceTrame];
        indiceTrame++;
        bool debug = (debugByte == 0) ? false : true;

        config = new ReaLabConfiguration(lastName, firstName, new DateTime(year, month, day), difficulty, zoom, level, debug);
        return config;
    }

    private static PointData DeconstructPoint(byte[] trame)
    {
        // lecture de la coordonnée x
        double x = BitConverter.ToDouble(trame, indiceTrame);
        indiceTrame += 8;

        // lecture de la coordonnée y
        double y = BitConverter.ToDouble(trame, indiceTrame);
        indiceTrame += 8;

        var point = new PointData(x, y);

        return point;
    }

    private static int DeconstructMessage(byte[] trame)
    {
        if (trame.Length >= 4)
        {
            // lecture de la data codé sur 4 bytes (int)
            return BitConverter.ToInt32(trame, indiceTrame);
        }
        else
        {
            return -1;
        }
    }

    private static ValueData DeconstructValue(byte[] trame)
    {
        // Taille de la key codé sur 2 byte car c'est un ushort
        ushort keyCount = (ushort)BitConverter.ToInt16(trame, indiceTrame);
        indiceTrame += 2;
        string key = Encoding.ASCII.GetString(trame, indiceTrame, keyCount);
        indiceTrame += keyCount;

        // Type de data codé sur 1 byte
        byte dataTypeByte = trame[indiceTrame];
        DataTypeTrameId dataType = (DataTypeTrameId)dataTypeByte;
        indiceTrame++;

        // Taille de la data codé sur 2 byte car c'est un ushort
        ushort dataCount = (ushort)BitConverter.ToInt16(trame, indiceTrame);
        indiceTrame += 2;

        object data = new object();

        switch (dataType)
        {
            case DataTypeTrameId.Numeric:
                data = BitConverter.ToDouble(trame, indiceTrame);
                break;
            case DataTypeTrameId.Alphabetic:
                data = Encoding.ASCII.GetString(trame, indiceTrame, dataCount);
                break;
            case DataTypeTrameId.Boolean:
                data = BitConverter.ToBoolean(trame, indiceTrame);
                break;
            case DataTypeTrameId.NumericTab:
                var datatemp = new List<int>();
                for (int i = 0; i < dataCount; i++)
                {
                    int dataint = BitConverter.ToInt32(trame, indiceTrame);
                    datatemp.Add(dataint);
                    indiceTrame += 4;
                }
                data = datatemp.ToArray();
                break;
            default:
                break;
        }

        return new ValueData(key, dataType, data);
    }
}
#endregion

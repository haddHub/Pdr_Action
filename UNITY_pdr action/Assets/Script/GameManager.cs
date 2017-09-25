using UnityEngine;
using System.Collections;
using System;

public enum GameState
{
    Init,
    LevelLoading,
    Positioning,
    Countdown,
    Playing,
    Transition,
    Pause,
    Stop,
    Waiting,
    PauseTarget,
    HandTarget,
    End
}

public class GameManager : MonoBehaviour {

    /// <summary>
    /// Instance courante du game manager
    /// </summary>
    private static GameManager _instance = null;

    /// <summary>
    /// Etat actuel du jeu
    /// </summary>
    private GameState _state;

    /// <summary>
    /// Instance du state précédent
    /// </summary>
    private GameState _previousState;

    /// <summary>
    /// Gets l'instance courante du game manager
    /// </summary>
    public static GameManager Instance { get { return _instance; } }

    [HideInInspector]
    public ReaLabCommunication client;

    public ReaLabConfiguration Config { get; set; }

    /// <summary>
    /// Gets l'état actuel du jeu
    /// </summary>
    public GameState State { get { return _state; } }

    public bool DebugEditorMod;

    [Range(0,5)]
    public int LevelToLoad;

    private bool canStart = false;
    private bool canStop = false;
    private bool canNextLvl = false;

    private bool pause = false;

    public event EventHandler onGameStateChanged;

    void Awake()
    {

        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(this.gameObject);
        }
        
        // Le game manager ne sera pas détruit au chargement d'une nouvelle scène
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        if (DebugEditorMod == false)
        {
            this.client = GameObject.FindGameObjectWithTag("Communication").GetComponent<ReaLabCommunication>();
            this.client.Connect();
            this.client.onPause += Client_onPause;
            this.client.onNewConfiguration += Client_onNewConfiguration;
            this.client.onStop += Client_onStop;
            this.client.onNextLevel += Client_onNextLevel;
        }
        else
        {
            var config = new ReaLabConfiguration("debug", "Debug", new DateTime(), 0, 0, LevelToLoad, true);
            Config = config;
            StartGame();
        }
    }

    private void Client_onNextLevel(object obj, EventArgs nextLevelArgs)
    {
        Debug.Log("Next");
        SetGameState(GameState.Waiting);
        canNextLvl = true;
    }

    private void Client_onStop(object obj, EventArgs stopArgs)
    {
        this.client.Disconnect();
        this.client.onPause -= Client_onPause;
        this.client.onNewConfiguration -= Client_onNewConfiguration;
        this.client.onNextLevel -= Client_onNextLevel;
        this.client = null;
        canStop = true;
    }

    void OnApplicationQuit()
    {
        Debug.Log("Quit");
        if (this.client != null)
        {
            Debug.Log("Disco Quit");
            this.client.Disconnect();
            this.client.onPause -= Client_onPause;
            this.client.onNewConfiguration -= Client_onNewConfiguration;
            this.client.onNextLevel -= Client_onNextLevel;
            this.client = null;
        }
    }

    // Update is called once per frame
    void Update () {

        if (canStart == true)
        {
            canStart = false;
            StartGame();
        }

        if (canStop == true)
        {
            canStop = false;
            Application.Quit();
        }

        if (canNextLvl == true)
        {
            canNextLvl = false;
            Application.LoadLevel("waiting");
        }
    }

    private void Client_onNewConfiguration(object obj, ConfigEvent configArgs)
    {
        this.Config = configArgs.Config;
        canStart = true;
    }

    void StartGame()
    {
        this.SetGameState(GameState.Init);

        try
        {
            if (this.Config != null)
            {
                this.SetGameState(GameState.LevelLoading);

                // Load de la scene correspondant au niveau
                if (Config.LevelToLoad == 0)
                {
                    Application.LoadLevel("amplitude");
                }
                else if (Config.LevelToLoad == 1)
                {
                    Application.LoadLevel("force");
                }
                else if (Config.LevelToLoad == 2)
                {
                    Application.LoadLevel("targets");
                }
                else if (Config.LevelToLoad == 3)
                {
                    Application.LoadLevel("targetsForce");
                }
                else if (Config.LevelToLoad == 4)
                {
                    Application.LoadLevel("amplitudeArticulaire");
                }
                else if (Config.LevelToLoad == 5)
                {
                    Application.LoadLevel("Targets couloir");
                }
            }
            else
            {
                Debug.Log("null");
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    private void Client_onPause(object obj, EventArgs pauseArgs)
    {
        if (!pause)
        {
            if (this.State != GameState.Pause && this.State != GameState.Stop)
            {
                this.SetGameState(GameState.Pause);
                pause = !pause;
            }
        }
        else
        {
            if (this.State == GameState.Pause)
            {
                this.SetGameState(this._previousState);
                pause = !pause;
            }
        }
    }

    /// <summary>
    /// Change le state du jeu
    /// </summary>
    /// <param name="newState">nouveau state que le jeu doit adopter</param>
    public void SetGameState(GameState newState)
    {
        // Ne pas écraser le state précedent si le même état est lancé plusieur fois de suite
        if (newState != this._state)
        {
            this._previousState = this._state;
        }

        this._state = newState;

        switch (newState)
        {
            case GameState.Init:
                break;
            case GameState.LevelLoading:
                break;
            case GameState.Positioning:
                Debug.Log("Positioning");
                break;
            case GameState.Countdown:
                break;
            case GameState.Playing:
                Debug.Log("Playing");
                break;
            case GameState.Transition:
                break;
            case GameState.Pause:
                break;
            case GameState.Stop:
                Debug.Log("Stop");
                break;
            case GameState.Waiting:
                break;
            case GameState.End:
                break;
            default:
                break;
        }

        if (onGameStateChanged != null)
        {
            onGameStateChanged(this, EventArgs.Empty);
        }
    }
}

public static class GameUtils
{
    /// <summary>
    /// Converti un point en pixel 1920*1080 en world point en tenant compte de la zone de jeu pour que la convertion soit bonne aussi dans l'éditeur
    /// </summary>
    /// <param name="point">Le point en pixel</param>
    /// <param name="cam">La caméra de la scene</param>
    /// <returns>Le point en world point</returns>
    public static Vector3 PixelToWorldPoint(Vector3 point, Camera cam)
    {
        // La conversion des pixel en world point dépend de la zone visible par la caméra
        // Cette zone est l'écran de jeu dont la taille peut varier d'un éditeur à l'autre
        // Pour que la conversion se fasse correctement indépendament de la taille de la zone de jeu 
        // Il faut mettre à l'echelle de la caméra les point en pixel utilisé. La résolution de base est 1920*1080

        var rectCam = cam.pixelRect;

        float xEchelle = ((float)point.x / 1920f) * rectCam.width;
        // Le 0;0 de Unity (bas gauche) n'est pas le même que celui d'une fenetre classique (haut gauche)
        // pour ramener le point dans le bon repère il faut faire 1080 - coord y
        float yEchelle = ((1080f - (float)point.y) / 1080f) * rectCam.height;
        var pixelEchelle = new Vector3(xEchelle, yEchelle, 0f);

        var worldPoint = cam.ScreenToWorldPoint(pixelEchelle);
        // Redéfini le z pour l'objet soit toujours visible par la caméra
        worldPoint.z = 0;

        return worldPoint;
    }
}

using UnityEngine;
using System.Collections.Generic;
using System;

public class TargetManagerCouloir : MonoBehaviour, Trigger2DListener {

    /// <summary>
    /// Instance du game manager
    /// </summary>
    private GameManager _gameManager;

    public bool Force = false;

    [Range(1, 10)]
    public int LeftRepetion;
    private int saveLeftRepetion;

    [Range(1, 10)]
    public int RightRepetion;
    private int saveRightRepetion;

    private List<Transform> leftTarget = new List<Transform>();
    private List<Transform> rightTarget = new List<Transform>();
    private List<Transform> leftTargetremain;
    private List<Transform> rightTargetremain;
    private int[] leftTargetBase;
    private int[] rightTargetBase;

    private System.Random leftRand = new System.Random();
    private System.Random rightRand = new System.Random();

    private Transform currentTarget;
    private Transform previousTarget;

    [HideInInspector]
    public int CurrentTargetNumber;

    private bool canTimer = false;
    private bool inside = false;
    public float GeneralTimer = 20f;
    private float _generalTimer = 0f;
    public float OutsideTimer = 7f;
    private float _outsideTimer = 0f;

    private InitZoneCouloir initZoneCouloir;
    private bool canSendToInitZone = false;
    private float initTimer = 7f;

    private int nbrTargetBeforePause;

    // Quand passe a true, on a bien recu les cibles et repets de realab
    private bool canManageTarget = false;

    private int leftTargetRep;
    private int rightTargetRep;

    private CouloirCreator couloir;
    private Transform startingPoint;
    void Awake()
    {
        // Trouve le game object game manager et instancie le field
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        _gameManager.onGameStateChanged += _gameManager_onGameStateChanged;
        if (_gameManager.client != null)
        {
            _gameManager.client.onNewValue += Client_onNewValue;
        }

        couloir = GameObject.FindGameObjectWithTag("CouloirManager").GetComponent<CouloirCreator>();
        couloir.AddListener(this);
        startingPoint = GameObject.FindGameObjectWithTag("StartingPoint").GetComponent<Transform>();
        initZoneCouloir = GameObject.Find("Initiation Zone").GetComponent<InitZoneCouloir>();
    }

    void Start()
    {
        if (_gameManager.client != null)
        {
            // Astuce pour faire comprendre a REALab qu'il peut envoyer les cibles cochées sur l'interface ainsi que les répétiotions gauche droit
            _gameManager.client.SetTrajectory(new PointData[] { });
            _gameManager.client.SetValue("AvantPause", true);
        }
        else
        {
            // Mode debug
            LeftRepetion = 1;
            saveLeftRepetion = LeftRepetion;
            RightRepetion = 1;
            saveRightRepetion = RightRepetion;
            leftTargetBase = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };//
            rightTargetBase = new int[] { 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };// 
            ManageTargetsFromReaLab();
        }
    }

    void Update()
    {
        if (canManageTarget == true)
        {
            canManageTarget = false;
            ManageTargetsFromReaLab();
        }

        if (_gameManager != null && _gameManager.State == GameState.Playing && canTimer)
        {
            _generalTimer += Time.deltaTime;
            Debug.Log("General Timer : " + _generalTimer);

            if (inside == false)
            {
                _outsideTimer += Time.deltaTime;
                Debug.Log("Outside Timer : " + _outsideTimer);
            }

            if ((_generalTimer >= GeneralTimer) || (_outsideTimer >= OutsideTimer))
            {
                canTimer = false;
                // le temps général est écoulé, il faut passer à la cible suivante
                if (_gameManager.client != null)
                {
                    _gameManager.client.CheckpointReached(CurrentTargetNumber);
                }

                _gameManager.SetGameState(GameState.Positioning);
                NextTarget();
            }
        }

        if (canSendToInitZone)
        {
            canSendToInitZone = false;
            initZoneCouloir.InitTimer = initTimer;
        }
    }

    private void Client_onNewValue(object obj, ValueEvent valueArgs)
    {
        if (valueArgs.Key == "RepetitionGauche")
        {
            LeftRepetion = Convert.ToInt32(valueArgs.Value);
            saveLeftRepetion = LeftRepetion;
        }
        else if (valueArgs.Key == "RepetitionDroit")
        {
            RightRepetion = Convert.ToInt32(valueArgs.Value);
            saveRightRepetion = RightRepetion;
        }
        else if (valueArgs.Key == "CiblesGauche")
        {
            leftTargetBase = (valueArgs.Value as int[]);
        }
        else if (valueArgs.Key == "CiblesDroite")
        {
            rightTargetBase = (valueArgs.Value as int[]);
        }
        else if (valueArgs.Key == "GeneralTimer")
        {
            GeneralTimer = Convert.ToInt32(valueArgs.Value);
        }
        else if (valueArgs.Key == "OutsideTimer")
        {
            OutsideTimer = Convert.ToInt32(valueArgs.Value);
            canManageTarget = true;
        }
        else if (valueArgs.Key == "InitTimer")
        {
            initTimer = Convert.ToInt32(valueArgs.Value);
            canSendToInitZone = true;
        }
    }

    private void ManageTargetsFromReaLab()
    {
        foreach (Transform child in transform)
        {
            foreach (Transform target in child)
            {
                int targetNumber = 0;

                if (Force == true)
                {
                    targetNumber = target.GetComponent<TargetCatchForce>().TargetNumber;
                }
                else
                {
                    targetNumber = target.GetComponent<TargetCatch>().TargetNumber;
                }

                if (child.name == "Left Target")
                {
                    if (leftTargetBase.Length > 0)
                    {
                        foreach (int item in leftTargetBase)
                        {
                            if (item == targetNumber)
                            {
                                leftTarget.Add(target);
                            }
                        }
                    }
                }
                else
                {
                    if (rightTargetBase.Length > 0)
                    {
                        foreach (int item in rightTargetBase)
                        {
                            if (item == targetNumber)
                            {
                                rightTarget.Add(target);
                            }
                        }
                    }
                }
            }
        }

        leftTargetremain = new List<Transform>(leftTarget);
        rightTargetremain = new List<Transform>(rightTarget);

        nbrTargetBeforePause = ((leftTarget.Count * LeftRepetion) + (rightTarget.Count * RightRepetion)) / 2;
        leftTargetRep = (leftTarget.Count * saveLeftRepetion) / 2;
        rightTargetRep = (rightTarget.Count * saveRightRepetion) / 2;
        NextTarget();
    }

    private void _gameManager_onGameStateChanged(object sender, EventArgs e)
    {
        if (_gameManager.State == GameState.Playing)
        {
            ShowNewTarget();
        }
        else if (_gameManager.State == GameState.Countdown || _gameManager.State == GameState.PauseTarget || _gameManager.State == GameState.Stop)
        {
            if (previousTarget != null)
                previousTarget.gameObject.SetActive(false);
        }
    }

    public void NextTarget()
    {
        canTimer = false;
        inside = false;
        ResetTimers();

        previousTarget = currentTarget;

        nbrTargetBeforePause--;

        if (nbrTargetBeforePause == -1)
        {
            leftTargetRep = (leftTarget.Count * saveLeftRepetion) / 2;
            rightTargetRep = (rightTarget.Count * saveRightRepetion) / 2;
            _gameManager.SetGameState(GameState.PauseTarget);
            if (_gameManager.client != null)
            {
                _gameManager.client.SetValue("AvantPause", false);
            }
        }

        // 1.Il faut choisir entre une cilbe à gauche ou à droite
        var side = ChoseLeftOrRight();

        // 2.Il faut choisir random une cible du bon coté
        if (side == 0)
        {
            // gauche
            var indice = leftRand.Next(0, leftTargetremain.Count);
            currentTarget = leftTargetremain[indice];
            Vector3 worldPos = Camera.main.WorldToScreenPoint(currentTarget.position);
            leftTargetremain.RemoveAt(indice);
            if (leftTargetremain.Count <= 0)
            {
                LeftRepetion--;

                if (LeftRepetion > 0)
                {
                    leftTargetremain = new List<Transform>(leftTarget);
                }
            }

            if (_gameManager.client != null)
            {
                _gameManager.client.SetValue("Gauche", true);
                _gameManager.client.SetValue("TargetX", worldPos.x);
                _gameManager.client.SetValue("TargetY", worldPos.y);
            }
        }
        else if (side == 1)
        {
            // droite
            var indice = rightRand.Next(0, rightTargetremain.Count);
            currentTarget = rightTargetremain[indice];
            Vector3 worldPos = Camera.main.WorldToScreenPoint(currentTarget.position);
            rightTargetremain.RemoveAt(indice);
            if (rightTargetremain.Count <= 0)
            {
                RightRepetion--;

                if (RightRepetion > 0)
                {
                    rightTargetremain = new List<Transform>(rightTarget);
                }
            }

            if (_gameManager.client != null)
            {
                _gameManager.client.SetValue("Gauche", false);
                _gameManager.client.SetValue("TargetX", worldPos.x);
                _gameManager.client.SetValue("TargetY", worldPos.y);
            }
        }
        else
        {
            currentTarget = null;
            _gameManager.SetGameState(GameState.End);
            if (_gameManager.client != null)
            {
                _gameManager.client.LevelStopped();
            }
            // Fin de l'exerice
            Debug.Log("Fin");
        }

        if(currentTarget != null && couloir != null && startingPoint != null)
        {
            Debug.Log("Couloir Setup");
            couloir.SetUp(startingPoint.position, currentTarget.position);
            couloir.Enabled(false);
        }
    }

    private void ResetTimers()
    {
        _generalTimer = 0f;
        _outsideTimer = 0f;
    }

    public void InitZoneExited()
    {
        couloir.Enabled(true);
        canTimer = true;
    }

    /// <returns>0 = gauche 1 = droite -1 = rien</returns>
    private int ChoseLeftOrRight()
    {
        int leftOrRight = 0;
        if (leftTargetRep > 0 && rightTargetRep > 0)
        {
            var rand = UnityEngine.Random.value;
            if (rand <= 0.5f)
            {
                leftOrRight = 0;
                leftTargetRep--;
            }
            else
            {
                leftOrRight = 1;
                rightTargetRep--;
            }
        }
        else if (leftTargetRep > 0 && rightTargetRep <= 0)
        {
            leftOrRight = 0;
            leftTargetRep--;
        }
        else if (leftTargetRep <= 0 && rightTargetRep > 0)
        {
            leftOrRight = 1;
            rightTargetRep--;
        }
        else
        {
            leftOrRight = -1;
        }

        return leftOrRight;
    }

    private void ShowNewTarget()
    {
        if (currentTarget != null)
        {
            if (previousTarget != null)
                previousTarget.gameObject.SetActive(false);
            currentTarget.gameObject.SetActive(true);
        }
    }

    void Trigger2DListener.Trigger2DEnter(string tag)
    {
        inside = true;
        _outsideTimer = 0f;
    }

    void Trigger2DListener.Trigger2DStay(string tag)
    {

    }

    void Trigger2DListener.Trigger2DExit(string tag)
    {
        inside = false;
        _outsideTimer = 0f;
    }
}

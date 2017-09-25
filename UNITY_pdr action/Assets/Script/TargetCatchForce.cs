using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TargetCatchForce : MonoBehaviour {

    /// <summary>
    /// Instance du game manager
    /// </summary>
    private GameManager _gameManager;
    private TargetsManager targetManager;

    public int TimeToStay = 1;
    private float timeToStay;

    private float initTime = 0f;
    private bool canInit = true;

    public int TargetNumber;
    public int ForceMax = 10;
    public Image ValidationImage;

    private bool canShowValidation = false;

    void Awake()
    {
        // Trouve le game object game manager et instancie le field
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        _gameManager.onGameStateChanged += _gameManager_onGameStateChanged;
        targetManager = GameObject.FindGameObjectWithTag("TargetsManager").GetComponent<TargetsManager>();
    }

    private void _gameManager_onGameStateChanged(object sender, System.EventArgs e)
    {
        if (_gameManager.State == GameState.Countdown || _gameManager.State == GameState.PauseTarget)
        {
            ValidationImage.fillAmount = 0f;
        }
    }

    void Start()
    {
        RefreshTimeToStay();
        initTime = 0f;
        canInit = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            DebugForce(0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            DebugForce(1);
        }

        if (canInit == true)
        {
            initTime += Time.deltaTime;
        }

        if (canShowValidation == true)
        {
            timeToStay -= Time.deltaTime;

            ValidationImage.fillAmount = 1f - timeToStay;

            if (timeToStay <= 0)
            {
                canShowValidation = false;

                NextTarget();
                // Reset le temps de validation pour l'éventuelle prochaine fois
                RefreshTimeToStay();
            }
        }
    }

    private void RefreshTimeToStay()
    {
        timeToStay = TimeToStay;
    }

    private void OnEnable()
    {
        canInit = true;
        if (_gameManager != null && _gameManager.client != null)
        {
            _gameManager.client.onNewForces += Client_onNewForces;
        }
    }

    private void Client_onNewForces(object obj, PointEvent forcesArgs)
    {
        if (_gameManager.State == GameState.Playing)
        {
            if (forcesArgs.Point.Yf >= ForceMax)
            {
                SendInitTime();

                // le mouvement est validé comme un mouvement vers le haut.
                _gameManager.client.onNewForces -= Client_onNewForces;

                _gameManager.client.SetValue("Reponse", "Haut");

                if (transform.parent.name.Contains("Left"))
                {
                    _gameManager.client.SetValue("GaucheForce", true);
                    // bonne réponse
                    AfficherValidation();
                }
                else
                {
                    _gameManager.client.SetValue("GaucheForce", false);
                    // mauvaise réponse
                    NextTarget();
                }
            }
            else if (forcesArgs.Point.Yf <= (ForceMax * -1))
            {
                SendInitTime();
                // le mouvement est validé comme un mouvement vers le bas.
                _gameManager.client.SetValue("Reponse", "Bas");

                _gameManager.client.onNewForces -= Client_onNewForces;

                if (transform.parent.name.Contains("Left"))
                {
                    _gameManager.client.SetValue("GaucheForce", true);
                    // mauvaise réponse
                    NextTarget();
                }
                else
                {
                    _gameManager.client.SetValue("GaucheForce", false);
                    // bonne réponse
                    AfficherValidation();
                }
            }
        }
    }

    private void SendInitTime()
    {
        canInit = false;
        _gameManager.client.SetValue("InitTimeForce", (double)initTime);
        initTime = 0f;
    }

    private void NextTarget()
    {
        if (_gameManager != null && _gameManager.client != null)
        {
            _gameManager.client.CheckpointReached(TargetNumber);
            // Repasser en mode positionning
            _gameManager.SetGameState(GameState.Positioning);
            targetManager.NextTarget();
        }  
    }

    private void AfficherValidation()
    {
        canShowValidation = true;
    }

    private void OnDisable()
    {
        if (_gameManager != null && _gameManager.client != null)
        {
            _gameManager.client.onNewForces -= Client_onNewForces;
        }

        RefreshTimeToStay();
    }

    private void DebugForce(int dir)
    {
        // haut
        if (dir == 0)
        {
            SendInitTime();
            // le mouvement est validé comme un mouvement vers le haut.
            _gameManager.client.onNewForces -= Client_onNewForces;

            _gameManager.client.SetValue("Reponse", "Haut");

            if (transform.parent.name.Contains("Left"))
            {
                _gameManager.client.SetValue("GaucheForce", true);
                // bonne réponse
                AfficherValidation();
            }
            else
            {
                _gameManager.client.SetValue("GaucheForce", false);
                // mauvaise réponse
                NextTarget();
            }
        }
        else
        {
            SendInitTime();
            // le mouvement est validé comme un mouvement vers le bas.
            _gameManager.client.SetValue("Reponse", "Bas");

            _gameManager.client.onNewForces -= Client_onNewForces;

            if (transform.parent.name.Contains("Left"))
            {
                _gameManager.client.SetValue("GaucheForce", true);
                // mauvaise réponse
                NextTarget();
            }
            else
            {
                _gameManager.client.SetValue("GaucheForce", false);
                // bonne réponse
                AfficherValidation();
            }
        }
    }
}

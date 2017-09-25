using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TargetCatch : MonoBehaviour {

    /// <summary>
    /// Instance du game manager
    /// </summary>
    private GameManager _gameManager;
    private TargetsManager targetManager;
    private TargetManagerCouloir targetCouloir;

    public int TimeToStay = 1;
    private float timeToStay;

    public int TargetNumber;

    public Image ValidationImage;

    void Awake()
    {
        // Trouve le game object game manager et instancie le field
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        _gameManager.onGameStateChanged += _gameManager_onGameStateChanged;
        targetManager = GameObject.FindGameObjectWithTag("TargetsManager").GetComponent<TargetsManager>();
        targetCouloir = GameObject.FindGameObjectWithTag("TargetsManager").GetComponent<TargetManagerCouloir>();
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
        if (targetCouloir != null)
        {
            targetCouloir.CurrentTargetNumber = TargetNumber;
        }
    }

    private void RefreshTimeToStay()
    {
        timeToStay = TimeToStay;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (_gameManager != null && _gameManager.State == GameState.Playing)
            {
                timeToStay -= Time.deltaTime;

                ValidationImage.fillAmount = 1f - timeToStay;

                if (timeToStay <= 0)
                {
                    // Signaler à réalab que la cible est validée
                    if (_gameManager.client != null)
                        _gameManager.client.CheckpointReached(TargetNumber);
                    // Repasser en mode positionning
                    _gameManager.SetGameState(GameState.Positioning);

                    if (targetManager != null)
                        targetManager.NextTarget();
                    else
                        targetCouloir.NextTarget();
                    // Reset le temps de validation pour l'éventuelle prochaine fois
                    RefreshTimeToStay();
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (_gameManager != null && _gameManager.State == GameState.Playing)
            {
                RefreshTimeToStay();
            }
        }
    }
}

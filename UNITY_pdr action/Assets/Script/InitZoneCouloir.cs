using UnityEngine;
using System.Collections;

public class InitZoneCouloir : MonoBehaviour {

    /// <summary>
    /// Instance du game manager
    /// </summary>
    private GameManager _gameManager;

    private float initTime = 0f;
    private CircleCollider2D myCollider;
    private TargetManagerCouloir targetManagerCouloir;

    public float InitTimer = 7f;

    void Awake()
    {
        // Trouve le game object game manager et instancie le field
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        _gameManager.onGameStateChanged += _gameManager_onGameStateChanged;
        myCollider = gameObject.GetComponent<CircleCollider2D>();
        targetManagerCouloir = GameObject.FindGameObjectWithTag("TargetsManager").GetComponent<TargetManagerCouloir>();
    }

    

    private void _gameManager_onGameStateChanged(object sender, System.EventArgs e)
    {
        if (_gameManager != null)
        {
            if (_gameManager.State == GameState.Positioning)
            {
                myCollider.enabled = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (_gameManager != null)
            {
                if (_gameManager.State == GameState.Playing)
                {
                    if (_gameManager.client != null)
                    {
                        _gameManager.client.SetValue("InitTime", (double)initTime);
                    }

                    ResetInitTime();
                    
                    targetManagerCouloir.InitZoneExited();
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (_gameManager != null)
            {
                if (_gameManager.State == GameState.Playing)
                {
                    initTime += Time.deltaTime;
                    Debug.Log("Init Timer : " + initTime);

                    if (initTime >= InitTimer)
                    {
                        ResetInitTime();
                        if (_gameManager.client != null)
                        {
                            _gameManager.client.SetValue("InitTime", (double)InitTimer);
                            _gameManager.client.CheckpointReached(targetManagerCouloir.CurrentTargetNumber);
                        }

                        _gameManager.SetGameState(GameState.Positioning);
                        targetManagerCouloir.NextTarget();
                    }
                }
            }
        }
    }

    private void ResetInitTime()
    {
        initTime = 0f;
        myCollider.enabled = false;
    }
}

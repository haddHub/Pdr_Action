using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartingPointTarget : MonoBehaviour {

    public float TimeToStayBeforeCountdown;
    private float timeToStayBeforeCountdown;
    public bool SelfDestruct = true;

    public Text EndText;
    /// <summary>
    /// Instance du game manager
    /// </summary>
    private GameManager _gameManager;

    void Awake()
    {
        // Trouve le game object game manager et instancie le field
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        if (_gameManager != null)
        {
            Debug.Log("Game Manager trouvé dans Level Manager");
        }
        else
        {
            Debug.Log("Game Manager pas trouvé dans Level Manager");
        }
    }

    // Use this for initialization
    void Start()
    {

        if (_gameManager != null)
        {
            timeToStayBeforeCountdown = TimeToStayBeforeCountdown;

            _gameManager.SetGameState(GameState.Positioning);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (_gameManager != null)
            {
                if (_gameManager.State == GameState.Positioning)
                {
                    timeToStayBeforeCountdown -= Time.deltaTime;
                    Debug.Log(timeToStayBeforeCountdown);
                    if (timeToStayBeforeCountdown <= 0)
                    {
                        Debug.Log("Countdown");
                        _gameManager.SetGameState(GameState.Countdown);

                        if (SelfDestruct == true)
                        {
                            Destroy(this.gameObject, 3f);
                        }

                        TimeToStayBeforeCountdown = 0.5f;
                        timeToStayBeforeCountdown = TimeToStayBeforeCountdown;
                    }
                }
                else if (_gameManager.State == GameState.End)
                {
                    EndText.gameObject.SetActive(true);
                    _gameManager.SetGameState(GameState.Stop);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            timeToStayBeforeCountdown = TimeToStayBeforeCountdown;
        }
    }
}
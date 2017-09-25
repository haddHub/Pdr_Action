using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CountdownTarget : MonoBehaviour {

    /// <summary>
    /// Instance du game manager
    /// </summary>
    private GameManager _gameManager;

    public int CountdownTime;
    private float countdownTime;

    public Text CountdownText;
    private bool activeText = false;

    void Awake()
    {
        // Trouve le game object game manager et instancie le field
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        _gameManager.onGameStateChanged += _gameManager_onGameStateChanged;
    }

    private void _gameManager_onGameStateChanged(object sender, System.EventArgs e)
    {
        if (_gameManager.State == GameState.PauseTarget)
        {
            CountdownTime = 3;
            countdownTime = CountdownTime;

            CountdownText.text = "PAUSE";
            activeText = true;
            CountdownText.gameObject.SetActive(true);
        }
        else if (_gameManager.State == GameState.HandTarget)
        {
            CountdownText.fontSize = 100;
            CountdownText.text = "CHANGEMENT DE MAIN";
        }
    }

    // Use this for initialization
    void Start()
    {

        countdownTime = CountdownTime;
        CountdownText.text = ((int)countdownTime).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager != null)
        {
            if (_gameManager.State == GameState.Countdown)
            {
                CountdownText.fontSize = 180;
                if (activeText == false)
                {
                    activeText = true;
                    CountdownText.gameObject.SetActive(true);
                }

                countdownTime -= Time.deltaTime;

                CountdownText.text = (Mathf.Ceil(countdownTime)).ToString();

                if (countdownTime <= 0)
                {
                    activeText = false;
                    CountdownText.gameObject.SetActive(false);
                    _gameManager.SetGameState(GameState.Playing);

                    if (_gameManager.client != null)
                    {
                        _gameManager.client.LevelStarted();
                    }

                    CountdownTime = 1;
                    countdownTime = CountdownTime;
                    CountdownText.text = ((int)countdownTime).ToString();
                }
            }
        }
    }
}
using UnityEngine;
using System.Collections;
using System;

public class PauseTarget : MonoBehaviour {

    /// <summary>
    /// Instance du game manager
    /// </summary>
    private GameManager _gameManager;

    public float PauseTimeEval;
    public float PauseTimeTraining;
    private float pauseTime;

    public float HandTimeEval;
    public float HandTimeTaining;
    private float handTime;

    void Awake()
    {
        // Trouve le game object game manager et instancie le field
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Use this for initialization
    void Start () {

        if (_gameManager != null)
        {
            if (_gameManager.client != null)
            {
                // Eval
                if (_gameManager.Config.GameDifficulty == 0)
                {
                    pauseTime = PauseTimeEval;
                    handTime = HandTimeEval;
                }
                else
                {
                    pauseTime = PauseTimeTraining;
                    handTime = HandTimeTaining;
                }
            }
            else
            {
                pauseTime = PauseTimeEval;
                handTime = HandTimeEval;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (_gameManager.State == GameState.PauseTarget)
        {
            pauseTime -= Time.deltaTime;

            if (pauseTime <= 0)
            {
                _gameManager.SetGameState(GameState.HandTarget);
            }
        }
        else if (_gameManager.State == GameState.HandTarget)
        {
            handTime -= Time.deltaTime;

            if (handTime <= 0)
            {
                _gameManager.SetGameState(GameState.Countdown);
            }
        }
    }
}

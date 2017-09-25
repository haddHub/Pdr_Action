using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseGameManager : MonoBehaviour {

    /// <summary>
    /// Instance du game manager
    /// </summary>
    private GameManager _gameManager;

    public Text PauseText;

    void Awake()
    {
        // Trouve le game object game manager et instancie le field
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Start()
    {
        _gameManager.onGameStateChanged += _gameManager_onGameStateChanged;
    }

    private void _gameManager_onGameStateChanged(object sender, System.EventArgs e)
    {
        if (_gameManager.State == GameState.Pause)
        {
            PauseText.gameObject.SetActive(true);
        }
        else
        {
            PauseText.gameObject.SetActive(false);
        }
    }
}

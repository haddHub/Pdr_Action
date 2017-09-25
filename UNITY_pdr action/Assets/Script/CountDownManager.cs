using UnityEngine;
using UnityEngine.UI;

public class CountDownManager : MonoBehaviour {

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
    }

    // Use this for initialization
    void Start () {

        countdownTime = CountdownTime;
        CountdownText.text = ((int)countdownTime).ToString();
    }
	
	// Update is called once per frame
	void Update () {
        if (_gameManager != null)
        {
            if (_gameManager.State == GameState.Countdown)
            {
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
                    _gameManager.client.LevelStarted();
                    this.gameObject.SetActive(false);
                }
            }
        }
	}
}

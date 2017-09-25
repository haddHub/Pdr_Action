using UnityEngine;
using System.Collections;

public class InitZone : MonoBehaviour {

    /// <summary>
    /// Instance du game manager
    /// </summary>
    private GameManager _gameManager;

    private float initTime = 0f;
    private CircleCollider2D myCollider;

    void Awake()
    {
        // Trouve le game object game manager et instancie le field
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        _gameManager.onGameStateChanged += _gameManager_onGameStateChanged;
        myCollider = gameObject.GetComponent<CircleCollider2D>();
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
                    Debug.Log("Init Time : " + initTime);
                    initTime = 0f;

                    myCollider.enabled = false;
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
                }
            }
        }
    }
}

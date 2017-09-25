using UnityEngine;
using System.Collections;
using System;

public class GoldenStarNew : MonoBehaviour {

    public GameObject TriggerEffect;

    private GoldenStarManagerNew starsManager;

    /// <summary>
    /// Instance du game manager
    /// </summary>
    private GameManager _gameManager;

    void Awake()
    {
        starsManager = GameObject.FindObjectOfType<GoldenStarManagerNew>();

        // Trouve le game object game manager et instancie le field
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (_gameManager != null && _gameManager.State == GameState.Playing)
            {
                // Signal que cette étoile à été touchée
                starsManager.SetActiveNextStar(Convert.ToInt32(name));
            }
        }
    }

    public void ValiderEtoile()
    {
        // Active les particules
        var obj = Instantiate(TriggerEffect, transform.position, Quaternion.identity);
        Destroy(obj, 1f);
        Destroy(gameObject);
    }
}
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class GoldenStarManagerNew : MonoBehaviour {

    /// <summary>
    /// Instance du game manager
    /// </summary>
    private GameManager _gameManager;

    public Text StarPointText;

    private List<Transform> goldenStarsCol1 = new List<Transform>();
    private List<Transform> goldenStarsCol2 = new List<Transform>();
    private List<Transform> goldenStarsCol3 = new List<Transform>();
    private List<Transform> goldenStarsCol4 = new List<Transform>();

    private int currentStar = 0;

    void Awake()
    {
        // Trouve le game object game manager et instancie le field
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        // Cherche toutes les golden stars dissponible
        // tri les stars en fonction de leur colonne
        foreach (Transform child in transform)
        {
            // Récupère le nom de l'étoile pour le classer dans la bonne colonne
            int num = Convert.ToInt32(child.name);

            if (num >= 1 && num <= 6)
            {
                goldenStarsCol1.Add(child);
            }
            if (num >= 7 && num <= 12)
            {
                goldenStarsCol2.Add(child);
            }
            if (num >= 13 && num <= 18)
            {
                goldenStarsCol3.Add(child);
            }
            if (num >= 19 && num <= 24)
            {
                goldenStarsCol4.Add(child);
            }
        }
    }

    public void SetActiveNextStar(int num)
    {
        if (num >= 1 && num <= 6)
        {
            // Action sur la colonne 1
            for (int i = (num - 1); i < 6; i++)
            {
                if (goldenStarsCol1[i] != null)
                {
                    var scriptEtoile = goldenStarsCol1[i].GetComponent<GoldenStarNew>();
                    scriptEtoile.ValiderEtoile();
                    currentStar++;
                    if (_gameManager.client != null)
                    {
                        _gameManager.client.CheckpointReached(i + 1);
                    }
                }
            }
        }
        if (num >= 7 && num <= 12)
        {
            num -= 6;
            // Action sur la colonne 2
            for (int i = (num - 1); i < 6; i++)
            {
                if (goldenStarsCol2[i] != null)
                {
                    var scriptEtoile = goldenStarsCol2[i].GetComponent<GoldenStarNew>();
                    scriptEtoile.ValiderEtoile();
                    currentStar++;
                    if (_gameManager.client != null)
                    {
                        _gameManager.client.CheckpointReached(i + 7);
                    }
                }
            }
        }
        if (num >= 13 && num <= 18)
        {
            num -= 12;
            // Action sur la colonne 1
            for (int i = (num - 1); i < 6; i++)
            {
                if (goldenStarsCol3[i] != null)
                {
                    var scriptEtoile = goldenStarsCol3[i].GetComponent<GoldenStarNew>();
                    scriptEtoile.ValiderEtoile();
                    currentStar++;
                    if (_gameManager.client != null)
                    {
                        _gameManager.client.CheckpointReached(i + 13);
                    }
                }
            }
        }
        if (num >= 19 && num <= 24)
        {
            num -= 18;
            // Action sur la colonne 1
            for (int i = (num - 1); i < 6; i++)
            {
                if (goldenStarsCol4[i] != null)
                {
                    var scriptEtoile = goldenStarsCol4[i].GetComponent<GoldenStarNew>();
                    scriptEtoile.ValiderEtoile();
                    currentStar++;
                    if (_gameManager.client != null)
                    {
                        _gameManager.client.CheckpointReached(i + 19);
                    }
                }
            }
        }

        if (currentStar != 0)
        {
            StarPointText.text = currentStar.ToString();
        }
    }
}
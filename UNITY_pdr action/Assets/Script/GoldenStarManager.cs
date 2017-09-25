using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class GoldenStarManager : MonoBehaviour {

    public Text StarPointText;

    private List<Transform> goldenStars = new List<Transform>();
    private int currentStar = 0;

    void Awake()
    {
        // Cherche toutes les golden stars dissponible
        foreach (Transform child in transform)
        {
            goldenStars.Add(child);
        }
    }

	// Use this for initialization
	void Start () {
        SetActiveNextStar();
    }

    public void SetActiveNextStar()
    {
        if (currentStar != 0)
        {
            StarPointText.text = currentStar.ToString();
        }

        if (currentStar < goldenStars.Count)
        {
            var collider = goldenStars[currentStar].GetComponent<BoxCollider2D>();
            collider.enabled = true;

            currentStar++;
        }
    }
}

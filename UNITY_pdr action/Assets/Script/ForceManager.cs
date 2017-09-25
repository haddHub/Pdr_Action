using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ForceManager : MonoBehaviour {


    /// <summary>
    /// Instance du game manager
    /// </summary>
    private GameManager _gameManager;

    public int MaxForce = 150;

    public Slider ForceBar;
    public Text EndText;

    private float forceX = 0f;
    private float forceY = 0f;

    public int ExerciceTime;
    private float exerciceElapsedTime;

    private bool canMovePoint = false;
    private int amplitude;

    public GameObject StartingPoint;
    void Awake()
    {
        // Trouve le game object game manager et instancie le field
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Use this for initialization
    void Start () {
        if (_gameManager != null && _gameManager.client != null)
        {
            _gameManager.client.onNewValue += Client_onNewValue;
            // astuce pour prévenir REAlab qu'il doit envoyer le point de départ
            _gameManager.client.SetTrajectory(new PointData[] { });
            _gameManager.client.onNewForces += Client_onNewForces;
        }

        exerciceElapsedTime = ExerciceTime;
	}

    private void Client_onNewValue(object obj, ValueEvent valueArgs)
    {
        if (valueArgs.Key == "AmplitudeY")
        {
            amplitude = Convert.ToInt32(valueArgs.Value);
            canMovePoint = true;
        }
    }

    private void Client_onNewForces(object obj, PointEvent forcesArgs)
    {
        forceX = forcesArgs.Point.Xf;
        forceY = forcesArgs.Point.Yf;
    }

    // Update is called once per frame
    void Update () {
        if (_gameManager != null)
        {
            if (_gameManager.State == GameState.Playing)
            {
                if (_gameManager.DebugEditorMod == true)
                {
                    ForceBar.value += 0.01f;
                }
                else
                {
                    ForceBar.value = CalculPourcentageFill();
                }

                exerciceElapsedTime -= Time.deltaTime;

                if (exerciceElapsedTime <= 0)
                {
                    _gameManager.SetGameState(GameState.Stop);
                    EndText.gameObject.SetActive(true);
                    ForceBar.value = 0f;
                }
            }

            if (canMovePoint == true)
            {
                canMovePoint = false;
                // Place le point de départ en fonction de l'amplitude envoyée par REALab
                var vectorAmplitude = new Vector3(0f, amplitude, 0f);
                var amplitudeWorld = GameUtils.PixelToWorldPoint(vectorAmplitude, Camera.main);

                amplitudeWorld.x = StartingPoint.transform.position.x;
                amplitudeWorld.z = StartingPoint.transform.position.z;
                StartingPoint.transform.position = amplitudeWorld;
                StartingPoint.SetActive(true);
            }
        }
	}

    /// <summary>
    /// Calcul du pourcentage de remplissage de l'image en fonction de la force actuelle et du min max voulu
    /// </summary>
    private float CalculPourcentageFill()
    {
        float fillAmount = 0f;

        // Il faut commencer par faire la norme de la force afin de prendre en compte la force X et Y
        var norme = NormeForce(forceX, forceY);

        fillAmount = norme / MaxForce;

        return fillAmount;
    }

    private float NormeForce(float x, float y)
    {
        return Mathf.Sqrt(Mathf.Pow(x, 2f) + Mathf.Pow(y, 2f));
    }
}

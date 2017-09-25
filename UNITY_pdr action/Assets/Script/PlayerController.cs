using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    /// <summary>
    /// Instance du game manager
    /// </summary>
    private GameManager _gameManager;

    public float RotationSpeed = 5f;
    public bool FollowRobot = false;
    public bool Moveable = true;

    private float posX;
    private float posY;

    private Vector3 previousPos = Vector3.zero;
    private float speed;

    void Awake()
    {
        // Trouve le game object game manager et instancie le field
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Use this for initialization
    void Start () {

        if (_gameManager != null && _gameManager.client != null)
        {
            _gameManager.client.onNewPositions += Client_onNewPositions;
        }
    }

    private void Client_onNewPositions(object obj, PointEvent positionsArgs)
    {
        posX = positionsArgs.Point.Xf;
        posY = positionsArgs.Point.Yf;
    }

    // Update is called once per frame
    void Update () {

        if (Moveable == true)
        {
            if (_gameManager != null && _gameManager.State != GameState.Pause)
            {
                Vector3 positionPixel = new Vector3();
                Vector3 positionWorld = new Vector3();

                // Récupération des coordonnées de la souris ou du robot
                if (FollowRobot == false)
                {
                    positionPixel = Input.mousePosition;
                    positionWorld = Camera.main.ScreenToWorldPoint(positionPixel);
                }
                else
                {
                    positionPixel = new Vector3(posX, posY, 0f);
                    positionWorld = GameUtils.PixelToWorldPoint(positionPixel, Camera.main);
                }

                positionWorld.z = 0f;

                // Calcul de la vitesse par rapport à la frame d'avant et adapte la RotationSpeed pour que la rotation soit toujours fluide
                if (previousPos == Vector3.zero)
                {
                    previousPos = transform.position;
                }
                else
                {
                    speed = Vector3.Distance(previousPos, transform.position) / Time.deltaTime;

                    if (speed < 10f)
                    {
                        RotationSpeed = 0.1f;
                    }
                    else
                    {
                        RotationSpeed = 2f;
                    }

                    previousPos = Vector3.zero;
                }
                // Pour que le player effectue une rotation fluide vers l'endroit ou il se dirige
                // Il faut d'abord calculer la position relative du joueur
                Vector3 relativePosition;

                if (_gameManager.State == GameState.Countdown || _gameManager.State == GameState.PauseTarget)
                {

                    var fakePosition = new Vector3(0f, 10f, 0f);
                    relativePosition = fakePosition - this.transform.position;
                }
                else
                {
                    relativePosition = positionWorld - this.transform.position;
                }

                // Si le player ne bouge pas il n'y a pas besoin de changer sa rotation
                if (relativePosition != Vector3.zero)
                {
                    // Calcule de l'angle de la rotation en fonction de la direction à atteindre
                    float angle = Mathf.Atan2(relativePosition.y, relativePosition.x) * Mathf.Rad2Deg;

                    // Soustraction de 90° car le "front" de l'objet 2d est en haut et non a droite
                    angle -= 90;
                    // Etat de la rotation acutelle
                    var from = this.transform.rotation;
                    // Calcule de la rotation à atteindre
                    var to = Quaternion.AngleAxis(angle, Vector3.forward);
                    // Interpolation entre la rotation actuelle et la rotation à atteindre pour que même a basse vitesse la rotation soit fluide
                    transform.rotation = Quaternion.Lerp(from, to, this.RotationSpeed);
                }

                // On attribue la nouvelle position au player
                this.transform.position = positionWorld;
            }
        }
	}
}

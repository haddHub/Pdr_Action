using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
public class CouloirCreator : MonoBehaviour {

    private BoxCollider2D myCollider;
    private List<Trigger2DListener> listeners = new List<Trigger2DListener>();

    private void Awake()
    {
        myCollider = GetComponent<BoxCollider2D>();
        Enabled(false);
    }

    public void SetUp(Vector3 pointA, Vector3 pointB)
    {
        transform.position = PositionBetweenPoints(pointA, pointB);

        myCollider.size = HeightToReachBothPoints(pointA, pointB);

        // Direction
        var direction = pointB - pointA;
        transform.rotation = Quaternion.LookRotation(direction);

        float zRotation = 0f;
        if (transform.eulerAngles.y >= 180)
        {
            zRotation = transform.eulerAngles.x - transform.eulerAngles.y;
            zRotation = Mathf.Abs(zRotation);
        }
        else
        {
            zRotation = transform.eulerAngles.y - transform.eulerAngles.x;
        }

        Vector3 newRotation = new Vector3(0f, 0f, zRotation);
        transform.eulerAngles = newRotation;
    }

    public void Enabled(bool status)
    {
        myCollider.enabled = status;
    } 

    private Vector3 PositionBetweenPoints(Vector3 a, Vector3 b)
    {
        float x = (a.x + b.x) / 2f;
        float y = (a.y + b.y) / 2f;

        Vector3 pos = new Vector3(x, y, 0f);

        return pos;
    }

    private Vector2 HeightToReachBothPoints(Vector3 a, Vector3 b)
    {
        Vector2 height = new Vector2(1f, Vector3.Distance(a, b));
        return height;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var listener in listeners)
        {
            listener.Trigger2DEnter(collision.tag);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        foreach (var listener in listeners)
        {
            listener.Trigger2DStay(collision.tag);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        foreach (var listener in listeners)
        {
            listener.Trigger2DExit(collision.tag);
        }
    }

    public void AddListener(Trigger2DListener listener)
    {
        listeners.Add(listener);
    }

    public void Removeistener(Trigger2DListener listener)
    {
        listeners.Remove(listener);
    }
}

public interface Trigger2DListener
{
    void Trigger2DEnter(string tag);

    void Trigger2DStay(string tag);

    void Trigger2DExit(string tag);
}

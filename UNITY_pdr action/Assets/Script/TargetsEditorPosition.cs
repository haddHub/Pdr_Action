using UnityEngine;
using System.Collections;

public class TargetsEditorPosition : MonoBehaviour {

    public Vector3[] PixelPostions;
    public float SphereRadius = 2f;

    void OnDrawGizmos()
    {
        if (PixelPostions != null && PixelPostions.Length > 0)
        {
            for (int i = 0; i < PixelPostions.Length; i++)
            {
                var tempsPos = PixelPostions[i];
                tempsPos.y = 1080 - tempsPos.y;
                Vector3 worldPosition = GameUtils.PixelToWorldPoint(tempsPos, Camera.main);

                Gizmos.DrawSphere(worldPosition, SphereRadius);
            }
        }
    }
}

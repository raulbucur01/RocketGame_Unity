using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // Obiectul pe care îl urmărim, în cazul acesta va fi camera principală
    public Vector3 offset;   // Distanța față de obiectul țintă
    public float smoothSpeed = 0.125f;  // Liniaritatea mișcării camerei

    void Start()
    {
        void Start()
        {
            if (target == null)
            {
                target = Camera.main.transform;  // Setăm camera principală ca target
                if (target == null)
                {
                    Debug.LogError("No valid target found for CameraFollow!");
                }
            }
        }

    }

    void LateUpdate()
    {
        if (target == null)
        {
            return;  // Dacă nu există target, nu se face nimic
        }

        // Calculăm poziția dorită pe baza poziției camerei și offset-ului
        Vector3 desiredPosition = target.position + offset;

        // Mișcăm camera către poziția dorită cu o mișcare lină
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Actualizăm poziția camerei
        transform.position = smoothedPosition;
    }
}

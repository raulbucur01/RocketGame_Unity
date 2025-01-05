using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // The object to follow
    public Vector3 offset;   // The distance between the camera and the target
    public float smoothSpeed = 0.125f;  // Smoothness of the camera movement

    void LateUpdate()
    {
        // Calculate the desired position based on the target's position and the offset
        Vector3 desiredPosition = target.position + offset;
        
        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        // Update the camera's position
        transform.position = smoothedPosition;
    }
}

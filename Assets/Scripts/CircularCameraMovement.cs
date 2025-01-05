using UnityEngine;

public class CircularCameraMovement : MonoBehaviour
{
    public Transform target; // The object to orbit around
    public float radius = 5f; // Distance from the target
    public float heightOffset = 2f; // Vertical offset above the target
    public float speed = 20f; // Rotation speed (degrees per second)

    private float angle = 0f; // Current angle around the target

    void Update()
    {
        if (target == null) return;

        // Calculate the new angle
        angle += speed * Time.deltaTime;
        angle %= 360f; // Keep angle in range 0-360 to avoid overflow

        // Convert angle to radians for Mathf functions
        float radians = angle * Mathf.Deg2Rad;

        // Calculate new position
        float x = target.position.x + radius * Mathf.Cos(radians);
        float z = target.position.z + radius * Mathf.Sin(radians);
        float y = target.position.y + heightOffset; // Maintain a vertical offset

        // Update the camera position
        transform.position = new Vector3(x, y, z);

        // Make the camera look at the target
        Vector3 lookAtPosition = new Vector3(target.position.x, target.position.y + heightOffset / 2, target.position.z);
        transform.LookAt(lookAtPosition);
    }
}

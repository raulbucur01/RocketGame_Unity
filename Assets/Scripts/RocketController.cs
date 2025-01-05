using UnityEngine;

public class RocketController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 50f;       // Normal forward speed
    public float boostedSpeed = 70f;      // Speed when Space is pressed
    public float baseRotationSpeed = 100f; // Base rotation speed

    private Rigidbody rb;
    private float currentSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = forwardSpeed; // Start with normal forward speed
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        // Adjust speed based on Space key
        currentSpeed = Input.GetKey(KeyCode.Space) ? boostedSpeed : forwardSpeed;

        // Move forward along the Z-axis at the current speed
        rb.linearVelocity = -(transform.forward * currentSpeed);
    }

    private void HandleRotation()
    {
        float XAxisRotation = 0f;  // Up/Down rotation
        float ZAxisRotation = 0f; // Left/Right rotation
        float YAxisRotation = 0f;

        // Adjust rotation based on WASD keys
        if (Input.GetKey(KeyCode.W)) XAxisRotation = 1f;   // Rotate up
        if (Input.GetKey(KeyCode.S)) XAxisRotation = -1f;  // Rotate down
        if (Input.GetKey(KeyCode.Q)) ZAxisRotation = -1f; // Roll left
        if (Input.GetKey(KeyCode.E)) ZAxisRotation = 1f; // Roll right
        if (Input.GetKey(KeyCode.A))
        {
            YAxisRotation = -1f;
            ZAxisRotation = -0.2f; // Slight roll to the left
        }
        if (Input.GetKey(KeyCode.D))
        {
            YAxisRotation = 1f;
            ZAxisRotation = 0.2f; // Slight roll to the right
        }

        // Scale rotation speed based on movement speed
        float scaledRotationSpeed = baseRotationSpeed * (currentSpeed / boostedSpeed);

        // Apply rotation to the transform
        Vector3 rotation = new Vector3(XAxisRotation, YAxisRotation, ZAxisRotation) * scaledRotationSpeed * Time.deltaTime;
        transform.Rotate(rotation, Space.Self);
    }
}

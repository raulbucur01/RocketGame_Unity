using System.Net.Sockets;
using UnityEngine;

public class ArcadeShipControls: MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 50f;       // Normal forward speed
    public float boostedSpeed = 70f;      // Speed when Space is pressed
    public float baseRotationSpeed = 100f; // Base rotation speed

    [Header("Shooting Settings")]
    public GameObject rocketPrefab;      // Rocket prefab
    public Transform rocketLauncher1;          // Fire point for shooting rockets
    public Transform rocketLauncher2;          // Fire point for shooting rockets
    public float rocketSpeed = 10f;      // Speed of the rocket
    public float fireRate = 0.5f;

    [Header("Particle Effects")]
    public ParticleSystem effect1;   // Particle effect 1 (e.g., engine 1)
    public ParticleSystem effect2;   // Particle effect 2 (e.g., engine 2)
    public ParticleSystem effect3;   // Particle effect 3 (e.g., engine 3)
    public ParticleSystem effect4;

    private Rigidbody rb;
    private float currentSpeed;
    private float lastFireTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = forwardSpeed; // Start with normal forward speed
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleShooting();
    }

    private void HandleMovement()
    {
        // Adjust speed based on Space key
        currentSpeed = Input.GetKey(KeyCode.Space) ? boostedSpeed : forwardSpeed;

        // Adjust particle emission rate based on boost
        float newRate = Input.GetKey(KeyCode.Space) ? 300f : 20f; // Adjust these values as needed
        SetParticleEmissionRate(newRate);

        // Move forward along the Z-axis at the current speed
        rb.linearVelocity = -(transform.forward * currentSpeed);
    }

    private void SetParticleEmissionRate(float newRate)
    {
        var effect1Emission = effect1.emission;
        var effect2Emission = effect2.emission;
        var effect3Emission = effect3.emission;
        var effect4Emission = effect4.emission;

        effect1Emission.rateOverTime = newRate;
        effect2Emission.rateOverTime = newRate;
        effect3Emission.rateOverTime = newRate;
        effect4Emission.rateOverTime = newRate;
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

    private void HandleShooting()
    {
        // Check if the fire button is pressed and enough time has passed since the last shot
        if (Input.GetKey(KeyCode.R) && Time.time > lastFireTime + fireRate)
        {
            ShootRockets();
            lastFireTime = Time.time;
        }
    }

    private void ShootRockets()
    {
        if (rocketPrefab == null || rocketLauncher1 == null || rocketLauncher2 == null)
        {
            Debug.LogWarning("Rocket prefab or rocket launcher is missing!");
            return;
        }

        // Instantiate the rocket at the rocket launcher's position and rotation
        GameObject rocket1 = Instantiate(rocketPrefab, rocketLauncher1.position, rocketLauncher1.rotation);
        GameObject rocket2 = Instantiate(rocketPrefab, rocketLauncher2.position, rocketLauncher2.rotation);

        // Apply force to propel the rocket in the ship's moving direction
        Rigidbody rocketRb1 = rocket1.GetComponent<Rigidbody>();
        if (rocketRb1 != null)
        {
            // Combine ship's velocity and rocket's forward speed
            Vector3 shipVelocity = rb.linearVelocity; // Ship's current velocity
            Vector3 rocketDirection = rocketLauncher1.up * rocketSpeed; // Rocket's forward motion
            rocketRb1.linearVelocity = shipVelocity + rocketDirection;
        }

        Rigidbody rocketRb2 = rocket2.GetComponent<Rigidbody>();
        if (rocketRb2 != null)
        {
            // Combine ship's velocity and rocket's forward speed
            Vector3 shipVelocity = rb.linearVelocity; // Ship's current velocity
            Vector3 rocketDirection = rocketLauncher2.up * rocketSpeed; // Rocket's forward motion
            rocketRb2.linearVelocity = shipVelocity + rocketDirection;
        }
    }

}

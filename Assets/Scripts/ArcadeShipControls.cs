using System.Net.Sockets;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Net.Sockets;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

public class ArcadeShipControls: MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;   // Maximum health of the ship
    private int currentHealth;    // Current health of the ship

    [Header("Explosion Settings")]
    public GameObject explosionPrefab;  // Assign an explosion prefab here
    public float explosionDuration = 2f; // Time before transitioning to Game Over

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

    [Header("Other")]
    [SerializeField] private TextMeshProUGUI _uiText;
    [SerializeField] private Image _screenBlackout;
    [SerializeField] private GameObject _gameOverButton;
    [SerializeField] private GameObject _quitButton;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _explosionSound;
    [SerializeField] private AudioClip _typeSound;
    [SerializeField] private AudioClip _damageSound;
    [SerializeField] private List<GameObject> _shipParts;
    [SerializeField] private GameObject _booster;
    [SerializeField] private GameObject _bgMusic;

    [Header("Cameras")]
    public Camera mainCamera;       // Main gameplay camera
    public Camera gameOverCamera;   // Secondary camera for Game Over scene

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 5f; // Adjust drag to resist movement
        rb.angularDamping = 5f; // To reduce rotation as well
        currentSpeed = forwardSpeed; // Start with normal forward speed
        currentHealth = maxHealth; // Initialize health
        _gameOverButton.SetActive(false);
        _quitButton.SetActive(false);

        // Ensure the main camera is active when the game starts
        if (mainCamera != null)
        {
            mainCamera.enabled = true;
        }

        // Ensure the game over camera is disabled at the start
        if (gameOverCamera != null)
        {
            gameOverCamera.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if collided object has the "EnemyRocket" tag
        if (collision.gameObject.CompareTag("EnemyRocket"))
        {
            TakeDamage(20); // Apply 20 damage points
            _audioSource.PlayOneShot(_damageSound);
            Destroy(collision.gameObject); // Destroy the enemy rocket
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Ship took {damage} damage! Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Ship destroyed!");

        // Play explosion sound
        _audioSource.PlayOneShot(_explosionSound);

        // Instantiate explosion effect
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
        }

        // Destroy the ship
        foreach (var shipPart in _shipParts)
        {
            Destroy(shipPart);
        }

        // Transition to the Game Over screen after showing the explosion
        GameOverScreen("Your ship is done for!");
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
        
        if(effect1 == null || effect2 == null || effect3 == null || effect4 == null)
        {
            Debug.LogWarning("Particle emission is missing!");
            return;
        }

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

        // Get the colliders of the player ship and the rockets
        Collider playerCollider = GetComponent<Collider>();
        Collider rocketCollider1 = rocket1.GetComponent<Collider>();
        Collider rocketCollider2 = rocket2.GetComponent<Collider>();

        if (playerCollider != null && rocketCollider1 != null)
        {
            // Ignore collisions between the player ship and the first rocket
            Physics.IgnoreCollision(playerCollider, rocketCollider1);
        }

        if (playerCollider != null && rocketCollider2 != null)
        {
            // Ignore collisions between the player ship and the second rocket
            Physics.IgnoreCollision(playerCollider, rocketCollider2);
        }

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

    private async Task GameOverScreen(string text)
    {
        // Deactivate the ship's collider to prevent further collisions
        var shipCollider = GetComponent<Collider>();
        shipCollider.enabled = false;
        Destroy(_booster);
        
        // Fade to black
        while (_screenBlackout.color.a < 1)
        {
            _screenBlackout.color = new Color(0, 0, 0, _screenBlackout.color.a + Time.deltaTime);
            await Task.Delay(20);
        }

        // Wait for 2 seconds
        await Task.Delay(2000);

        // make text add letters one by one
        for (int i = 0; i < text.Length; i++)
        {
            _uiText.text += text[i];
            _audioSource.PlayOneShot(_typeSound);
            await Task.Delay(100);
        }

        await Task.Delay(1000);
        _gameOverButton.SetActive(true);
        _quitButton.SetActive(true);
    }
}

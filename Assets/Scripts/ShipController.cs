using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Net.Sockets;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RocketController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float forwardSpeed = 50f;       // Normal forward speed
    [SerializeField] private float boostedSpeed = 70f;      // Speed when Space is pressed
    [SerializeField] private float baseRotationSpeed = 100f; // Base rotation speed

    [Header("Shooting Settings")]
    [SerializeField] private GameObject rocketPrefab;      // Rocket prefab
    [SerializeField] private Transform rocketLauncher1;          // Fire point for shooting rockets
    [SerializeField] private Transform rocketLauncher2;          // Fire point for shooting rockets
    [SerializeField] private float rocketSpeed = 10f;      // Speed of the rocket
    [SerializeField] private float fireRate = 0.5f;

    [Header("Particle Effects")]
    [SerializeField] private ParticleSystem effect1;   // Particle effect 1 (e.g., engine 1)
    [SerializeField] private ParticleSystem effect2;   // Particle effect 2 (e.g., engine 2)
    [SerializeField] private ParticleSystem effect3;   // Particle effect 3 (e.g., engine 3)
    [SerializeField] private ParticleSystem effect4;
    
    [Header("Camera Settings")]
    [SerializeField] private Camera _camera;
    [SerializeField] private float _cameraFollowSpeed;
    
    [Header("Other")]
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private TextMeshProUGUI _uiText;
    [SerializeField] private Image _screenBlackout;
    [SerializeField] private GameObject _gameOverButton;
    [SerializeField] private GameObject _quitButton;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _explosionSound;
    [SerializeField] private AudioClip _typeSound;
    [SerializeField] private List<GameObject> _shipParts;
    [SerializeField] private GameObject _booster;
    [SerializeField] private GameObject _bgMusic;
 
    private Rigidbody rb;
    private float currentSpeed;
    private float lastFireTime;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = forwardSpeed; // Start with normal forward speed
        _quitButton.SetActive(false);
        _gameOverButton.SetActive(false);
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleShooting();
        UpdateCamera();
    }

    private Vector3 _currentVelocity; // Variabilă pentru viteza de tranziție a camerei

    private void UpdateCamera()
    {
        // Define offset relative to the ship's local space (back and up)
        Vector3 offset = transform.up * 40f + transform.forward * 20f;

        // Calculate the target position in world space
        Vector3 targetPosition = transform.position + offset;

        // Smoothly move the camera towards the target position using SmoothDamp
        _camera.transform.position = Vector3.SmoothDamp(
            _camera.transform.position, 
            targetPosition, 
            ref _currentVelocity, 
            _cameraFollowSpeed // Ajustează timpul dorit pentru tranziție
        );

        // Ensure the camera always looks at the ship with the correct up direction
        Vector3 lookAtTarget = transform.position + transform.forward * 2f;
        Vector3 upDirection = transform.up;

        _camera.transform.rotation = Quaternion.LookRotation(lookAtTarget - _camera.transform.position, upDirection);
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
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) XAxisRotation = 1f;   // Rotate up
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) XAxisRotation = -1f;  // Rotate down
        if (Input.GetKey(KeyCode.Q)) ZAxisRotation = -1f; // Roll left
        if (Input.GetKey(KeyCode.E)) ZAxisRotation = 1f; // Roll right
        if (Input.GetKey(KeyCode.A)  || Input.GetKey(KeyCode.LeftArrow))
        {
            YAxisRotation = -1f;
            ZAxisRotation = -0.2f; // Slight roll to the left
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
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

    private void OnCollisionEnter(Collision other)
    {
        // Instantiate explosion effect
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

        // Destroy the rendered ship
        foreach (var part in _shipParts)
            Destroy(part);
        
        GetComponent<Collider>().enabled = false;
        
        // Play explosion sound
        _audioSource.PlayOneShot(_explosionSound);
        
        GameOver("You crashed");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            GameOver("You lost your objective");
        }
    }

    private async Task GameOver(string text)
    {
        Destroy(_booster);
        
        // Fade to black
        while(_screenBlackout.color.a < 1)
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

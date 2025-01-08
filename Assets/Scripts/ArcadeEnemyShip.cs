using UnityEngine;

public class ArcadeEnemyShip : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject rocketPrefab;        // Rocket prefab to shoot
    public Transform[] rocketSpawnPoints; // Array of rocket spawn points
    public float fireRate = 2f;           // Time between each shot
    public float rocketSpeed = 500f;      // Speed of the rocket
    public Transform target;              // The player's ship to target

    private float nextFireTime = 0f;

    void Update()
    {
        // Check if it's time to shoot
        if (Time.time >= nextFireTime)
        {
            ShootRockets();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void ShootRockets()
    {
        if (rocketPrefab != null && target != null && rocketSpawnPoints.Length > 0)
        {
            foreach (Transform spawnPoint in rocketSpawnPoints)
            {
                // Instantiate the rocket
                GameObject rocket = Instantiate(rocketPrefab, spawnPoint.position, Quaternion.identity);

                // Calculate the direction to the target at the moment of firing
                Vector3 directionToTarget = (target.position - spawnPoint.position).normalized;

                // Apply the velocity to the rocket
                Rigidbody rocketRb = rocket.GetComponent<Rigidbody>();
                if (rocketRb != null)
                {
                    rocketRb.linearVelocity = directionToTarget * rocketSpeed; // Set initial velocity
                }
            }
        }
    }
}

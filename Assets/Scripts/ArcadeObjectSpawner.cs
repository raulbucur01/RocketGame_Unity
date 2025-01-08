using UnityEngine;

public class ArcadeObjectSpawner : MonoBehaviour
{
    public GameObject[] spawnablePrefabs; // Array of prefabs to spawn // The prefab to spawn
    public float spawnRadius = 250f;   // Radius around the ship where objects spawn
    public float spawnInterval = 2f;   // Time between spawns
    public int maxSpawnedObjects = 10; // Maximum number of objects at a time

    private int currentObjectCount = 0;

    void Start()
    {
        // Start spawning objects at regular intervals
        InvokeRepeating(nameof(SpawnObject), spawnInterval, spawnInterval);
    }

    void SpawnObject()
    {
        if (currentObjectCount >= maxSpawnedObjects || spawnablePrefabs.Length == 0)
            return;

        // Randomly select a prefab from the array
        GameObject selectedPrefab = spawnablePrefabs[Random.Range(0, spawnablePrefabs.Length)];

        // Random position around the ship within the spawn radius
        Vector3 spawnPosition = transform.position + Random.onUnitSphere * spawnRadius;

        // Ensure objects spawn on the same plane as the ship (optional)
        spawnPosition.y = transform.position.y;

        // Generate a random rotation
        Quaternion randomRotation = Random.rotation;

        // Spawn the object with random rotation
        GameObject newObject = Instantiate(selectedPrefab, spawnPosition, randomRotation);

        // Add random movement
        Rigidbody rb = newObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Random direction and speed
            Vector3 randomDirection = Random.onUnitSphere; // Random direction in 3D space
            float randomSpeed = Random.Range(5f, 120f);    // Adjust speed range as needed
            rb.linearVelocity = randomDirection * randomSpeed;
        }

        // Increase the count of spawned objects
        currentObjectCount++;

        // Set the spawner reference on the object
        ArcadeDestructibleObject destructible = newObject.GetComponent<ArcadeDestructibleObject>();
        if (destructible != null)
        {
            destructible.SetSpawner(this);
        }
    }



    public void ObjectDestroyed()
    {
        // Decrease the count of spawned objects when one is destroyed
        currentObjectCount--;

        // Ensure the count doesn't go below zero
        currentObjectCount = Mathf.Max(currentObjectCount, 0);
    }
}

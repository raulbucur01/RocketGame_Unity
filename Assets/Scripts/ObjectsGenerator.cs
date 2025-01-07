using UnityEngine;
using System.Collections.Generic;

public class GenerateAsteroids : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject[] asteroidPrefabs; // Lista cu prefabs de asteroizi
    public int maxAsteroids = 100; // Numărul maxim de asteroizi simultan

    [Header("Spawn Area Settings")]
    public Transform ship; // Referință la nava centrală
    public float spawnRadius = 50f; // Raza în jurul navei pentru spawn

    [Header("Scale Settings")]
    public Vector2 asteroidScaleRange = new Vector2(1f, 10f); // Interval pentru scalarea asteroizilor (minim, maxim)

    private List<GameObject> activeAsteroids = new List<GameObject>(); // Lista de asteroizi activi

    void Update()
    {
        // Detectăm apăsarea tastei 'K' pentru a genera asteroizi
        if (Input.GetKeyDown(KeyCode.K))
        {
            GenerateInitialAsteroids();
        }

        // Actualizăm pozițiile asteroizilor
        UpdateAsteroids();
    }

    void GenerateInitialAsteroids()
    {
        // Distrugem asteroizii existenți
        foreach (GameObject asteroid in activeAsteroids)
        {
            Destroy(asteroid);
        }
        activeAsteroids.Clear();

        // Generăm noi asteroizi
        for (int i = 0; i < maxAsteroids; i++)
        {
            SpawnAsteroidAtRandomPosition();
        }

        Debug.Log($"{maxAsteroids} asteroids have been generated around the ship!");
    }

    void UpdateAsteroids()
    {
        for (int i = activeAsteroids.Count - 1; i >= 0; i--)
        {
            GameObject asteroid = activeAsteroids[i];

            // Verificăm dacă asteroidul a ieșit din raza specificată
            if (Vector3.Distance(asteroid.transform.position, ship.position) > spawnRadius)
            {
                // Calculăm poziția de respawn pe partea opusă
                Vector3 oppositePosition = CalculateOppositeSpawnPosition(asteroid.transform.position);

                // Distrugem asteroidul și respawnăm unul nou
                Destroy(asteroid);
                activeAsteroids.RemoveAt(i);

                SpawnAsteroidAtPosition(oppositePosition);
            }
        }
    }

    void SpawnAsteroidAtRandomPosition()
    {
        if (asteroidPrefabs.Length == 0)
        {
            Debug.LogError("No asteroid prefabs assigned!");
            return;
        }

        // Selectează un prefab aleatoriu din listă
        GameObject randomPrefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];

        // Calculăm o poziție aleatorie pe marginea razei
        Vector3 randomDirection = Random.onUnitSphere; // Direcție aleatorie
        Vector3 spawnPosition = ship.position + randomDirection * spawnRadius;

        // Instantiem prefab-ul selectat aleatoriu
        SpawnAsteroidAtPosition(spawnPosition);
    }

    void SpawnAsteroidAtPosition(Vector3 position)
    {
        GameObject randomPrefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];

        // Instantiem prefab-ul selectat aleatoriu
        GameObject asteroid = Instantiate(randomPrefab, position, Random.rotation);

        // Aplicăm o scalare aleatorie
        float randomScale = Random.Range(asteroidScaleRange.x, asteroidScaleRange.y);
        asteroid.transform.localScale = Vector3.one * randomScale;

        // Adăugăm asteroidul în lista de asteroizi activi
        activeAsteroids.Add(asteroid);
    }

    Vector3 CalculateOppositeSpawnPosition(Vector3 position)
    {
        // Direcția asteroidului față de navă
        Vector3 direction = (position - ship.position).normalized;

        // Poziția pe partea opusă, la marginea razei
        return ship.position - direction * spawnRadius;
    }

    // Vizualizarea zonei de spawn în editor
    private void OnDrawGizmos()
    {
        if (ship != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(ship.position, spawnRadius);
        }
    }
}

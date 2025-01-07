using UnityEngine;

public class GenerateAsteroids : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject[] asteroidPrefabs; // Lista cu prefabs de asteroizi
    public int numberOfAsteroids = 1000; // Numărul total de asteroizi

    [Header("Spawn Area Settings")]
    public Vector3 spawnAreaSize = new Vector3(50, 50, 50); // Dimensiunea zonei de spawn
    public Vector3 spawnAreaCenter = Vector3.zero; // Centrul zonei de spawn

    [Header("Scale Settings")]
    public Vector2 asteroidScaleRange = new Vector2(1f, 10f); // Interval pentru scalarea asteroizilor (minim, maxim)

    void Start()
    {
        GenerateAsteroidsInScene();
    }

    void GenerateAsteroidsInScene()
    {
        if (asteroidPrefabs.Length == 0)
        {
            Debug.LogError("No asteroid prefabs assigned!");
            return;
        }

        for (int i = 0; i < numberOfAsteroids; i++)
        {
            // Selectează un prefab aleatoriu din listă
            GameObject randomPrefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];

            // Calculăm o poziție aleatorie în zona de spawn
            Vector3 randomPosition = new Vector3(
                Random.Range(spawnAreaCenter.x - spawnAreaSize.x / 2, spawnAreaCenter.x + spawnAreaSize.x / 2),
                Random.Range(spawnAreaCenter.y - spawnAreaSize.y / 2, spawnAreaCenter.y + spawnAreaSize.y / 2),
                Random.Range(spawnAreaCenter.z - spawnAreaSize.z / 2, spawnAreaCenter.z + spawnAreaSize.z / 2)
            );

            // Instantiem prefab-ul selectat aleatoriu
            GameObject asteroid = Instantiate(randomPrefab, randomPosition, Random.rotation);

            // Aplicăm o scalare aleatorie
            float randomScale = Random.Range(asteroidScaleRange.x, asteroidScaleRange.y);
            asteroid.transform.localScale = Vector3.one * randomScale;
        }

        Debug.Log($"{numberOfAsteroids} asteroids have been generated in the scene!");
    }

    // Vizualizarea zonei de spawn în editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(spawnAreaCenter, spawnAreaSize);
    }
}

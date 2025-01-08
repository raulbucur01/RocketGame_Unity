using UnityEngine;

public class ArcadeExplosionCleanup : MonoBehaviour
{
    public float lifetime = 2f; // Time to wait before destroying the explosion

    void Start()
    {
        Destroy(gameObject, lifetime); // Automatically destroy the explosion after `lifetime` seconds
    }
}

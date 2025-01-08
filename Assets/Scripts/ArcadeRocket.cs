using UnityEngine;

public class ArcadeRocket : MonoBehaviour
{
    public float lifetime = 5f; // Time before the rocket is destroyed
    public GameObject explosionPrefab; // Explosion effect prefab

    void Start()
    {
        // Destroy the rocket after a set lifetime
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Shootable"))
        {
            // Instantiate the explosion effect
            TriggerExplosion();

            // Handle hit logic
            Destroy(gameObject); // Destroy rocket
        }
    }

    private void TriggerExplosion()
    {
        if (explosionPrefab != null)
        {
            // Create the explosion effect at the rocket's position and rotation
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }


}

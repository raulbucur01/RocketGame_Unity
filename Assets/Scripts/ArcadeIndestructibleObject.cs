using UnityEngine;

public class ArcadeIndestructibleObject : MonoBehaviour
{
    private Rigidbody rb;

    public GameObject explosionPrefab;

    void Start()
    {
    }
    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with a rocket
        if (collision.gameObject.CompareTag("Rocket"))
        {
            // Instantiate explosion effect
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}

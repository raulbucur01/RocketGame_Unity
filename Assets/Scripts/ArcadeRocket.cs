using UnityEngine;

public class ArcadeRocket : MonoBehaviour
{
    public float lifetime = 5f; // Time before the rocket is destroyed

    void Start()
    {
        // Destroy the rocket after a set lifetime
        Destroy(gameObject, lifetime);
    }

   private void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Shootable"))
    {
        // Handle hit logic
        Destroy(gameObject); // Destroy rocket
    }
}

}

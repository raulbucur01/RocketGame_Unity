using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float lifetime = 5f; // Time before the rocket is destroyed

    void Start()
    {
        // Destroy the rocket after a set lifetime
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Example: Destroy the rocket on collision
        // You can add effects like explosions here
        Destroy(gameObject);
    }
}

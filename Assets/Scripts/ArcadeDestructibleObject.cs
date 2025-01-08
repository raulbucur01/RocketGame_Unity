using UnityEngine;

public class ArcadeDestructibleObject : MonoBehaviour
{
    public float maxHealth = 100f; // Maximum health
    private float currentHealth;

    private ArcadeObjectSpawner spawner; // Reference to the spawner
    private Rigidbody rb;

    void Start()
    {
        currentHealth = maxHealth; // Initialize health
    }

    public void SetSpawner(ArcadeObjectSpawner spawner)
    {
        this.spawner = spawner;
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with a rocket
        // Check if the collision is with a rocket
        if (collision.gameObject.CompareTag("Rocket"))
        {
            Debug.Log("Rocket hit: " + collision.gameObject.name);

            // Apply damage
            float damage = 20f; // Adjust damage as needed
            TakeDamage(damage);

            // Apply a small force to the object in the direction of the collision
            if (rb != null)
            {
                // Get the direction of the collision contact
                Vector3 forceDirection = collision.contacts[0].normal;

                rb.linearDamping = 0.5f; // Adjust drag to resist movement
                rb.angularDamping = 0.5f; // To reduce rotation as well

                // Apply a small force (adjust the strength as needed)
                float forceStrength = 0.000001f; // A small value for the force
                rb.AddForce(forceDirection * forceStrength, ForceMode.Impulse); // Apply the force
            }

            //// Destroy the rocket after impact
            //Destroy(collision.gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("TakeDamage, currentHealth: " + currentHealth);
        // Update health bar UI (see **2. Health Bar UI**)
        //UpdateHealthBar();

        if (currentHealth <= 0)
        {
            DestroyObject();
        }
    }

    private void DestroyObject()
    {
        // Notify the spawner that this object is destroyed
        spawner?.ObjectDestroyed();

        // (Optional) Play an explosion effect here

        Destroy(gameObject);
    }

    //private void UpdateHealthBar()
    //{
    //    // Logic to update health bar (filled in step **2**)
    //}
}

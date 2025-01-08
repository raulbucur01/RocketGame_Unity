using UnityEngine;

public class ForwardBackwardMovement : MonoBehaviour
{
    public float speed = 5f; // Viteza de mișcare
    public float distance = 10f; // Distanța maximă față de poziția inițială

    private Vector3 startPosition; // Poziția inițială a asteroidului
    private Vector3 targetPosition; // Poziția țintă curentă
    private bool movingToTarget = true; // Indică dacă asteroidul se mișcă spre țintă sau se întoarce

    void Start()
    {
        // Stochează poziția de start și setează prima țintă
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.forward * distance;
    }

    void Update()
    {
        // Mișcă asteroidul spre ținta curentă
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Verifică dacă asteroidul a ajuns la ținta curentă
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Comută ținta între poziția inițială și poziția finală
            if (movingToTarget)
                targetPosition = startPosition; // Întoarcere la poziția inițială
            else
                targetPosition = startPosition + Vector3.forward * distance; // Mișcare spre față

            movingToTarget = !movingToTarget; // Comută direcția
        }
    }
}

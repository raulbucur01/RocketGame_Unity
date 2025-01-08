using UnityEngine;

public class ZigZagForwardBackward : MonoBehaviour
{
    public float speed = 5f; // Viteza de mișcare înainte-înapoi
    public float distance = 10f; // Distanța maximă față de poziția inițială pe axa Z
    public float zigZagAmplitude = 2f; // Amplitudinea mișcării pe axa X (zigzag)
    public float zigZagFrequency = 2f; // Frecvența oscilației zigzag

    private Vector3 startPosition; // Poziția inițială a obiectului
    private Vector3 targetPosition; // Poziția țintă curentă
    private bool movingToTarget = true; // Indică dacă obiectul se deplasează înainte sau înapoi

    void Start()
    {
        // Stochează poziția inițială și setează prima țintă
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.forward * distance; // Mișcare înainte pe axa Z
    }

    void Update()
    {
        // Calculează oscilația pe axa X pentru zigzag
        float xOffset = Mathf.Sin(Time.time * zigZagFrequency) * zigZagAmplitude;

        // Mișcare pe axa Z (față-spate) combinată cu oscilație pe axa X (zigzag)
        Vector3 nextPosition = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );
        nextPosition.x = startPosition.x + xOffset; // Aplica zigzag-ul pe axa X

        // Actualizează poziția obiectului
        transform.position = nextPosition;

        // Verifică dacă obiectul a ajuns la ținta curentă
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Comută între mișcarea înainte și înapoi
            if (movingToTarget)
                targetPosition = startPosition; // Întoarcere la poziția inițială
            else
                targetPosition = startPosition + Vector3.forward * distance; // Mișcare înainte

            movingToTarget = !movingToTarget; // Comută direcția
        }
    }
}

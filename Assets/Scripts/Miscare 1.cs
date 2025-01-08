using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    public float speed = 5f; // Viteză pentru mișcarea liniară
    public Vector3 rotationPoint = Vector3.zero; // Punctul central al rotației (pentru mișcarea orbitală)
    public float rotationSpeed = 50f; // Viteză de rotație (pentru mișcarea orbitală)
    public float orbitRadius = 5f; // Rază orbitală
    public float zigZagAmplitude = 5f; // Amplitudinea oscilației zig-zag
    public float zigZagFrequency = 5f; // Frecvența oscilației zig-zag

    private enum MovementType { None, Linear, Orbital, ZigZag }
    private MovementType currentMovement = MovementType.None;

    void Start()
    {
        // Generează un număr aleator între 1 și 100
        int randomValue = Random.Range(1, 101);

        // Dacă numărul este mai mic de 10, alege o mișcare aleatorie
        if (randomValue < 30)
        {
            currentMovement = MovementType.Linear;
        }else
        if (randomValue < 65 && randomValue > 50)
        {
            currentMovement = MovementType.Orbital;
        }
        if (randomValue < 80 && randomValue > 65)
        {
            currentMovement = MovementType.ZigZag;
        }
    }

    private void Move()
    {
        // Aplică mișcarea în funcție de tipul selectat
        switch (currentMovement)
        {
            case MovementType.Linear:
                MoveLinear();
                break;
            case MovementType.Orbital:
                MoveOrbital();
                break;
            case MovementType.ZigZag:
                MoveZigZag();
                break;
        }
    }

    private void MoveLinear()
    {
        // Mișcare în linie dreaptă
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    private void MoveOrbital()
    {
        // Rotație în jurul unui punct
        transform.RotateAround(rotationPoint, Vector3.forward, rotationSpeed * Time.deltaTime);

        // Menține asteroidul la o rază fixă
        Vector3 offset = (transform.position - rotationPoint).normalized * orbitRadius;
        transform.position = rotationPoint + offset;
    }

    private void MoveZigZag()
    {
        // Mișcare înainte (pe axa X)
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Oscilație pe axa Y
        float yOffset = Mathf.Sin(Time.time * zigZagFrequency) * zigZagAmplitude;
        transform.position = new Vector3(transform.position.x, yOffset, transform.position.z);
    }
}

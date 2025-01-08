using UnityEngine;

public class CircularMovementWithRotation : MonoBehaviour
{
    public Vector3 centerPoint = Vector3.zero; // Punctul central al cercului
    public float radius = 5f; // Raza cercului
    public float rotationSpeed = 50f; // Viteza de rotație pe cerc
    public float selfRotationSpeed = 100f; // Viteza de rotație a obiectului în jurul propriei axe

    private float angle = 0f; // Unghiul curent de rotație pe cerc (în radiani)

    void Update()
    {
        // Crește unghiul în funcție de viteza de rotație
        angle += rotationSpeed * Time.deltaTime;

        // Convertește unghiul în coordonate pe cerc (X și Z, păstrând Y constant)
        float x = centerPoint.x + Mathf.Cos(angle) * radius;
        float z = centerPoint.z + Mathf.Sin(angle) * radius;

        // Actualizează poziția obiectului
        Vector3 newPosition = new Vector3(x, transform.position.y, z);
        transform.position = newPosition;

        // Calculează direcția de mișcare pe cerc (spre care obiectul ar trebui să fie orientat)
        Vector3 direction = (newPosition - centerPoint).normalized;

        // Rotește obiectul astfel încât să fie orientat spre direcția mișcării pe cerc
        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = lookRotation;

        // Adaugă rotație pe axa proprie a obiectului (pentru „înot” sau animație suplimentară)
        transform.Rotate(Vector3.forward, selfRotationSpeed * Time.deltaTime, Space.Self);
    }
}

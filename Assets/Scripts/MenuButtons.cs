using System;
using System.Threading.Tasks;
using UnityEngine;


public class MenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private CircularCameraMovement cameraFollow; // Scriptul CameraFollow

    public float takeoffDuration = 3f;

    private void Start()
    {
        rocketPrefab.transform.position = new Vector3(185, 19.5f, 523.1f);
        rocketPrefab.transform.rotation = Quaternion.Euler(0.4f, -9.91f, 2.139f);
    }

    public void PlayGame()
    {
        Animatie(1);
    }

    private async Task Animatie(int level)
    {
        if (rocketPrefab == null)
        {
            Debug.LogError("Rocket prefab is not assigned!");
            return;
        }

        Transform rocketTransform = rocketPrefab.transform;
        if (rocketTransform == null)
        {
            Debug.LogError("Rocket's transform has been destroyed or is null!");
            return;
        }

        // Dezactivează scriptul de urmărire cameră
        if (cameraFollow != null)
        {
            cameraFollow.enabled = false;
        }

        // Poziția inițială a rachetei
        Vector3 startPosition = rocketTransform.position;

        // Animația de decolare
        float elapsedTime = 0f;
        while (elapsedTime < takeoffDuration)
        {
            elapsedTime += 0.1f;
            float progress = elapsedTime / takeoffDuration;
            rocketTransform.position = startPosition + new Vector3(0, progress * 20f, 0); // Mișcare
            await Task.Delay(20);
        }
        
        startPosition = rocketTransform.position;

        elapsedTime = 3f;
        while (elapsedTime < takeoffDuration)
        {
            elapsedTime += 0.1f;
            float progress = elapsedTime / takeoffDuration;
            rocketTransform.position = startPosition + new Vector3(0, 0, 0);
            rocketTransform.rotation *= Quaternion.Euler(1.1f, 0, 0);
            await Task.Delay(20);
        }
        
        startPosition = rocketTransform.position;
        
        elapsedTime = 0f;
        while (elapsedTime < takeoffDuration)
        {
            elapsedTime += 0.1f;
            float progress = elapsedTime / takeoffDuration;
            rocketTransform.position = startPosition + new Vector3(0, progress * 100f, progress * -300f); // Mișcare
            await Task.Delay(20);
        }

        // Încărcăm scena după animație
        UnityEngine.SceneManagement.SceneManager.LoadScene(level);
    }

    public void PlayArcade()
    {
        Animatie(2);
    }
    
    public void QuitGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}

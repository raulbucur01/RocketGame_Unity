using System.Threading.Tasks;
using UnityEngine;


public class MenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private CameraFollow cameraFollow; // Scriptul CameraFollow

    public float takeoffDuration = 3f;

    public void PlayGame()
    {
        Animatie();
    }

    private async void Animatie()
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
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / takeoffDuration;
            rocketTransform.position = startPosition + new Vector3(0, progress * 10f, progress * 2f); // Mișcare
            await Task.Yield();
        }

        // Încărcăm scena după animație
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void PlayArcade()
    {
        // Load the next scene in the build settings.
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
    
    public void QuitGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}

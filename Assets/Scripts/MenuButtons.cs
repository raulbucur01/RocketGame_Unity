using System.Threading.Tasks;
using UnityEngine;


public class MenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject rocketPrefab; // Racheta atașată din inspector
    public float takeoffDuration = 3f; // Durata animației de decolare

    public void PlayGame()
    {
        // Inițiem animația cu `async`
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
        Vector3 startPosition = rocketTransform.position;

        // Prima etapă: Racheta merge încet înainte și ușor în sus
        float halfDuration = takeoffDuration / 2f;
        float elapsedTime = 0f;
        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / halfDuration;
            rocketTransform.position = startPosition + new Vector3(0, progress * 2f, progress * 1f); // Mișcare lentă
            await Task.Yield(); // Așteaptă următorul cadru
        }

        // A doua etapă: Racheta accelerează vertical
        elapsedTime = 0f;
        startPosition = rocketTransform.position; // Resetează poziția de început
        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / halfDuration;
            rocketTransform.position = startPosition + new Vector3(0, progress * 10f, 0); // Accelerație rapidă
            await Task.Yield(); // Așteaptă următorul
        }

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

using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    public void PlayGame()
    {
        // Load the next scene in the build settings.
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}

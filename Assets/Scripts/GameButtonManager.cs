using UnityEngine;

public class GameButtonManager : MonoBehaviour
{
    public void RestartLevel()
    {
        //load current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
    
    public void QuitGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}

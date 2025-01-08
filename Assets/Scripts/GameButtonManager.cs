using UnityEngine;

public class GameButtonManager : MonoBehaviour
{
    public void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}

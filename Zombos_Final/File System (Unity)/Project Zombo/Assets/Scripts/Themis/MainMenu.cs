using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        // 1 should be our first playable scene, check build settings
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void ToMainMenu()
    {
        // 0 should always be main menu, check build settings
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}

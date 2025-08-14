using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // Load scene by name
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Load scene by index
    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    // Quit the game (works in builds)
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game quit (only works in build)");
    }
}

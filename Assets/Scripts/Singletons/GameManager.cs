using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void GoToSettings()
    {
        LoadScene("Settings");
    }

    public void GoToMainMenu()
    {
        LoadScene("MainMenu");
    }

    public void StartGame()
    {
        LoadScene("Game");
    }
}

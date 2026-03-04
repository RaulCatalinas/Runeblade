using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        if (Application.isEditor) UnityEditor.EditorApplication.isPlaying = false;
        else Application.Quit();
    }

    public void GoToSettings()
    {
        Debug.Log("Going to settings...");
    }

    public void GoToMainMenu()
    {
        LoadScene("MainMenu");
    }

    public void StartGame()
    {
        Debug.Log("Starting game...");
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsGamePaused { get; private set; }
    public GameSaveData? gameData { get; private set; }
    public int currentSlotNumber { get; private set; }

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

    public async void StartLevel(int slotNumber)
    {
        currentSlotNumber = slotNumber;
        gameData = await SaveGameManager.Instance.LoadGame(slotNumber);

        if (gameData == null)
        {
            Debug.Log("Loading Levels/World1/Level1...");
            return;
        }

        Debug.Log($"Loading Levels/World{gameData.Value.currentWorld}/Level{gameData.Value.currentLevel}...");
    }

    public void TogglePauseGame()
    {
        IsGamePaused = !IsGamePaused;
        Time.timeScale = IsGamePaused ? 0 : 1;
    }
}

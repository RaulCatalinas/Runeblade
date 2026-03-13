using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    public static bool IsGamePaused { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        IsGamePaused = true;

        UIManager.Instance.ShowPanel(PanelType.Pause);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        IsGamePaused = false;

        UIManager.Instance.HidePanel(PanelType.Pause);
    }
}

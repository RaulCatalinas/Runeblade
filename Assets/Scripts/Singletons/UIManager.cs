using System.Collections.Generic;

using TMPro;

using UnityEngine;

public enum PanelType
{
    Pause,
    Settings,
}

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text[] saveSlotTexts;
    [SerializeField] private PanelUI pausePanel;
    [SerializeField] private PanelUI settingsPanel;

    private Dictionary<PanelType, PanelUI> panels;

    public static UIManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null) Instance = this;

        panels = new()
        {
            { PanelType.Pause, pausePanel },
            { PanelType.Settings, settingsPanel },
        };
    }

    void Start()
    {
        ChangeSaveSlotsTexts();
    }

    async void ChangeSaveSlotsTexts()
    {
        if (saveSlotTexts == null || saveSlotTexts.Length == 0) return;

        for (int i = 0; i < saveSlotTexts.Length; i++)
        {
            saveSlotTexts[i].text = await SaveGameManager.Instance.GetSaveSlotText(i + 1);
        }
    }

    public void ShowPanel(PanelType type)
    {
        panels[type].gameObject.SetActive(true);
    }

    public void HidePanel(PanelType type)
    {
        panels[type].gameObject.SetActive(false);
    }
}

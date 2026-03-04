using TMPro;

using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text[] saveSlotTexts;

    public static UIManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
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
}

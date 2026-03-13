using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public void HidePanel()
    {
        UIManager.Instance.HidePanel(PanelType.Settings);
    }
}

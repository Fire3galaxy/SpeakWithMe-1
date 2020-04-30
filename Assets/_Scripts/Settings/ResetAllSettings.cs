using UnityEngine;

class ResetAllSettings : MonoBehaviour
{
    Transform canvas;

    void Start()
    {
        canvas = transform.Find("/Canvas");
    }

    public void onClick()
    {
        foreach (Transform child in canvas)
        {
            SettingsInterface setting = child.GetComponentInChildren<SettingsInterface>();
            if (setting != null)
            {
                setting.resetSettings();
            }
        }
        PlayerPrefs.Save();
    }
}
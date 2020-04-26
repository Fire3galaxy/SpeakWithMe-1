using UnityEngine;
using TMPro;

class PlayStyleSettings : MonoBehaviour, SettingsInterface
{
    // Note: Assumption is that dropdown order corresponds to PlayerLoader.HeadsetType
    public static string preferenceKey = "playStyle";

    TMP_Dropdown dropdownMenu;
    // Default value is saved in editor. Save this before changing dropdown.
    int defaultValue;

    void Start()
    {
        dropdownMenu = GetComponent<TMP_Dropdown>();
        defaultValue = dropdownMenu.value;
        dropdownMenu.value = PlayerPrefs.GetInt(preferenceKey, defaultValue);
        PlayerPrefs.SetInt(preferenceKey, dropdownMenu.value);
    }

    public void onValueChanged(int value)
    {
        PlayerPrefs.SetInt(preferenceKey, value);
    }

    public void resetSettings()
    {
        PlayerPrefs.SetInt(preferenceKey, defaultValue);
        dropdownMenu.value = defaultValue;
    }
    
    public bool validateSettings()
    {
        return true;
    }
}
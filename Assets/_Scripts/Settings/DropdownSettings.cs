using UnityEngine;
using TMPro;

// Internal because it conflicts with a class in SteamVR with the same name. Unlikely
// we'll ever use that class, and the best name for this class is DropdownSettings
abstract internal class DropdownSettings : MonoBehaviour, SettingsInterface
{
    // FIXME: Microphone device array could change during gameplay. Look out for this in Update()
    string[] cachedMicDevices;

    abstract public string playerPrefsKey { get; }

    abstract protected void setDropdownToPlayerPref(TMP_Dropdown dropdownMenu);
    abstract public void onValueChanged(int value);
    abstract public void resetSettings();
    public bool validateSettings()
    {
        return PlayerPrefs.HasKey(playerPrefsKey);
    }
}
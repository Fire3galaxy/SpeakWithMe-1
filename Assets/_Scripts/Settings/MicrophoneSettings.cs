using System.Collections.Generic;
using UnityEngine;
using TMPro;

class MicrophoneSettings : DropdownSettings
{
    string[] cachedMicDevices;
    TMP_Dropdown dropdownMenu;

    public static string preferenceKey = "microphone";
    public override string playerPrefsKey => preferenceKey;

    // Default value is 
    void Start()
    {
        // FIXME: Microphone device array could change during gameplay. Look out for this in Update()
        cachedMicDevices = Microphone.devices;

        dropdownMenu = GetComponent<TMP_Dropdown>();
        dropdownMenu.AddOptions(new List<string>(cachedMicDevices));
        setDropdownToPlayerPref(dropdownMenu);
    }

    protected override void setDropdownToPlayerPref(TMP_Dropdown dropdownMenu)
    {
        int micSettingIndex = getPreferredDeviceIndex();
        // Index 0 is "Select an option"
        dropdownMenu.value = micSettingIndex == -1 ? 0 : micSettingIndex + 1;
    }

    public override void onValueChanged(int value)
    {
        if (value == 0) return;
        PlayerPrefs.SetString(playerPrefsKey, Microphone.devices[value - 1]);
    }

    public override void resetSettings()
    {
        PlayerPrefs.DeleteKey(playerPrefsKey);
        dropdownMenu.value = 0;
    }

    public static int getPreferredDeviceIndex()
    {
        if (!PlayerPrefs.HasKey(preferenceKey)) return -1;

        string prefMic = PlayerPrefs.GetString(preferenceKey);
        string[] devices = Microphone.devices;

        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].Equals(prefMic))
            {
                return i;
            }
        }

        return -1;
    }
}
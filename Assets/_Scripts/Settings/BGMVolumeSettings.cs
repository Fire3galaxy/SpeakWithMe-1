using UnityEngine;
using UnityEngine.UI;

class BGMVolumeSettings : MonoBehaviour, SettingsInterface
{
    string playerPrefsKey = "bgmVolume";
    float defaultVolume;
    Slider volumeSlider;

    void Start()
    {
        // Object start with default value. Save this before changing value to preference.
        volumeSlider = GetComponentInChildren<Slider>();
        defaultVolume = volumeSlider.value;

        // Set slider to preference if exists. Else, save default value.
        if (PlayerPrefs.HasKey(playerPrefsKey)) 
            volumeSlider.value = PlayerPrefs.GetFloat(playerPrefsKey);
        else 
            PlayerPrefs.SetFloat("bgmVolume", volumeSlider.value);
    }

    // Attached to volume slider in settings
    public void onValueChanged(float value)
    {
        PlayerPrefs.SetFloat("bgmVolume", value);
    }

    public bool validateSettings()
    {
        return true; // PlayerPrefs should already have bgmVolume due to Start()
    }

    public void resetSettings()
    {
        PlayerPrefs.DeleteKey(playerPrefsKey);
        volumeSlider.value = defaultVolume;
    }
}
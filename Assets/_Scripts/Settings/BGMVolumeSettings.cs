using UnityEngine;
using UnityEngine.UI;

class BGMVolumeSettings : MonoBehaviour, SettingsInterface
{
    string playerPrefsKey = "bgmVolume";
    float defaultVolume;
    Slider volumeSlider;

    void Start()
    {
        volumeSlider = GetComponentInChildren<Slider>();
        defaultVolume = volumeSlider.value; // Editor determines default value.

        if (PlayerPrefs.HasKey(playerPrefsKey)) 
            volumeSlider.value = PlayerPrefs.GetFloat(playerPrefsKey);
        // Save a DEFAULT value in case user doesn't change this value.
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
using UnityEngine;
using UnityEngine.UI;

abstract class VolumeSettings : MonoBehaviour, SettingsInterface
{
    float defaultVolume;
    Slider volumeSlider;

    virtual internal void Start()
    {
        // Object start with default value. Save this before changing value to preference.
        volumeSlider = GetComponentInChildren<Slider>();
        defaultVolume = volumeSlider.value;

        // Set slider to preference if exists. Else, save default value.
        if (PlayerPrefs.HasKey(getPlayerPrefsKey())) 
            volumeSlider.value = PlayerPrefs.GetFloat(getPlayerPrefsKey());
        else 
            PlayerPrefs.SetFloat(getPlayerPrefsKey(), volumeSlider.value);
    }

    // Attached to volume slider in settings
    public virtual void onValueChanged(float value)
    {
        PlayerPrefs.SetFloat(getPlayerPrefsKey(), value);
    }

    public virtual bool validateSettings()
    {
        return true; // this assumes there's never a case where PlayerPrefs isn't set.
    }

    public virtual void resetSettings()
    {
        PlayerPrefs.SetFloat(getPlayerPrefsKey(), defaultVolume);
        volumeSlider.value = defaultVolume;
    }

    abstract internal string getPlayerPrefsKey();
}
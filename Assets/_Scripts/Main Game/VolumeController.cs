using UnityEngine;

abstract class VolumeController : MonoBehaviour
{
    bool loggedError = false;
    AudioSource audioSource;    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        UpdateVolume();
    }

    void UpdateVolume()
    {
        if (!PlayerPrefs.HasKey(getPlayerPrefsKey()))
        {
            if (!loggedError) 
            {
                Debug.LogError("No volume saved for " + getPlayerPrefsKey());
                loggedError = true;
            }
            return;
        }

        audioSource.volume = PlayerPrefs.GetFloat(getPlayerPrefsKey());
    }

    abstract internal string getPlayerPrefsKey();
}
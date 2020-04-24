using UnityEngine;

class BGMVolumeController : MonoBehaviour
{
    bool loggedError = false;
    AudioSource bgmAudioSource;

    void Start()
    {
        bgmAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        UpdateVolume();
    }

    void UpdateVolume()
    {
        if (!loggedError && !PlayerPrefs.HasKey("bgmVolume"))
        {
            Debug.LogError("This case should not occur. Default volume should at least be saved.");
            loggedError = true;
            return;
        }

        bgmAudioSource.volume = PlayerPrefs.GetFloat("bgmVolume");
    }
}
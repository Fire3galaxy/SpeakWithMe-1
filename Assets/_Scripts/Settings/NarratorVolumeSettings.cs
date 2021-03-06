using UnityEngine;

class NarratorVolumeSettings : VolumeSettings
{
    static public string playerPrefsKey = "narratorVolume";

    [Tooltip("Random audio clips to play while user adjusts narrator settings")]
    public AudioClip[] audioClips;

    AudioSource narratorAudioSource;
    bool isAfterStart = false;

    override protected void Start()
    {
        // Assumed to be attached to Settings Menu/Settings Narrator Volume/Slider
        narratorAudioSource = transform.parent.parent.Find("Narrator Audio")
                                .GetComponent<AudioSource>();
        base.Start(); // changes value of dropdown, triggering onValueChanged().
        isAfterStart = true;
    }

    override protected string getPlayerPrefsKey() {
        return playerPrefsKey;
    }

    override public void onValueChanged(float value)
    {
        if (!isAfterStart) return;

        if (!narratorAudioSource.isPlaying)
        {
            int index = getRandomAudioIndex();
            narratorAudioSource.clip = audioClips[index];
            narratorAudioSource.Play();
        }

        PlayerPrefs.SetFloat(getPlayerPrefsKey(), value);
    }

    int getRandomAudioIndex()
    {
        return (int) Mathf.Clamp(Random.Range(0f, audioClips.Length), 0f, audioClips.Length - 1);
    }
}
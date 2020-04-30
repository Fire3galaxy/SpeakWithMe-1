using UnityEngine;
using UnityEngine.UI;

class NarratorVolumeSettings : VolumeSettings
{
    static public string playerPrefsKey = "narratorVolume";

    [Tooltip("Random audio clips to play while user adjusts narrator settings")]
    public AudioClip[] audioClips;

    AudioSource narratorAudioSource;

    override internal void Start()
    {
        narratorAudioSource = transform.Find("/Narrator Audio").GetComponent<AudioSource>();
        base.Start();
    }

    override internal string getPlayerPrefsKey() {
        return playerPrefsKey;
    }

    override public void onValueChanged(float value)
    {
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
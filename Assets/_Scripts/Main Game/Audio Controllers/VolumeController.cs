using UnityEngine;

/* VolumeController.cs
 * Keeps volume for audioSource updated to set value in PlayerPrefs. Player prefs
 * key defined by child.
 * 
 * A note about pausedByPlayer: it does not truly reflect the AudioSource state.
 * AudioSource is a class that gets directly accessed by multiple classes, by design,
 * and so I want to avoid assigning VolumeController a role of "total audio control".
 * Outside classes should still check the audio source directly for audio-related
 * state. The pausedByPlayer variable's only role is to let other classes know that
 * audio was paused by this class specifically because of PlayerSettingsControls.
 * This variable is implemented because otherwise there can be a race condition 
 * between VolumeController unpausing the audio and other classes that expect the
 * audio to be playing or paused for their state machines (sadly, AudioSource has
 * no variable that distinguishes between paused and stopped/no clip).
 */
abstract class VolumeController : MonoBehaviour
{
    bool loggedError = false;
    AudioSource audioSource;

    public bool pausedByPlayer {get; private set;} = false;

    virtual protected void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        updatePausedByPlayerState();
        updateVolume();
    }

    void updatePausedByPlayerState()
    {
        if (PlayerSettingsControls.paused)
        {
            // Only run below code once.
            if (!pausedByPlayer)
            {
                pausedByPlayer = true;
                audioSource.Pause(); // does nothing if no audio playing
            }
        }
        else 
        {
            // Only run below code once.
            if (pausedByPlayer)
            {
                pausedByPlayer = false;
                audioSource.UnPause(); // does nothing if no audio playing
            }
        }            
    }

    void updateVolume()
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

    abstract protected string getPlayerPrefsKey();
}
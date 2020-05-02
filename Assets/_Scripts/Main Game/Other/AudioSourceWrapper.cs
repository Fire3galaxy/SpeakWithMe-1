using UnityEngine;

class AudioSourceWrapper : PlayPauseBehaviour
{
    public bool pauseAudioDuringGamePause = true;
    AudioSource audioSource;
    
    public AudioState currentState {get; private set;}

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        currentState = AudioState.NotPlaying;
    }

    public void Play(AudioClip audioClip)
    {
        currentState = AudioState.Playing;
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    // Combines PlayScheduled and SetScheduledEndTime. Duration == -1 just ignores SetScheduledEndTime().
    public void PlayScheduled(AudioClip audioClip, double startTime, double duration = -1)
    {
        currentState = AudioState.Playing;
        audioSource.clip = audioClip;
        audioSource.PlayScheduled(startTime);
        if (duration != -1) audioSource.SetScheduledEndTime(startTime + duration);
    }

    public void Stop()
    {
        currentState = AudioState.NotPlaying;
        audioSource.Stop();
    }

    void Pause()
    {
        currentState = AudioState.Paused;
        audioSource.Pause();
    }

    void UnPause()
    {
        currentState = AudioState.Playing;
        audioSource.UnPause();
    }

    protected override void UpdatePlay()
    {
        if (pauseAudioDuringGamePause && currentState == AudioState.Paused)
        {
            currentState = AudioState.Playing;
            audioSource.UnPause();
        }
        
        // AudioSource lacks a pretty basic feature: how to tell when the clip actually ended.
        // I find the best indication is that (1) audio isn't playing and (2) audio time has reset
        // to 0.
        if (currentState == AudioState.Playing && !audioSource.isPlaying && audioSource.time == 0)
        {
            currentState = AudioState.NotPlaying;
        }
    }

    protected override void UpdatePause()
    {
        if (pauseAudioDuringGamePause && currentState != AudioState.Paused)
        {
            currentState = AudioState.Paused;
            audioSource.Pause();
        }
    }

    // Another basic feature AudioSource lacks: distinguishing between paused and not playing.
    public enum AudioState
    {
        NotPlaying, Paused, Playing
    }
}
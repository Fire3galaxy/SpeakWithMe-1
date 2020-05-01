using UnityEngine;

public class Narrator : MonoBehaviour {
    public AudioClip[] narratorClips;

    int clipNum = 0;
    OnNarratorCompleteListener scriptLogic;
    AudioSource narratorAudio;
    VolumeController narratorVolumeController;
    bool isPlaying = false;

	// Use this for initialization
	void Start () {
        GameObject narratorAudioObject = GameObject.Find(PlayerLoader.playerPath() + 
                                                         "/Narrator Audio");
        narratorAudio = narratorAudioObject.GetComponent<AudioSource>();
        narratorVolumeController = narratorAudioObject.GetComponent<VolumeController>();
	}
	
	// Update is called once per frame
	void Update () {
        // Clip has ended
		if (isPlaying && !narratorAudio.isPlaying && !PlayerSettingsControls.paused && 
            !narratorVolumeController.pausedByPlayer)
        {
            isPlaying = false;
            clipNum++;
            scriptLogic.onNarratorClipCompleted();
        }
	}

    public void StartNarrator(OnNarratorCompleteListener caller)
    {
        scriptLogic = caller;
        narratorAudio.clip = narratorClips[clipNum];
        narratorAudio.Play();
        isPlaying = true;
    }

    public interface OnNarratorCompleteListener
    {
        void onNarratorClipCompleted();
    }
}

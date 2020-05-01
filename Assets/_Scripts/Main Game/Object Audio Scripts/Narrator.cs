using UnityEngine;

using AudioState = AudioSourceWrapper.AudioState;

class Narrator : MonoBehaviour {
    public AudioClip[] narratorClips;

    int clipNum = 0;
    OnNarratorCompleteListener scriptLogic;
    AudioSourceWrapper narratorAudioSourceWrapper;
    bool isPlaying = false;

	void Start () {
        narratorAudioSourceWrapper = GameObject.Find(PlayerLoader.playerPath() + "/Narrator Audio")
                                        .GetComponent<AudioSourceWrapper>();
	}

    void Update()
    {
        // A clip starts playing when StartNarrator() is called. This code waits for
        // the clip to ends, then notifies the callee we're done.
		if (isPlaying && narratorAudioSourceWrapper.currentState == AudioState.NotPlaying)
        {
            isPlaying = false;
            clipNum++;
            scriptLogic.onNarratorClipCompleted();
        }
    }

    public void StartNarrator(OnNarratorCompleteListener caller)
    {
        scriptLogic = caller;
        narratorAudioSourceWrapper.Play(narratorClips[clipNum]);
        isPlaying = true;
    }

    public interface OnNarratorCompleteListener
    {
        void onNarratorClipCompleted();
    }
}
